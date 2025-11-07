using UnityEngine;
using UnityEngine.UIElements;

public class GameOverScript : MonoBehaviour
{
    public GameObject origin;
    public UIDocument document;

    Button settings, soundsettings, levelselect, saveload, mainmenu, restart, exit, nextlevel;
    Label title;

    public DataMasterScript datamaster;
    public GameMasterScript gamemaster;
    public MasterScript master;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    void Startup()
    {
        datamaster = GameObject.Find("DataMaster").GetComponent<DataMasterScript>();
        gamemaster = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();
        master = GameObject.Find("Master").GetComponent<MasterScript>();
        
        title = document.rootVisualElement.Q<Label>("Title");

        settings = document.rootVisualElement.Q<Button>("Settings");
        settings.RegisterCallback<ClickEvent>(Settings);

        exit = document.rootVisualElement.Q<Button>("Exit");
        exit.RegisterCallback<ClickEvent>(Exit);

        soundsettings = document.rootVisualElement.Q<Button>("SoundSettings");
        soundsettings.RegisterCallback<ClickEvent>(SoundSettings);

        levelselect = document.rootVisualElement.Q<Button>("LevelSelect");
        levelselect.RegisterCallback<ClickEvent>(LevelSelect);

        saveload = document.rootVisualElement.Q<Button>("SaveLoad");
        saveload.RegisterCallback<ClickEvent>(SaveLoad);

        mainmenu = document.rootVisualElement.Q<Button>("MainMenu");
        mainmenu.RegisterCallback<ClickEvent>(MainMenu);

        restart = document.rootVisualElement.Q<Button>("Restart");
        restart.RegisterCallback<ClickEvent>(Restart);

        nextlevel = document.rootVisualElement.Q<Button>("NextLevel");
        nextlevel.RegisterCallback<ClickEvent>(NextLevel);
    }

    void Update()
    {
        
    }
    
    public void DisplayEndCondition()
    {
        Startup();
        switch (gamemaster.gameMode)
        {
            case "tutorial":
            case "puzzle":
            case "vAI":
                if (gamemaster.player1win)
                    title.text = "You Win!";
                else
                    title.text = "You Lost!";
                break;

            case "PVP":
                if (gamemaster.player1win) title.text = "Player 1 Wins!";
                if (gamemaster.player2win) title.text = "Player 2 Wins!";

                break;
            default:
                title.text = "You Lost!";
                break;
        }
        if (!gamemaster.player1win) nextlevel.SetEnabled(false);
        if (datamaster.scenario.scenarioName == "tutorial7") nextlevel.text = "Finish tutorial";
        else nextlevel.text = "Next Level";
    }

    void Settings(ClickEvent evt)
    {
        master.OpenSettingsScene();
    }
    void Exit(ClickEvent evt)
    {
        Exit();
    }
    void SoundSettings(ClickEvent evt)
    {
        master.OpenSoundSettingsScene();
    }
    void LevelSelect(ClickEvent evt)
    {
        master.OpenLevelSelectScene();
    }
    void SaveLoad(ClickEvent evt)
    {
        master.OpenSaveLoadFilesScene();
    }
    void MainMenu(ClickEvent evt)
    {
        master.OpenMainMenuScene();
    }

    void Restart(ClickEvent evt)
    {
        gamemaster.Restart();
        Exit();
    }

    void NextLevel(ClickEvent evt)
    {
        gameObject.SetActive(false);
        master.OpenNextLevel();
    }
    
    void Exit()
    {
        gamemaster.RefocusGame();
        origin.SetActive(false);
    }
}
