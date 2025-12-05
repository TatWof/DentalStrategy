using UnityEngine;
using UnityEngine.UIElements;

public class LevelsSelectScript : MonoBehaviour
{
    public UIDocument document;

    Button back;
    Button one, two, three, four, five, six, reset;
    public GameObject selectMain;
    public DataMasterScript datamaster;
    public MasterScript master;
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Restart()
    {
        datamaster = GameObject.Find("DataMaster").GetComponent<DataMasterScript>();
        master = GameObject.Find("Master").GetComponent<MasterScript>();
        
        back = document.rootVisualElement.Q<Button>("Back");
        back.RegisterCallback<ClickEvent>(Back);

        one = document.rootVisualElement.Q<Button>("One");
        one.RegisterCallback<ClickEvent>(One);

        two = document.rootVisualElement.Q<Button>("Two");
        if (datamaster.progress.scenarios < 1) two.SetEnabled(false);
        two.RegisterCallback<ClickEvent>(Two);

        three = document.rootVisualElement.Q<Button>("Three");
        if (datamaster.progress.scenarios < 2) three.SetEnabled(false);
        three.RegisterCallback<ClickEvent>(Three);

        four = document.rootVisualElement.Q<Button>("Four");
        if (datamaster.progress.scenarios < 3) four.SetEnabled(false);
        four.RegisterCallback<ClickEvent>(Four);

        five = document.rootVisualElement.Q<Button>("Five");
        if (datamaster.progress.scenarios < 4) five.SetEnabled(false);
        five.RegisterCallback<ClickEvent>(Five);

        six = document.rootVisualElement.Q<Button>("Six");
        if (datamaster.progress.scenarios < 5) six.SetEnabled(false);
        six.RegisterCallback<ClickEvent>(Six);

        reset = document.rootVisualElement.Q<Button>("Reset");
        reset.RegisterCallback<ClickEvent>(ResetProgress);
    }
    
    
    void One(ClickEvent evt)
    {
        LoadScenario("scenario1");
    }

    void Two(ClickEvent evt)
    {
        LoadScenario("scenario2");
    }

    void Three(ClickEvent evt)
    {
        LoadScenario("scenario3");
    }

    void Four(ClickEvent evt)
    {
        LoadScenario("scenario4");
    }

    void Five(ClickEvent evt)
    {
        LoadScenario("scenario5");
    }

    void Six(ClickEvent evt)
    {
        LoadScenario("scenario6");
    }
    
    void Back(ClickEvent evt)
    {
        gameObject.SetActive(false);
        selectMain.SetActive(true);
        selectMain.GetComponent<SelectScript>().Restart();
    }

    void LoadScenario(string name)
    {
        master.scenarioToLoad = name;
        Debug.Log(name);
        StartCoroutine(master.OpenScenario());
        //master.OpenScenario();
    }

    void ResetProgress(ClickEvent evt)
    {
        datamaster.progress.scenarios = 0;
        datamaster.ApplyProgress();
        Restart();
    }
}
