using UnityEngine;
using UnityEngine.UI;

public class GameUIScript : MonoBehaviour
{
    public GameMasterScript gms;
    public Text textthing;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        textthing.text = "Goals\n";
        textthing.text += "Eliminate all enemies\n";
        if (gms.turnLimit > 0) textthing.text += "Turns: " + gms.turn + "/" + gms.turnLimit + "\n";
        if (gms.extractionLimit > 0) textthing.text += "Extracted: " + gms.extracted + "/" + gms.extractionLimit + "\n";
        if (gms.unitLimit > 0) textthing.text += "No Less than " + gms.unitLimit + " Units: " + gms.unitcount + "\n";
    }
}
