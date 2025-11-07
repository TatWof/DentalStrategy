using UnityEngine;

public class Singleton : MonoBehaviour
{
    GameObject Master;
    public GameObject mr;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Master = GameObject.Find("Master");
        

        if (Master == null)
        {
            GameObject Master = Instantiate(mr);
            Master.name = "Master";
        }
    }

}
