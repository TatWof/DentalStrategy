using UnityEngine;
using UnityEngine.UIElements;

public class TutorialsSelectScript : MonoBehaviour
{
    public UIDocument document;
    Button one, two, three, four, five, six, seven, back, reset;

    public GameObject selectMain;
    public DataMasterScript datamaster;
    public MasterScript master;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        datamaster = GameObject.Find("DataMaster").GetComponent<DataMasterScript>();
        master = GameObject.Find("Master").GetComponent<MasterScript>();
        Restart();
    }
    
    // Update is called once per frame
    void Update()
    {

    }

    public void Restart()
    {
        datamaster = GameObject.Find("DataMaster").GetComponent<DataMasterScript>();
        master = GameObject.Find("Master").GetComponent<MasterScript>();

        one = document.rootVisualElement.Q<Button>("1");
        one.RegisterCallback<ClickEvent>(One);

        two = document.rootVisualElement.Q<Button>("2");
        if (datamaster.progress.tutorials < 1) two.SetEnabled(false);
        two.RegisterCallback<ClickEvent>(Two);

        three = document.rootVisualElement.Q<Button>("3");
        if (datamaster.progress.tutorials < 2) three.SetEnabled(false);
        three.RegisterCallback<ClickEvent>(Three);

        four = document.rootVisualElement.Q<Button>("4");
        if (datamaster.progress.tutorials < 3) four.SetEnabled(false);
        four.RegisterCallback<ClickEvent>(Four);

        five = document.rootVisualElement.Q<Button>("5");
        if (datamaster.progress.tutorials < 4) five.SetEnabled(false);
        five.RegisterCallback<ClickEvent>(Five);

        six = document.rootVisualElement.Q<Button>("6");
        if (datamaster.progress.tutorials < 5) six.SetEnabled(false);
        six.RegisterCallback<ClickEvent>(Six);

        seven = document.rootVisualElement.Q<Button>("7");
        if (datamaster.progress.tutorials < 6) seven.SetEnabled(false);
        seven.RegisterCallback<ClickEvent>(Seven);
        
        back = document.rootVisualElement.Q<Button>("Back");
        back.RegisterCallback<ClickEvent>(Back);

        reset = document.rootVisualElement.Q<Button>("Reset");
        reset.RegisterCallback<ClickEvent>(ResetProgress);
    }

    void One(ClickEvent evt)
    {
        LoadTutorial("tutorial1");
    }

    void Two(ClickEvent evt)
    {
        LoadTutorial("tutorial2");
    }

    void Three(ClickEvent evt)
    {
        LoadTutorial("tutorial3");
    }

    void Four(ClickEvent evt)
    {
        LoadTutorial("tutorial4");
    }

    void Five(ClickEvent evt)
    {
        LoadTutorial("tutorial5");
    }

    void Six(ClickEvent evt)
    {
        LoadTutorial("tutorial6");
    }
    
    void Seven(ClickEvent evt)
    {
        LoadTutorial("tutorial7");
    }

    void Back(ClickEvent evt)
    {
        selectMain.SetActive(true);
        selectMain.GetComponent<SelectScript>().Restart();
        gameObject.SetActive(false);

    }

    void LoadTutorial(string name)
    {
        master.scenarioToLoad = name;
        Debug.Log(name);
        StartCoroutine(master.OpenScenario());
        //master.OpenScenario();
    }

    void ResetProgress(ClickEvent evt)
    {
        datamaster.progress.tutorials = 0;
        datamaster.ApplyProgress();
        Restart();
    }
}
