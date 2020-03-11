using Godot;
using System;

using System.Collections.Generic;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

// once at program start necessary
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "config/log4net.config")]

public class Global : Node
{
    public Node Gui;
    public Node CurrentGuiScene;
    public const string GUI_START_PATH = "res://gui/title_menu/TitleMenu.tscn";

    public Node World;
    public Node CurrentWorldScene;
    public const string WORLD_START_PATH = "res://scenes/startup/TitleScene.tscn";
    public const string WORLD_ROOM_PATH = "res://scenes/dungeon/rooms/Room.tscn";

    public PlayerAttributes PlayerAttributes;
    public bool SaveGameLoaded;
    public bool TwitchMode;
    public bool SwitchRoomMode;
    public bool RoomSpawnLeft;

    // database
    public static ISession connection;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SaveGameLoaded = false;
        TwitchMode = false;
        SwitchRoomMode = false;
        RoomSpawnLeft = true;

        // initialize database
        InitHibernate();
        //WebSocketImpl wsi = WebSocketImpl.getInstance();

        // get gui and world node
        Viewport root = GetTree().Root;
        Gui = root.GetNode("Main/CanvasLayer/GUI");
        World = root.GetNode("Main/World");

        // set current gui scene child
        PackedScene currentGui = (PackedScene)ResourceLoader.Load(GUI_START_PATH);
        CurrentGuiScene = currentGui.Instance();
        Gui.AddChild(CurrentGuiScene);

        // set current world scene child
        PackedScene currentWorld = (PackedScene)ResourceLoader.Load(WORLD_START_PATH);
        CurrentWorldScene = currentWorld.Instance();
        World.AddChild(CurrentWorldScene);

        // set initial player attributes
        PlayerAttributes = new PlayerAttributes();

        // load possible save data
        LoadGame();
        Log.log.Info(PlayerAttributes.Name + " " + PlayerAttributes.Level + " " + PlayerAttributes.Xp);
    }

    #region Scene Handling
    public void ChangeScene(string guiScenePath, string worldScenePath)
    {
        if (!String.IsNullOrEmpty(guiScenePath) || !String.IsNullOrEmpty(worldScenePath))
        {
            CallDeferred(nameof(DeferredChangeScene), guiScenePath, worldScenePath);
        }
    }

    public void DeferredChangeScene(string guiScenePath, string worldScenePath)
    {
        // CurrentGuiScene.Free();
        // CurrentWorldScene.Free();

        PackedScene newGuiScene = null;
        PackedScene newWorldScene = null;

        if (!String.IsNullOrEmpty(guiScenePath))
        {
            CurrentGuiScene.Free();
            newGuiScene = (PackedScene)ResourceLoader.Load(guiScenePath);
        }

        if (!String.IsNullOrEmpty(worldScenePath))
        {
           CurrentWorldScene.Free();
           newWorldScene = (PackedScene)ResourceLoader.Load(worldScenePath);
        }

        if (newGuiScene != null)
        {
            CurrentGuiScene = newGuiScene.Instance();
            Gui.AddChild(CurrentGuiScene);
            GD.Print("GUI changed");
        }

        if (newWorldScene != null)
        {
            CurrentWorldScene = newWorldScene.Instance();
            World.AddChild(CurrentWorldScene);
            GD.Print("World changed");
        }

    }

    #endregion

    #region PlayerData
    public void SaveGame()
    {
        File saveGame = new File();
        saveGame.Open("user://duckest_dungeon.save", File.ModeFlags.Write);

        // transform player data to json
        var playerData = PlayerAttributes.Save();
        saveGame.StoreLine(JSON.Print(playerData));

        saveGame.Close();
        SaveGameLoaded = true;
    }

    public void LoadGame()
    {
        File saveGame = new File();
        if (!saveGame.FileExists("user://duckest_dungeon.save")) return;
        
        saveGame.Open("user://duckest_dungeon.save", File.ModeFlags.Read);
        
        // parse json to player dictionary
        var playerData = (Godot.Collections.Dictionary)JSON.Parse(saveGame.GetLine()).Result;
        Log.log.Debug(playerData);
        saveGame.Close();

        // TODO -> check if error
        PlayerAttributes.Load(playerData);
        SaveGameLoaded = true;
    }

    #endregion

    #region Database

    private static void InitHibernate()
    {
        try
        {
            Log.log.Info("Loading Hibernate Config");
            // read hibernate.cfg.xml, initializing hibernate
            var cfg = new Configuration();
            cfg.Configure("config/hibernate.cfg.xml");

            IEnumerable<string> Files = System.IO.Directory
                .EnumerateFiles("config/mappings", "*.hbm.xml", System.IO.SearchOption.TopDirectoryOnly);
            
            foreach(string File in Files)
                cfg.AddFile(File);
                
            // Create or update tables in DB
            new SchemaUpdate(cfg).Execute(false, true);
            
            // Get ourselves an NHibernate Session
            var sessions = cfg.BuildSessionFactory();
            connection = sessions.OpenSession();
        }
        catch (Exception e)
        {
            Log.log.Error("Error on create hibernate session", e);
        }

    }

    #endregion

    #region Signals

    public void OnSecExited(bool exitedRight)
    {
        if (!SwitchRoomMode)
        {
            GD.Print(exitedRight);

            try
            {
                // get Map node
                Map map = Gui.GetNode<Map>("DungeonMenu/MarginContainer/HBoxContainer/Minimap/MarginContainer/MapContainer/MapView/Map");
                map.ChangePlayerPosition(exitedRight);
            }
            catch (Exception e)
            {
                Log.log.Error("Error on getting Map scene", e);
            }
        }
    }

    public void OnStartEntered()
    {
        RoomSpawnLeft = false;
        ChangeScene(null, WORLD_ROOM_PATH);

        // get Map node
        Map map = Gui.GetNode<Map>("DungeonMenu/MarginContainer/HBoxContainer/Minimap/MarginContainer/MapContainer/MapView/Map");
        SwitchRoomMode = true;
        map.ChangePlayerPosition(false);
    }

    public void OnEndEntered()
    {
        RoomSpawnLeft = true;
        ChangeScene(null, WORLD_ROOM_PATH);

        // get Map node
        Map map = Gui.GetNode<Map>("DungeonMenu/MarginContainer/HBoxContainer/Minimap/MarginContainer/MapContainer/MapView/Map");
        SwitchRoomMode = true;
        map.ChangePlayerPosition(true);
    }

    #endregion
}
