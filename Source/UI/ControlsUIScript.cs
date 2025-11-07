using UnityEngine;
using UnityEngine.UIElements;

public class ControlsUIScript : MonoBehaviour
{
    public UIDocument document;
    Button back;
    public MasterScript master;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Restart();
    }

    // Update is called once per frame
    void Restart()
    {
        master = GameObject.Find("Master").GetComponent<MasterScript>();
        back = document.rootVisualElement.Q<Button>("Back");
        back.RegisterCallback<ClickEvent>(Back);
    }

    void Back(ClickEvent evt)
    {
        Restart();
        StartCoroutine(master.LoadPreviousScene());
    }
}
