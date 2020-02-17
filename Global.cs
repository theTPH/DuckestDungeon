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

    public PlayerAttributes PlayerAttributes;
    public bool SaveGameLoaded;
    public bool TwitchMode;

    // database
    public static ISession connection;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SaveGameLoaded = false;
        TwitchMode = false;

        // initialize database
        InitHibernate();

        // get gui and world node
        Viewport root = GetTree().Root;
        Gui = root.GetNode("Main/GUI");
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
        CurrentGuiScene.Free();
        CurrentWorldScene.Free();

        PackedScene newGuiScene = null;
        PackedScene newWorldScene = null;

        // TODO assert path

        if (!String.IsNullOrEmpty(guiScenePath))
        {
            newGuiScene = (PackedScene)ResourceLoader.Load(guiScenePath);
        }

        if (!String.IsNullOrEmpty(worldScenePath))
        {
           newWorldScene = (PackedScene)ResourceLoader.Load(worldScenePath);
        }

        if (newGuiScene != null)
        {
            CurrentGuiScene = newGuiScene.Instance();
            Gui.AddChild(CurrentGuiScene);
        }

        if (newWorldScene != null)
        {
            CurrentWorldScene = newWorldScene.Instance();
            World.AddChild(CurrentWorldScene);
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
}
