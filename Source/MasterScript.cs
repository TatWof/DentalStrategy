using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MasterScript : MonoBehaviour
{
    private static WaitForSecondsRealtime _wait = new(0.5f);
    public string lastlastlastActiveScene;
    public string lastlastActiveScene;
    public string lastActiveScene;

    public DataMasterScript datamaster;
    public string scenarioToLoad;
    public string saveToLoad;
    public bool isloadingscenario;
    public bool isloadingsave;
    public GameMasterScript gameMaster;

    void Start()
    {
        datamaster = GameObject.Find("DataMaster").GetComponent<DataMasterScript>();
        if (datamaster.GrabSettings()) datamaster.ApplySettings();
        datamaster.GrabProgression();
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game")
            gameMaster = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();


        if (isloadingscenario && SceneManager.GetActiveScene().name == "Game")
        {
            isloadingsave = false;
            datamaster.GrabScenario(scenarioToLoad);
            gameMaster.LoadScenario();
            isloadingscenario = false;
        }
        if (isloadingsave && SceneManager.GetActiveScene().name == "Game")
        {
            datamaster.GrabSave(saveToLoad);
            gameMaster = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();
            gameMaster.QuickLoad();
            isloadingsave = false;
        }
    }
    


    public IEnumerator LoadPreviousScene()
    {
        saveToLoad = "qsave";
        if (SceneManager.GetActiveScene().name == "Settings" && lastActiveScene == "SoundSettings")
            lastActiveScene = lastlastlastActiveScene;

        LoadScene(lastActiveScene);
        isloadingsave = true;
        yield return _wait;

    }

    public void OpenSaveLoadFilesScene()
    {
        datamaster.QuickSave();
        LoadScene("SaveLoad");
    }

    public void OpenSettingsScene()
    {
        datamaster.QuickSave();
        LoadScene("Settings");
    }

    public void OpenSoundSettingsScene()
    {
        datamaster.QuickSave();
        LoadScene("SoundSettings");
    }

    public void OpenMainMenuScene()
    {
        datamaster.QuickSave();
        LoadScene("MainMenu");
    }

    public void OpenLevelSelectScene()
    {
        datamaster.QuickSave();
        LoadScene("LevelSelect");
    }

    public void UpdateProgress()
    {
        switch(datamaster.scenario.scenarioName)
        {
            case "tutorial1": if (datamaster.progress.tutorials < 1) datamaster.progress.tutorials = 1; break;
            case "tutorial2": if (datamaster.progress.tutorials < 2) datamaster.progress.tutorials = 2; break;
            case "tutorial3": if (datamaster.progress.tutorials < 3) datamaster.progress.tutorials = 3; break;
            case "tutorial4": if (datamaster.progress.tutorials < 4) datamaster.progress.tutorials = 4; break;
            case "tutorial5": if (datamaster.progress.tutorials < 5) datamaster.progress.tutorials = 5; break;
            case "tutorial6": if (datamaster.progress.tutorials < 6) datamaster.progress.tutorials = 6; break;
            case "tutorial7": if (datamaster.progress.tutorials < 7) datamaster.progress.tutorials = 7; break;
            default: break;
        }
        datamaster.NewProgress(datamaster.progress);
    }
    
    public void OpenNextLevel()
    {
        bool end = false;

        UpdateProgress();

        switch(datamaster.scenario.scenarioName)
        {
            case "tutorial1": scenarioToLoad = "tutorial2"; break;
            case "tutorial2": scenarioToLoad = "tutorial3"; break;
            case "tutorial3": scenarioToLoad = "tutorial4"; break;
            case "tutorial4": scenarioToLoad = "tutorial5"; break;
            case "tutorial5": scenarioToLoad = "tutorial6"; break;
            case "tutorial6": scenarioToLoad = "tutorial7"; break;
            case "tutorial7": end = true; break;
            default: break;
        }

        if (end)
        {
            OpenMainMenuScene();
            return;
        }
        StartCoroutine(OpenScenario());
        // OpenScenario();
    }

    void OpenGameScene()
    {
        LoadScene("Game");
    }

    public IEnumerator OpenScenario()
    {
        datamaster.GrabScenario(scenarioToLoad);
        if (SceneManager.GetActiveScene().name != "Game") OpenGameScene();
        isloadingsave = false;
        isloadingscenario = true;
        yield return _wait;
    }

    // public void OpenScenario()
    // {
    //     datamaster.GrabScenario(scenarioToLoad);
    //     OpenGameScene();
    //     //yield return _wait;
    //     GameMasterScript gameMaster = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();
    //     Debug.Log(gameMaster.transform.gameObject.name);
    //     gameMaster.LoadScenario();
    // }

    public IEnumerator OpenSaveGame()
    {
        datamaster.GrabSave(saveToLoad);
        if (SceneManager.GetActiveScene().name != "Game") OpenGameScene();
        isloadingsave = true;
        yield return _wait;
    }
    
    public void OpenControlsScene()
    {
        LoadScene("Controls");
    }

    void LoadScene(string scenename)
    {
        lastlastlastActiveScene = lastlastActiveScene;
        lastlastActiveScene = lastActiveScene;
        lastActiveScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(scenename);

    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void ExitApp()
    {
        Application.Quit();
    }
}
