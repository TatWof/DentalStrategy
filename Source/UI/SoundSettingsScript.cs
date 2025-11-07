using UnityEngine;
using UnityEngine.UIElements;

public class SoundSettingsScript : MonoBehaviour
{
    public UIDocument document;
    public DataMasterScript datamaster;
    public MasterScript master;

    public Button back;

    public Slider masterV, effectsV, musicV;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        datamaster = GameObject.Find("DataMaster").GetComponent<DataMasterScript>();
        master = GameObject.Find("Master").GetComponent<MasterScript>();
        
        back = document.rootVisualElement.Q<Button>("Back");
        back.RegisterCallback<ClickEvent>(Back);

        masterV = document.rootVisualElement.Q<Slider>("Master");
        masterV.value = datamaster.settings.soundsettings.master;
        masterV.lowValue = 0.0001f;
        masterV.highValue = 1f;
        masterV.RegisterValueChangedCallback(Master);

        effectsV = document.rootVisualElement.Q<Slider>("Effects");
        effectsV.value = datamaster.settings.soundsettings.effects;
        effectsV.lowValue = 0.0001f;
        effectsV.highValue = 1f;
        effectsV.RegisterValueChangedCallback(Effects);

        musicV = document.rootVisualElement.Q<Slider>("Music");
        musicV.value = datamaster.settings.soundsettings.bgm;
        musicV.lowValue = 0.0001f;
        musicV.highValue = 1f;
        musicV.RegisterValueChangedCallback(Music);
    }

    void Master(ChangeEvent<float> evt)
    {
        datamaster.settings.soundsettings.master = masterV.value;
        datamaster.ApplySettings();
    }

    void Music(ChangeEvent<float> evt)
    {
        datamaster.settings.soundsettings.bgm = musicV.value;
        datamaster.ApplySettings();
    }

    void Effects(ChangeEvent<float> evt)
    {
        datamaster.settings.soundsettings.effects = effectsV.value;
        datamaster.ApplySettings();
    } 
    
    void Back(ClickEvent evt)
    {
        StartCoroutine(master.LoadPreviousScene());
    }
}
