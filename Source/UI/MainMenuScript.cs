using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuScript : MonoBehaviour
{
    
    public UIDocument document;

    Button settings, soundsettings, levelselect, saveload, controls, exit;
    
    public MasterScript master;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        
        
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

        controls = document.rootVisualElement.Q<Button>("Controls");
        controls.RegisterCallback<ClickEvent>(Controls);
    }

    // Update is called once per frame
    void Update()
    {
        if(master == null) master = GameObject.Find("Master").GetComponent<MasterScript>();
    }

    void Settings(ClickEvent evt)
    {
        master.OpenSettingsScene();
    }
    
    void Exit(ClickEvent evt)
    {
        master.ExitApp();
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
    void Controls(ClickEvent evt)
    {
        master.OpenControlsScene();
    }
}
