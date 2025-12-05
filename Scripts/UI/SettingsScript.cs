using UnityEngine;
using UnityEngine.UIElements;

public class SettingsScript : MonoBehaviour
{
    public UIDocument document;

    Button soundsettings, back;

    Slider cameraspeed;

    public DataMasterScript datamaster;
    public MasterScript master;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        datamaster = GameObject.Find("DataMaster").GetComponent<DataMasterScript>();
        master = GameObject.Find("Master").GetComponent<MasterScript>();
        
        back = document.rootVisualElement.Q<Button>("Back");
        back.RegisterCallback<ClickEvent>(Back);

        soundsettings = document.rootVisualElement.Q<Button>("SoundSettings");
        soundsettings.RegisterCallback<ClickEvent>(SoundSettings);

        cameraspeed = document.rootVisualElement.Q<Slider>("CameraSpeed");
        cameraspeed.value = datamaster.settings.cameraSpeed;
        cameraspeed.lowValue = 0.01f;
        cameraspeed.highValue = 0.25f;
        cameraspeed.RegisterValueChangedCallback(CameraSpeed);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SoundSettings(ClickEvent evt)
    {
        master.OpenSoundSettingsScene();
    }

    void Back(ClickEvent evt)
    {
        StartCoroutine(master.LoadPreviousScene());
    }
    
    void CameraSpeed(ChangeEvent<float> evt)
    {
        datamaster.settings.cameraSpeed = cameraspeed.value;
        datamaster.ApplySettings();
    }
}
