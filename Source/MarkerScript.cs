using System.Collections;
using UnityEngine;


public class MarkerScript : MonoBehaviour
{
    public enum MarkerType { MOVE, LEAP, ATTACK, CRUSH, PUSH, PROTECT}
    public GameMasterScript gms;
    public GameObject boardmaster;
    public BoardMasterScript bms;
    public GameObject origin;
    public GameObject reference;
    public Animator a;
    public MarkerType markerType = MarkerType.MOVE;
    public int posX, posY;
    public bool ghost = false;
    public Vector2Int pushDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boardmaster = GameObject.FindGameObjectWithTag("BoardMaster");
        bms = boardmaster.GetComponent<BoardMasterScript>();
        gms = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMasterScript>();
        reference = bms.FindUnitAtGrid(posX, posY);

        a.Play("New Animation");
    }

    // Update is called once per frame
    void Update()
    {
        bms = boardmaster.GetComponent<BoardMasterScript>();

        if (gms.gameActive)
        {
            Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetKeyDown(KeyCode.Mouse0)
                && markerType != MarkerType.PROTECT
                && mousepos.x >= posX && mousepos.x < posX + 1
                && mousepos.y >= posY && mousepos.y < posY + 1)
            {
                StartCoroutine(DoMarker());
            }
        }
    }

    public void NormalizePos()
    {
        transform.position = new Vector2(posX + 0.5f, posY + 0.5f);
    }

    public void SetNewPos(int x, int y)
    {
        posX = x;
        posY = y;
        NormalizePos();
    }

    public void ChangeMarkerType(MarkerType type)
    {
        if (ghost)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }

        markerType = type;
        switch (markerType)
        {
            case MarkerType.MOVE: gameObject.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, 1f); break;
            case MarkerType.ATTACK: gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1f); break;
            case MarkerType.PUSH:
                gameObject.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 1.0f, 1f);
                reference.GetComponent<UnitScript>().selectLock = true;
                break;
            case MarkerType.PROTECT: gameObject.GetComponent<SpriteRenderer>().color = new Color(0.0f, 1.0f, 0.0f, 1f); break;
            case MarkerType.CRUSH: gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.5f, 0.5f, 1f); break;
        }
    }
    
    public void Do()
    {
        gms = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMasterScript>();
        StartCoroutine(DoMarker());
    }

    IEnumerator DoMarker()
    {
        yield return new WaitForEndOfFrame();
        switch (markerType)
        {
            case MarkerType.MOVE:
                origin.GetComponent<UnitScript>().MoveTo(posX, posY);            
                break;
            case MarkerType.ATTACK:
                reference.GetComponent<UnitScript>().Kill();
                bms.DespawnUnit(reference);
                origin.GetComponent<UnitScript>().MoveTo(posX, posY);
                break;

            case MarkerType.PUSH:
                UnitScript rus = reference.GetComponent<UnitScript>();
                int rx = rus.posX + pushDirection.x, ry = rus.posY + pushDirection.y;
                rus.MoveTo(rx, ry);
                origin.GetComponent<UnitScript>().MoveTo(posX, posY);
                break;
            case MarkerType.CRUSH:
                bms.CrushRock(posX, posY);
                origin.GetComponent<UnitScript>().MoveTo(posX, posY);
                break;
        }
        //Debug.Log("end of do marker");
        GameObject[] collection = GameObject.FindGameObjectsWithTag("move");
        foreach (var item in collection)
        {
            item.GetComponent<SpriteRenderer>().enabled = false;
        }
        origin.GetComponent<UnitScript>().selected = false;

        yield return new WaitForSecondsRealtime(0.1f);

        gms = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMasterScript>();
        bms.DestroyMarkers();
        gms.EndTurn();
    }

    public void OnDestroy()
    {
        GameObject[] collection = GameObject.FindGameObjectsWithTag("Unit");

        foreach (var item in collection)
        {
            item.GetComponent<UnitScript>().selectLock = false;
        }
    }
}
