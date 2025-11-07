using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class DataMasterScript : MonoBehaviour
{
    public SaveData savedata;
    public Scenario scenario;
    public ProgressionData progress;
    public Settings settings;
    public bool edit;
    public bool gameLoadedOnce = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GrabSettings();
        GrabProgression();
        ApplySettings();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "LevelEdit" && Input.GetKeyDown(KeyCode.Space) && edit)
        {
            CreateScenairo();
        }

        if (SceneManager.GetActiveScene().name == "Game" && !gameLoadedOnce)
        {
            gameLoadedOnce = true;
        }
    }

    public void CleanSaves()
    {
        DeleteFile(Application.persistentDataPath, "save1");
        DeleteFile(Application.persistentDataPath, "save2");
        DeleteFile(Application.persistentDataPath, "save3");
    }
    
    void DeleteFile(string path, string filename)
    {
        string truepath = Path.Combine(path, filename);
        Debug.Log("Deleted file at: " + truepath);
        File.Delete(truepath);
    }

    public bool SaveFileExists(string name)
    {
        string path = Path.Combine(Application.persistentDataPath, name);
        return File.Exists(path);
    }

    public void ApplySettings()
    {
        GameObject camera = GameObject.Find("Game Camera");
        if (camera != null)
        {
            CameraScript camerascript = camera.GetComponent<CameraScript>();
            camerascript.cameraSpeed = settings.cameraSpeed;
        }

        SoundMasterScript soundmaster = GameObject.Find("SoundMaster").GetComponent<SoundMasterScript>();

        soundmaster.mixer.SetFloat("MasterVolume", (float)Math.Log10(settings.soundsettings.master) * 20);
        soundmaster.mixer.SetFloat("EffectVolume", (float)Math.Log10(settings.soundsettings.effects) * 20);
        soundmaster.mixer.SetFloat("MusicVolume", (float)Math.Log10(settings.soundsettings.bgm) * 20);

        NewSettings(settings);
    }

    public void ApplyProgress()
    {
        NewProgress(progress);
    }
    
    public void ClearProgress()
    {
        progress.scenarios = 0;
        progress.tutorials = 0;
        DeleteFile(Application.persistentDataPath, "progress");
    }
    
    public void CreateScenairo()
    {
        GetCurrentGameState();

        scenario.terrain = savedata.terrain;
        scenario.units = savedata.units;
        scenario.scenarioName = savedata.scenarioName;
        scenario.gameMode = savedata.gameMode;
        scenario.AIlevel = savedata.AIlevel;
        scenario.activeTeam = savedata.activeTeam;
        scenario.turnLimit = savedata.turnLimit;
        scenario.extractionLimit = savedata.extractionLimit;
        scenario.unitcount = savedata.unitcount;
        scenario.unitLimit = savedata.unitLimit;

        string path = Path.Combine(Application.dataPath, "Scenarios/", scenario.scenarioName);
        WriteData(path, scenario);
    }

    public bool GrabScenario(string scenarioname)
    {
        string path = Path.Combine(Application.dataPath, "Scenarios/", scenarioname);
        string temp = ReadData(path);
        if (temp == default) return false;
        scenario = JsonUtility.FromJson<Scenario>(temp);

        return true;
    }

    public bool GrabSave(string savename)
    {
        string path = Path.Combine(Application.persistentDataPath, savename);
        string temp = ReadData(path);
        if (temp == default) return false;
        savedata = JsonUtility.FromJson<SaveData>(temp);
        return true;
    }

    public bool GrabProgression()
    {
        string path = Path.Combine(Application.persistentDataPath, "progress");
        string temp = ReadData(path);
        if (temp == default) return false;
        progress = JsonUtility.FromJson<ProgressionData>(temp);
        return true;
    }

    public bool GrabSettings()
    {
        string path = Path.Combine(Application.persistentDataPath, "settings");
        string temp = ReadData(path);
        if (temp == default) return false;
        settings = JsonUtility.FromJson<Settings>(temp);
        return true;
    }

    public void SaveGame(string savename)
    {
        if (savedata == null) return;
        if (SceneManager.GetActiveScene().name == "Game") GetCurrentGameState();

        string path = Path.Combine(Application.persistentDataPath, savename);
        WriteData(path, savedata);

    }

    public void NewProgress(ProgressionData pd)
    {
        progress = pd;
        string path = Path.Combine(Application.persistentDataPath, "progress");
        WriteData(path, progress);
    }

    public void NewSettings(Settings settings)
    {
        string path = Path.Combine(Application.persistentDataPath, "settings");
        WriteData(path, settings);
    }
    
    public void NewSoundSettings(SoundSettings soundSettings)
    {
        settings.soundsettings = soundSettings;
        NewSettings(settings);
    }

    public void QuickSave()
    {
        if(SceneManager.GetActiveScene().name == "Game") GetCurrentGameState();
        QuickSave(savedata);

    }

    private void QuickSave(SaveData data)
    {
        string path = Path.Combine(Application.persistentDataPath, "qsave");
        WriteData(path, data);
    }

    public SaveData QuickLoad()
    {
        string path = Path.Combine(Application.persistentDataPath, "qsave");
        savedata = JsonUtility.FromJson<SaveData>(ReadData(path));
        return savedata;
    }
    private string ReadData(string filepath)
    {
        if (File.Exists(filepath)) return File.ReadAllText(filepath);
        else return default;
    }

    private void WriteData<T>(string filepath, T data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filepath, json);
        Debug.Log("written to: " + filepath);
    }

    List<Terrain> FindTerrains()
    {
        BoardMasterScript bms = GameObject.FindGameObjectWithTag("BoardMaster").GetComponent<BoardMasterScript>();
        Tilemap tilemap = bms.tilemap;
        List<Terrain> terrain = new();

        int i = 0, j, k;

        for (j = bms.maxW; j <= bms.maxE; ++j)
            for (k = bms.maxS; k <= bms.maxN; ++k)
            {
                Sprite sprite = tilemap.GetSprite(new Vector3Int(j, k, 0));
                if (sprite == null || sprite.name == "void") continue;
                
                terrain.Add(new()
                {
                    posX = j,
                    posY = k,
                    sprite = sprite.name
                }
                );
                ++i;
            }

        return terrain;
    }

    private List<Units> FindUnits()
    {
        GameObject[] list = GameObject.FindGameObjectsWithTag("Unit");
        List<Units> units = new();

        foreach (var item in list)
        {
            UnitScript us = item.GetComponent<UnitScript>();
            if (us.ghost == true) continue;

            units.Add(new()
            {
                posX = us.posX,
                posY = us.posY,
                name = us.name,
                team = TeamToString(us.team)
            }
            );
        }
        return units;
    }

    private void GetCurrentGameState()
    {
        List<Terrain> terrain = FindTerrains();
        List<Units> units = FindUnits();
        GameMasterScript gms = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();

        savedata.terrain = terrain;
        savedata.units = units;
        savedata.gameMode = gms.gameMode;
        savedata.AIlevel = gms.AIlevel;
        savedata.scenarioName = gms.scenarioName;
        savedata.activeTeam = TeamToString(gms.activeTeam);
        savedata.gameOver = gms.gameOver;
        savedata.turn = gms.turn;
        savedata.turnLimit = gms.turnLimit;
        savedata.extracted = gms.extracted;
        savedata.extractionLimit = gms.extractionLimit;
        savedata.unitcount = gms.unitcount;
        savedata.unitLimit = gms.unitLimit;
    }

    public GameMasterScript.Team StringToTeam(string thing)
    {
        return thing switch
        {
            "player1" => GameMasterScript.Team.PLAYER1,
            "player2" => GameMasterScript.Team.PLAYER2,
            "enemy1" => GameMasterScript.Team.ENEMY1,
            "enemy2" => GameMasterScript.Team.ENEMY2,
            _ => default,
        };
    }
    
    public string TeamToString(GameMasterScript.Team thing)
    {
        return thing switch
        {
            GameMasterScript.Team.PLAYER1 => "player1",
            GameMasterScript.Team.PLAYER2 => "player2",
            GameMasterScript.Team.ENEMY1 => "enemy1",
            GameMasterScript.Team.ENEMY2 => "enemy2",
            _ => null,
        };
    }

}
