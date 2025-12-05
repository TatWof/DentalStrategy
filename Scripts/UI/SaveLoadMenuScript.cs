using UnityEngine;
using UnityEngine.UIElements;

public class SaveLoadMenuScript : MonoBehaviour
{
    public UIDocument document;

    public DataMasterScript datamaster;
    public MasterScript master;
    Label text;

    Button save1, save2, save3, load1, load2, load3, back;
    Button cleanSaves;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        datamaster = GameObject.Find("DataMaster").GetComponent<DataMasterScript>();
        master = GameObject.Find("Master").GetComponent<MasterScript>();

        save1 = document.rootVisualElement.Q<Button>("save1");
        save1.RegisterCallback<ClickEvent>(Save1);

        save2 = document.rootVisualElement.Q<Button>("save2");
        save2.RegisterCallback<ClickEvent>(Save2);

        save3 = document.rootVisualElement.Q<Button>("save3");
        save3.RegisterCallback<ClickEvent>(Save3);

        load1 = document.rootVisualElement.Q<Button>("load1");
        
        load1.RegisterCallback<ClickEvent>(Load1);

        load2 = document.rootVisualElement.Q<Button>("load2");
        
        load2.RegisterCallback<ClickEvent>(Load2);

        load3 = document.rootVisualElement.Q<Button>("load3");
        load3.RegisterCallback<ClickEvent>(Load3);

        text = document.rootVisualElement.Q<Label>("text");

        back = document.rootVisualElement.Q<Button>("Back");
        back.RegisterCallback<ClickEvent>(Back);

        cleanSaves = document.rootVisualElement.Q<Button>("CleanSaves");
        cleanSaves.RegisterCallback<ClickEvent>(Clean);

        UpdateEnable();
    }


    void Restart()
    {
        datamaster = GameObject.Find("DataMaster").GetComponent<DataMasterScript>();
        master = GameObject.Find("Master").GetComponent<MasterScript>();

        save1 = document.rootVisualElement.Q<Button>("save1");
        save1.RegisterCallback<ClickEvent>(Save1);

        save2 = document.rootVisualElement.Q<Button>("save2");
        save2.RegisterCallback<ClickEvent>(Save2);

        save3 = document.rootVisualElement.Q<Button>("save3");
        save3.RegisterCallback<ClickEvent>(Save3);

        load1 = document.rootVisualElement.Q<Button>("load1");
        
        load1.RegisterCallback<ClickEvent>(Load1);

        load2 = document.rootVisualElement.Q<Button>("load2");
        
        load2.RegisterCallback<ClickEvent>(Load2);

        load3 = document.rootVisualElement.Q<Button>("load3");
        load3.RegisterCallback<ClickEvent>(Load3);

        text = document.rootVisualElement.Q<Label>("text");

        back = document.rootVisualElement.Q<Button>("Back");
        back.RegisterCallback<ClickEvent>(Back);

        cleanSaves = document.rootVisualElement.Q<Button>("CleanSaves");
        cleanSaves.RegisterCallback<ClickEvent>(Clean);

        UpdateEnable();
    }
    void UpdateEnable()
    {
        if (datamaster.gameLoadedOnce)
        {
            save1.SetEnabled(true);
            save2.SetEnabled(true);
            save3.SetEnabled(true);
        }
        else
        {   
            save2.SetEnabled(false);
            save1.SetEnabled(false);
            save3.SetEnabled(false);
        }

        if (datamaster.SaveFileExists("save1"))
        {
            Debug.Log("save1 exists");
            load1.SetEnabled(true);
        }
        else load1.SetEnabled(false);

        if (datamaster.SaveFileExists("save2"))
        {
            Debug.Log("save2 exists");
            load2.SetEnabled(true);
        }
        else load2.SetEnabled(false);

        if (datamaster.SaveFileExists("save3"))
        {
            Debug.Log("save3 exists");
            load3.SetEnabled(true);
        }
        else load3.SetEnabled(false);
    }

    void Clean (ClickEvent evt)
    {
        datamaster.CleanSaves();
        Restart();
    }

    void Save1(ClickEvent evt)
    {
        Save("save1");
        Restart();
    }

    void Save2(ClickEvent evt)
    {
        Save("save2");
        Restart();
    }
    
    void Save3(ClickEvent evt)
    {
        Save("save3");
        Restart();
    }

    void Load1(ClickEvent evt)
    {
        Load("save1");
    }

    void Load2(ClickEvent evt)
    {
        Load("save2");
    }
    
    void Load3(ClickEvent evt)
    {
        Load("save3");
    }

    void Back(ClickEvent evt)
    {
        Restart();
        StartCoroutine(master.LoadPreviousScene());
    }
    
    void Save(string name)
    {
        datamaster.SaveGame(name);
        text.text = "Saved to " + name;
    }
    
    void Load(string name)
    {
        master.saveToLoad = name;
        datamaster.GrabSave(name);
        StartCoroutine(master.OpenSaveGame());
    }
}
