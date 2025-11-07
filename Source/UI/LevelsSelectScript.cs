using UnityEngine;
using UnityEngine.UIElements;

public class LevelsSelectScript : MonoBehaviour
{
    public UIDocument document;

    Button back;
    public GameObject selectMain;
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        back = document.rootVisualElement.Q<Button>("Back");
        back.RegisterCallback<ClickEvent>(Back);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Restart()
    {
        back = document.rootVisualElement.Q<Button>("Back");
        back.RegisterCallback<ClickEvent>(Back);
    }
    
    void Back(ClickEvent evt)
    {
        gameObject.SetActive(false);
        selectMain.SetActive(true);
        selectMain.GetComponent<SelectScript>().Restart();

    }
}
