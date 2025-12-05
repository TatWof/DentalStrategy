using System.Collections;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    private static readonly WaitForSecondsRealtime _waitForSecondsRealtime0_5 = new(0.5f);
    public GameObject gamemaster;
    GameMasterScript gms;
    public GameObject boardmaster;
    BoardMasterScript bms;

    public Animator anim;

    public int posX, posY;

    public Sprite dummy, baby, incis, broadincis, canine, pawn, premolar, scaredpremolar, molar, bigmolar, king;

    public GameMasterScript.Team team;
    public bool selected;
    public bool selectLock;
    public bool ghost = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        boardmaster = GameObject.FindGameObjectWithTag("BoardMaster");
        bms = boardmaster.GetComponent<BoardMasterScript>();
        gamemaster = GameObject.Find("GameMaster");
        gms = gamemaster.GetComponent<GameMasterScript>();

    }

    void SetSprite()
    {
        if (ghost)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        
        switch (name)
        {
            case "baby": GetComponentInChildren<SpriteRenderer>().sprite = baby; break;
            case "incis": GetComponentInChildren<SpriteRenderer>().sprite = incis; break;
            case "broadincis": GetComponentInChildren<SpriteRenderer>().sprite = broadincis; break;
            case "canine": GetComponentInChildren<SpriteRenderer>().sprite = canine; break;
            case "pawn": GetComponentInChildren<SpriteRenderer>().sprite = pawn; break;
            case "premolar": GetComponentInChildren<SpriteRenderer>().sprite = premolar; break;
            case "scaredpremolar": GetComponentInChildren<SpriteRenderer>().sprite = scaredpremolar; break;
            case "molar": GetComponentInChildren<SpriteRenderer>().sprite = molar; break;
            case "bigmolar": GetComponentInChildren<SpriteRenderer>().sprite = bigmolar; break;
            case "king": GetComponentInChildren<SpriteRenderer>().sprite = king; break;
            
            case "dummy":  
            default: GetComponentInChildren<SpriteRenderer>().sprite = dummy; break;
        }

        switch (team)
        {
            case GameMasterScript.Team.PLAYER1:
                gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1f); break;
            case GameMasterScript.Team.PLAYER2:
                gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(0.0f, 1.0f, 0.0f, 1f); break;
            case GameMasterScript.Team.ENEMY1:
                gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(0.9f, 0.1f, 0.1f, 1f); break;
            case GameMasterScript.Team.ENEMY2:
                gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(0.9f, 0.5f, 0.5f, 1f); break;

        }
    }

    void SetNameAndTeam(string name, GameMasterScript.Team team)
    {
        this.name = name;
        this.team = team;
        if (!ghost) SetSprite();
    }

    public void Setup()
    {
        Setup(name, team, posX, posY, ghost);
    }

    public void Setup(string name, GameMasterScript.Team team, int x, int y, bool ghost)
    {
        SetNewPos(x, y);
        SetNameAndTeam(name, team);
        this.ghost = ghost;
    }

    // Update is called once per frame
    void Update()
    {

        if (gms.gameActive && !selectLock && gms.activeTeam == GameMasterScript.Team.PLAYER1)
        {
            Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (!selected && Input.GetKeyDown(KeyCode.Mouse0)
                && team == gms.activeTeam
                && mousepos.x >= posX && mousepos.x < posX + 1
                && mousepos.y >= posY && mousepos.y < posY + 1)
            {
                anim.Play("selected");
                GameObject[] list = GameObject.FindGameObjectsWithTag("Unit");
                foreach (var item in list)
                    item.GetComponent<UnitScript>().selected = false;
                bms.DestroyMarkers();
                CreateMarkers();
                selected = true;
            }
            if (selected && Input.GetKeyDown(KeyCode.Mouse1) 
                && team == gms.activeTeam
                && mousepos.x >= posX && mousepos.x < posX + 1
                && mousepos.y >= posY && mousepos.y < posY + 1)
            {
                bms.DestroyMarkers();
                selected = false;
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

    void CreateMarkers()
    {
        switch (name)
        {
            case "dummy": break;
            case "baby": CreateBabyMarkers(); break;
            case "incis": CreateIncisMarkers(); break;
            case "broadincis": CreateBroadIncisMarkers(); break;
            case "canine": CreateCanineMarkers(); break;
            case "pawn": CreatePawnMarkers(); break;
            case "premolar": CreatePremolarMarkers(); break;
            case "scaredpremolar": CreateScaredMarkers(); break;
            case "molar": CreateMolarMarkers(); break;
            case "bigmolar": CreateBigMolarMarkers(); break;
            case "king": CreateKingMarkers(); break;
        }
    }
    
    public void CreateGhostMarkers()
    {
        bool temp = ghost;
        ghost = true;
        CreateMarkers();
        ghost = temp;
    }

    private void CreateBabyMarkers()
    {
        BoardMasterScript.MarkerBools mb = new(false, true, false, false, false, false, ghost);

        OrthoMarkerValidate(mb, 1, 0, false);
    }

    private void CreateMolarMarkers()
    {
        BoardMasterScript.MarkerBools mb = new(true, true, false, true, false, false, ghost);

        OmniDistMarkerValidate(mb, 2, 0, false);
    }

    private void CreateScaredMarkers()
    {
        BoardMasterScript.MarkerBools mb = new(true, true, false, false, true, true, ghost);

        OrthoMarkerValidate(mb, 8, 1, false);
    }

    private void CreatePremolarMarkers()
    {
        BoardMasterScript.MarkerBools mb = new(true, true, false, false, true, false, ghost);

        OrthoMarkerValidate(mb, 8, 1, false);

        mb.canattackafterleap = false;
        DiagMarkerValidate(mb, 3, 0, false);
    }

    private void CreatePawnMarkers()
    {
        BoardMasterScript.MarkerBools mb = new(true, false, false, true, false, false, ghost);

        OrthoMarkerValidate(mb, 1, 0, false);

        mb.canmove = false;
        mb.canattack = true;

        DiagMarkerValidate(mb, 1, 0, false);
    }

    private void CreateBigMolarMarkers()
    {
        BoardMasterScript.MarkerBools mb = new(true, true, true, true, false, false, ghost);

        bool n = true, ne = true, e = true, se = true, s = true, sw = true, w = true, nw = true;
        n = DistMarkerValidate(0, 1, 2, mb, 0, false);
        s = DistMarkerValidate(0, -1, 2, mb, 0, false);
        w = DistMarkerValidate(-1, 0, 2, mb, 0, false);
        e = DistMarkerValidate(1, 0, 2, mb, 0, false);

        ne = DistMarkerValidate(1, 1, 2, mb, 0, false);
        se = DistMarkerValidate(-1, 1, 2, mb, 0, false);
        nw = DistMarkerValidate(1, -1, 2, mb, 0, false);
        sw = DistMarkerValidate(-1, -1, 2, mb, 0, false);

        mb.canpush = false;

        if (!(n && ne)) RelativeMarkerValidate(1, 2, mb, false, false);
        if (!(n && nw)) RelativeMarkerValidate(-1, 2, mb, false, false);
        if (!(w && nw)) RelativeMarkerValidate(-2, 1, mb, false, false);
        if (!(w && sw)) RelativeMarkerValidate(-2, -1, mb, false, false);
        if (!(s && se)) RelativeMarkerValidate(1, -2, mb, false, false);
        if (!(s && sw)) RelativeMarkerValidate(-1, -2, mb, false, false);
        if (!(e && ne)) RelativeMarkerValidate(2, 1, mb, false, false);
        if (!(e && se)) RelativeMarkerValidate(2, -1, mb, false, false);


    }

    private void CreateCanineMarkers()
    {
        BoardMasterScript.MarkerBools mb = new(true, true, false, false, false, true, ghost);

        OrthoMarkerValidate(mb, 3, 1, false);
        
    }

    private void CreateBroadIncisMarkers()
    {
        BoardMasterScript.MarkerBools mb = new(true, true, false, false, false, false, ghost);

        OmniDistMarkerValidate(mb, 2, 0, false);
    }

    private void CreateIncisMarkers()
    {
        BoardMasterScript.MarkerBools mb = new(true, true, false, false, false, false, ghost);

        OrthoMarkerValidate(mb, 1, 0, false);

        mb.canmove = false;
        OrthoMarkerValidate(mb, 2, 0, false);
    }
    
    private void CreateKingMarkers()
    {
        BoardMasterScript.MarkerBools mb = new(true, false, false, true, false, false, ghost);

        OrthoMarkerValidate(mb, 1, 0, false);
    }

    public void MoveTo(int x, int y)
    {
        StartCoroutine(MoveTo(x, y, 100));
    }

    IEnumerator MoveTo(int x, int y, int steps)
    {
        for (int i = 0; i < steps; ++i)
        {
            transform.position = new Vector2(posX + (x - posX) / (float)steps * i + 0.5f,
                posY + (y - posY) / (float)steps * i + 0.5f);
            yield return new WaitForSecondsRealtime(0.05f / steps);
        }
        SetNewPos(x, y);
        if (bms.GetTileSprite(x, y).name == "Square")
        {
            Extract();
        }
    }

    public void Kill()
    {
        SoundMasterScript soundmaster = GameObject.Find("SoundMaster").GetComponent<SoundMasterScript>();

        bms.DestroyMarkers();
        if(team == GameMasterScript.Team.PLAYER1) --gms.unitcount;
        soundmaster.Play("death");
        anim.Play("die");
        bms.DespawnUnit(gameObject);
    }

    int RelativeMarkerValidate(int x, int y, BoardMasterScript.MarkerBools mb, bool hasleapt, bool overwrite)
    {
        //Debug.Log("In relative move valid");
        int type = bms.MarkerValidate(gameObject, posX + x, posY + y, mb, hasleapt, overwrite);
        //Debug.Log(type);
        return type;
    }

    void OrthoMarkerValidate(BoardMasterScript.MarkerBools mb, int distance, int leaptimes, bool overwrite)
    {
        DistMarkerValidate(1, 0, distance, mb, leaptimes, overwrite);
        DistMarkerValidate(-1, 0, distance, mb, leaptimes, overwrite);
        DistMarkerValidate(0, 1, distance, mb, leaptimes, overwrite);
        DistMarkerValidate(0, -1, distance, mb, leaptimes, overwrite);
    }

    void HoriMarkerValidate(BoardMasterScript.MarkerBools mb, int distance, int leaptimes, bool overwrite)
    {
        DistMarkerValidate(1, 0, distance, mb, leaptimes, overwrite);
        DistMarkerValidate(-1, 0, distance, mb, leaptimes, overwrite);
    }
    void VertMarkerValidate(BoardMasterScript.MarkerBools mb, int distance, int leaptimes, bool overwrite)
    {
        DistMarkerValidate(0, 1, distance, mb, leaptimes, overwrite);
        DistMarkerValidate(0, -1, distance, mb, leaptimes, overwrite);
    }

    void SlopeUpMarkerValidate(BoardMasterScript.MarkerBools mb, int distance, int leaptimes, bool overwrite)
    {
        DistMarkerValidate(1, 1, distance, mb, leaptimes, overwrite);
        DistMarkerValidate(-1, -1, distance, mb, leaptimes, overwrite);
    }

    void SlopeDownMarkerValidate(BoardMasterScript.MarkerBools mb, int distance, int leaptimes, bool overwrite)
    {
        DistMarkerValidate(1, -1, distance, mb, leaptimes, overwrite);
        DistMarkerValidate(-1, 1, distance, mb, leaptimes, overwrite);
    }

    void DiagMarkerValidate(BoardMasterScript.MarkerBools mb, int distance, int leaptimes, bool overwrite)
    {
        DistMarkerValidate(1, 1, distance, mb, leaptimes, overwrite);
        DistMarkerValidate(1, -1, distance, mb, leaptimes, overwrite);
        DistMarkerValidate(-1, 1, distance, mb, leaptimes, overwrite);
        DistMarkerValidate(-1, -1, distance, mb, leaptimes, overwrite);
    }

    bool DistMarkerValidate(int x, int y, int distance, BoardMasterScript.MarkerBools mb, int leaptimes, bool overwrite)
    {
        int leapt = 0;
        int rx = x, ry = y;
        bool blocked = false;
        for (int i = 0; i < distance; ++i)
        {
            int blocktype = RelativeMarkerValidate(rx, ry, mb, leapt > 0, overwrite);
            if (blocktype > 0) blocked = true;
            if (blocktype == 2 || (blocktype == 1 && leapt >= leaptimes)) break;
            if (blocktype == 1 && leapt < leaptimes) ++leapt;
            rx += x;
            ry += y;
        }
        return blocked;
    }

    void OmniDistMarkerValidate(BoardMasterScript.MarkerBools mb, int distance, int leaptimes, bool overwrite)
    {
        OrthoMarkerValidate(mb, distance, leaptimes, overwrite);
        DiagMarkerValidate(mb, distance, leaptimes, overwrite);
    }

    public void Extract()
    {
        bms.DestroyMarkers();
        if (team == GameMasterScript.Team.PLAYER1)
        {
            --gms.unitcount;
            ++gms.extracted;
        }
        anim.Play("extract");
        bms.DespawnUnit(gameObject);
    }

    void OnDestroy()
    {
        Debug.Log("destroyed: " + gameObject.name + " " + posX + " " + posY);
    }
}
