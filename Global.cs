using Godot;
using System;
using Mono.Data.Sqlite;

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
    private string myDatabaseConnection;
    public static SqliteConnection connection;
    public static SqliteCommand command;
    public static string queryString;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SaveGameLoaded = false;
        TwitchMode = false;

        // initialize database
        InitDatabaseConnection();

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

    public void InitDatabaseConnection()
    {
        try
        {
           // set and open database connection
            myDatabaseConnection = "URI=file:viewerscore.db";
            connection = new SqliteConnection(myDatabaseConnection);
            connection.Open();

            // get the SQLite version by query string
            queryString = "SELECT SQLITE_VERSION()";

            // create commands
            command = new SqliteCommand(queryString, connection);
            string version = command.ExecuteScalar().ToString();
            Log.log.Debug($"SQLite version: {version}"); 

            // update viewerscore db
            //...

        }
        catch(SqliteException ex)
        {
            // report errors
            Log.log.Error("Exception on SQLite init", ex);
        }
        finally
        {
            if(command != null)
            {
                command.Dispose();
            }

            if(connection != null)
            {
                try
                {
                    connection.Close();
                }
                catch(SqliteException ex)
                {
                    Log.log.Error("Cannot close SQLite connection", ex);
                }
                finally
                {
                    connection.Dispose();
                }
            }
        }      

    }

    #endregion
}
