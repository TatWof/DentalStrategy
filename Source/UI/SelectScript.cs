using UnityEngine;
using UnityEngine.UIElements;

public class SelectScript : MonoBehaviour
{
    public UIDocument document;

    Button tutorials, levels, back;

    public GameObject tutorial, level;

    public MasterScript master;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        master = GameObject.Find("Master").GetComponent<MasterScript>();
        
        back = document.rootVisualElement.Q<Button>("Back");
        back.RegisterCallback<ClickEvent>(Back);

        tutorials = document.rootVisualElement.Q<Button>("Tutorials");
        tutorials.RegisterCallback<ClickEvent>(Tutorials);

        levels = document.rootVisualElement.Q<Button>("Levels");
        levels.RegisterCallback<ClickEvent>(Levels);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Restart()
    {
        master = GameObject.Find("Master").GetComponent<MasterScript>();
        
        back = document.rootVisualElement.Q<Button>("Back");
        back.RegisterCallback<ClickEvent>(Back);

        tutorials = document.rootVisualElement.Q<Button>("Tutorials");
        tutorials.RegisterCallback<ClickEvent>(Tutorials);

        levels = document.rootVisualElement.Q<Button>("Levels");
        levels.RegisterCallback<ClickEvent>(Levels);
    }

    void Levels(ClickEvent evt)
    {
        gameObject.SetActive(false);
        level.SetActive(true);
        level.GetComponent<LevelsSelectScript>().Restart();
    }

    void Tutorials(ClickEvent evt)
    {
        gameObject.SetActive(false);
        tutorial.SetActive(true);
        tutorial.GetComponent<TutorialsSelectScript>().Restart();
    }

    void Back(ClickEvent evt)
    {
        StartCoroutine(master.LoadPreviousScene());
    }
    

}
