using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AgentScript : MonoBehaviour
{
    public BoardMasterScript boardmaster;
    public GameMasterScript gamemaster;

    bool activated;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boardmaster = GameObject.Find("BoardMaster").GetComponent<BoardMasterScript>();
        gamemaster = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate(int depth)
    {
        if (activated) return;
        activated = true;
        Debug.Log("Agent Active: " + activated);
        EvalAllMoves(depth);
    }

    void EvalAllMoves(int depth)
    {
        if (depth <= 0)
        {
            activated = false;
            gamemaster.EndTurn();
            return;
        }
        StartCoroutine(EvalMoves(depth));
    }
    
    IEnumerator EvalMoves(int depth)
    {
        // gets all the units that match the current team turn
        GameObject[] unitlist = GameObject.FindGameObjectsWithTag("Unit");
        foreach (var item in unitlist)
        {
            if (item.GetComponent<UnitScript>().team == gamemaster.activeTeam)
                item.GetComponent<UnitScript>().CreateGhostMarkers();
        }

        // get all depth 1 markers
        GameObject[] markerlist = GameObject.FindGameObjectsWithTag("move");

        if (unitlist.Count() != 0 && markerlist.Count() != 0)
        {
            int[] markerEvals = new int[markerlist.Length];
            for (int i = 0; i < markerlist.Length; ++i)
            {
                // evaluate the value of each of the moves over a certain depth
                markerEvals[i] = EvalMove(markerlist[i], depth - 1);
            }

            int max = 0;
            for (int i = 0; i < markerEvals.Length; i++)
            {
                if (markerEvals[i] > max) max = markerEvals[i];
            }
            //Debug.Log("value: " + max);

            List<GameObject> list = new();
            for (int i = 0; i < markerEvals.Length; ++i)
            {
                if (markerEvals[i] == max) list.Add(markerlist[i]);
            }
            GameObject chosen = list[Random.Range(0, list.Count)];
            // Debug.Log(chosen.name);

            MarkerScript ms = chosen.GetComponent<MarkerScript>();
            Debug.Log(ms.posX + " " + ms.posY);
            //Debug.Log(ms.reference.name);
            ms.Do();
        }
        else
        {
            gamemaster.EndTurn();
        }
        activated = false;
        yield return new WaitForSeconds(4f);
        // gamemaster.EndTurn();
    }

    int EvalMove(GameObject move, int depth)
    {
        if (depth <= 0) return EvalMove(move);

        List<GameObject> markerlist = new();
        GameObject origin = move.GetComponent<MarkerScript>().origin;
        GameObject ghost = boardmaster.SpawnGhost(origin.name, origin.GetComponent<UnitScript>().team,
            move.GetComponent<MarkerScript>().posX, move.GetComponent<MarkerScript>().posY);
        //Debug.Log("Created a Ghost at: " + ghost.GetComponent<UnitScript>().posX + " " + ghost.GetComponent<UnitScript>().posY);

        ghost.GetComponent<UnitScript>().CreateGhostMarkers();
        if (ghost.transform.GetChild(1).childCount == 0)
        {
            Destroy(ghost);
            return 0;
        }

        int[] markerEvals = new int[ghost.transform.GetChild(1).childCount];

        for (int i = 0; i < ghost.transform.GetChild(1).childCount; ++i)
        {
            markerlist.Add(ghost.transform.GetChild(1).GetChild(i).gameObject);
        }

        for (int i = 0; i < markerlist.Count; ++i)
        {
            switch (markerlist[i].GetComponent<MarkerScript>().markerType)
            {
                case MarkerScript.MarkerType.ATTACK:
                    markerEvals[i] += 2 + EvalMove(markerlist[i], depth - 1); break;
                case MarkerScript.MarkerType.PROTECT:
                    markerEvals[i] += 1 + EvalMove(markerlist[i], depth - 1); break;
                case MarkerScript.MarkerType.MOVE:
                    markerEvals[i] += 0 + EvalMove(markerlist[i], depth - 1); break;
                case MarkerScript.MarkerType.CRUSH:
                    markerEvals[i] += 0 + EvalMove(markerlist[i], depth - 1); break;                
                default: break;
            }

        }

        int sum = 0;
        foreach (var item in markerEvals)
        {
            sum += item;
        }
        Destroy(ghost);
        return sum;
    }
    
    int EvalMove(GameObject move)
    {
        switch (move.GetComponent<MarkerScript>().markerType)
        {
            case MarkerScript.MarkerType.ATTACK: return 2;
            case MarkerScript.MarkerType.PROTECT: return 1;
            case MarkerScript.MarkerType.MOVE: return 0;
            default: break;
        }
        return 0;
    }
}
