using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EditorScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            List<GameObject> list = GameObject.FindGameObjectsWithTag("Unit").ToList();

            foreach (var item in list)
            {
                item.GetComponent<UnitScript>().Setup();
            }
        }
    }
}
