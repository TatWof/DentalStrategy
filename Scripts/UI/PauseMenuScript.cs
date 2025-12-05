using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject origin;
    public UIDocument document;

    Button settings, soundsettings, levelselect, saveload, mainmenu, restart, controls, exit;

    Slider cameraspeed;

    public DataMasterScript datamaster;
    public GameMasterScript gamemaster;
    public MasterScript master;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        datamaster = GameObject.Find("DataMaster").GetComponent<DataMasterScript>();
        gamemaster = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();
        master = GameObject.Find("Master").GetComponent<MasterScript>();
        Restart();

    }

    public void Restart()
    {
        datamaster = GameObject.Find("DataMaster").GetComponent<DataMasterScript>();
        gamemaster = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();
        master = GameObject.Find("Master").GetComponent<MasterScript>();


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

        controls = document.rootVisualElement.Q<Button>("Controls");
        controls.RegisterCallback<ClickEvent>(Controls);


        cameraspeed = document.rootVisualElement.Q<Slider>("CameraSpeed");
        cameraspeed.value = datamaster.settings.cameraSpeed;
        cameraspeed.lowValue = 0.01f;
        cameraspeed.highValue = 0.25f;
        cameraspeed.RegisterValueChangedCallback(CameraSpeed);
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

    void Controls(ClickEvent evt)
    {
        master.OpenControlsScene();
    }

    void Restart(ClickEvent evt)
    {
        gamemaster.Restart();
        Exit();
    }

    void CameraSpeed(ChangeEvent<float> evt)
    {
        datamaster.settings.cameraSpeed = cameraspeed.value;
        datamaster.ApplySettings();
    }
    
    void Exit()
    {
        gamemaster.RefocusGame();
        origin.SetActive(false);
    }
}
