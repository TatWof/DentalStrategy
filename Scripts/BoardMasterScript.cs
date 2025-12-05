using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardMasterScript : MonoBehaviour
{

    public struct MarkerBools
    {
        public MarkerBools(bool move, bool attack, bool crush, bool push, bool leapmove, bool leapattack, bool isghost)
        {
            canmove = move;
            canattack = attack;
            cancrush = crush;
            canpush = push;
            canmoveafterleap = leapmove;
            canattackafterleap = leapattack;
            ghost = isghost;
        }
        public bool canmove;
        public bool canattack;
        public bool cancrush;
        public bool canpush;
        public bool canmoveafterleap;
        public bool canattackafterleap;
        public bool ghost;
    }

    public GameObject gamemaster;
    public GameObject unit;
    public GameObject Units;
    public GameObject marker;

    public Tilemap tilemap;
    public TileBase grass1, grass2, grass3, grass4, grassball, extract, voidtile;
    public int maxN, maxS, maxW, maxE;

    public GameObject SpawnGhost(string name, GameMasterScript.Team team, int x, int y)
    {
        return SpawnUnit(name, team, x, y, true);
    }

    GameObject SpawnUnit(string name, GameMasterScript.Team team, int x, int y, bool ghost)
    {
        GameObject reference = FindUnitAtGrid(x, y);
        TileBase tile = GetTile(x, y);

        if (reference != null && !ghost) DespawnUnit(reference);
        if (tile == voidtile || tile == grassball) return null;

        GameObject obj = Instantiate(unit, Units.transform);
        UnitScript us = obj.GetComponent<UnitScript>();
        
        us.Setup(name, team, x, y, ghost);
        return obj;
    }
    public void DespawnUnit(GameObject obj)
    {
        Destroy(obj);
    }

    public TileBase GetTile(int x, int y)
    {
        return tilemap.GetTile(tilemap.WorldToCell(new Vector3(x + 0.5f, y + 0.5f, 0)));
    }


    public Sprite GetTileSprite(int x, int y)
    {
        return tilemap.GetSprite(tilemap.WorldToCell(new Vector3(x + 0.5f, y + 0.5f, 0)));
    }

    public GameObject FindUnitAtGrid(int x, int y)
    {
        GameObject[] list = GameObject.FindGameObjectsWithTag("Unit");
        //Debug.Log(x + " " + y);

        foreach (var item in list)
        {
            if (item == null) break;
            UnitScript us = item.GetComponent<UnitScript>();
            if (us.posX == x && us.posY == y && !us.ghost)
            {
                //Debug.Log(us.posX + " " + us.posY);
                return item;
            }
        }
        return null;
    }

    public int MarkerValidate(GameObject origin, int x, int y, MarkerBools mb, bool hasleapt, bool overwrite)
    {
        return MarkerValidate(origin, x, y, mb.canmove, mb.canattack, mb.cancrush, mb.canpush,
            mb.canmoveafterleap, mb.canattackafterleap, hasleapt,
            overwrite, mb.ghost);
    }

    
    private int MarkerValidate(GameObject origin, int x, int y,
        bool canmove, bool canattack, bool cancrush, bool canpush,
        bool canmoveafterleap, bool canattackafterleap, bool hasleapt,
        bool overwrite, bool ghost)
    {
        int blocked = 0;
        bool rockhit = false;
        bool enemyhit = false;
        bool teamhit = false;
        TileBase tile = GetTile(x, y);

        if (overwrite)
        {
            GameObject reference = FindMarker(origin, x, y);
            if (reference != null) Destroy(reference);
        }
        else if (MarkerAlreadyExists(origin, x, y)) return 3;

        if (tile == voidtile || tile == null || y > maxN || y < maxS || x > maxE || x < maxW) return 2;

        if (tile == grassball) rockhit = true;
        if (FindUnitAtGrid(x, y) != null)
        {
            if (FindUnitAtGrid(x, y).GetComponent<UnitScript>().team != origin.GetComponent<UnitScript>().team) enemyhit = true;
            else teamhit = true;
        }
        if (rockhit || teamhit || enemyhit) blocked = 1;

        if (blocked == 0 && canmove)
        {
            if (hasleapt && canmoveafterleap) CreateMarkerWith(origin, x, y, MarkerScript.MarkerType.LEAP, ghost);
            else CreateMarkerWith(origin, x, y, MarkerScript.MarkerType.MOVE, ghost);
        }
        if (blocked == 1)
        {
            if (enemyhit && (canattack || (hasleapt && canattackafterleap)))
                CreateMarkerWith(origin, x, y, MarkerScript.MarkerType.ATTACK, ghost);
            else if (rockhit && cancrush && !hasleapt)
                CreateMarkerWith(origin, x, y, MarkerScript.MarkerType.CRUSH, ghost);
            else if (teamhit && canpush && !hasleapt)
            {
                int ox = origin.GetComponent<UnitScript>().posX, oy = origin.GetComponent<UnitScript>().posY;
                int pdx = 0, pdy = 0;
                if (ox > x) pdx = -1;
                if (ox < x) pdx = 1;
                if (oy > y) pdy = -1;
                if (oy < y) pdy = 1;
                TileBase rtile = GetTile(x + pdx, y + pdy);

                if(rtile != voidtile && rtile != null)
                {
                    MarkerScript ms = CreateMarkerWith(origin, x, y, MarkerScript.MarkerType.PUSH, ghost);
                    ms.pushDirection = new Vector2Int(pdx, pdy);
                }
                else blocked = 2;

            }
            else if (teamhit && (canattack || (hasleapt && canattackafterleap)))
                CreateMarkerWith(origin, x, y, MarkerScript.MarkerType.PROTECT, ghost);
        }

        return blocked;
    }

    private bool MarkerAlreadyExists(GameObject origin, int x, int y)
    {
        if (FindMarker(origin, x, y) == null) return false;
        return true;
    }

    private GameObject FindMarker(GameObject origin, int x, int y)
    {
        GameObject[] list = GameObject.FindGameObjectsWithTag("move");

        foreach (var item in list)
        {
            MarkerScript ms = item.GetComponent<MarkerScript>();
            if (ms.origin == origin && ms.posX == x && ms.posY == y) return item;
        }
        return null;
    }

    MarkerScript CreateMarkerWith(GameObject origin, int x, int y, MarkerScript.MarkerType type, bool ghost)
    {
        GameObject obj = Instantiate(marker, origin.transform.GetChild(1));
        MarkerScript ms = obj.GetComponent<MarkerScript>();
        ms.SetNewPos(x, y);
        ms.origin = origin;
        ms.reference = FindUnitAtGrid(x, y);
        ms.ghost = ghost;
        ms.ChangeMarkerType(type);
        //Debug.Log("created a move");
        return ms;
    }


    public void DestroyMarkers()
    {
        GameObject[] moves = GameObject.FindGameObjectsWithTag("move");

        foreach (var item in moves)
        {
            Destroy(item);
        }
        Debug.Log("destroyed moves");
    }



    public bool CheckTeamUnitsExists()
    {
        GameObject[] list = GameObject.FindGameObjectsWithTag("Unit");

        for (int i = 0; i < list.Length; ++i)
        {
            if (list[i].GetComponent<UnitScript>().team == gamemaster.GetComponent<GameMasterScript>().activeTeam) return true;
        }
        return false;
    }

    public bool CheckTeamUnitsExists(GameMasterScript.Team team)
    {
        GameObject[] list = GameObject.FindGameObjectsWithTag("Unit");

        // Debug.Log(list[1]);
        
        for (int i = 0; i < list.Length; ++i)
        {
            if (list[i].GetComponent<UnitScript>().team == team) return true;
        }
        return false;
    }

    public void LoadScenario(Scenario s)
    {
        ClearBoard();
        
        SetTileMap(s.terrain);
        PlaceUnits(s.units);
    }

    public void LoadSaveData(SaveData s)
    {
        ClearBoard();

        SetTileMap(s.terrain);
        PlaceUnits(s.units);
    }

    void SetTileMap(List<Terrain> t)
    {
        tilemap.ClearAllTiles();
        foreach (var item in t)
        {
            Vector3Int v = new(item.posX, item.posY);
            if (v.x < maxW) maxW = v.x;
            if (v.x > maxE) maxE = v.x;
            if (v.y > maxN) maxN = v.y;
            if (v.y < maxS) maxS = v.y;

            switch (item.sprite)
            {
                case "grass 1_0": tilemap.SetTile(v, grass1); break;
                case "grass 2_0": tilemap.SetTile(v, grass2); break;
                case "grass 3_0": tilemap.SetTile(v, grass3); break;
                case "grass 4_0": tilemap.SetTile(v, grass4); break;
                case "grass ball_0": tilemap.SetTile(v, grassball); break;
                case "void_0": tilemap.SetTile(v, voidtile); break;
                case "Square": tilemap.SetTile(v, extract); break;
            }
        }
        // Vector3Int pos = new(maxE + 1, maxN + 1);
        // tilemap.FloodFill(pos, voidtile);
        // pos.x = maxW - 1;
        // pos.y = maxS - 1;
        // tilemap.FloodFill(pos, voidtile);
    }

    void PlaceUnits(List<Units> u)
    {
        foreach (var item in u)
        {
            switch (item.team)
            {
                case "player1": SpawnUnit(item.name, GameMasterScript.Team.PLAYER1, item.posX, item.posY, false); break;
                case "player2": SpawnUnit(item.name, GameMasterScript.Team.PLAYER2, item.posX, item.posY, false); break;
                case "enemy1": SpawnUnit(item.name, GameMasterScript.Team.ENEMY1, item.posX, item.posY, false); break;
                case "enemy2": SpawnUnit(item.name, GameMasterScript.Team.ENEMY2, item.posX, item.posY, false); break;
            }
        }
    }

    void ClearBoard()
    {
        tilemap.ClearAllTiles();
        DestroyMarkers();
        DespawnAllUnits();
    }

    void DespawnAllUnits()
    {
        GameObject[] list = GameObject.FindGameObjectsWithTag("Unit");
        foreach (var item in list)
        {
            Destroy(item);
        }
    }

    public bool CrushRock(int x, int y)
    {
        if (GetTile(x, y) != grassball) return false;

        switch (Random.Range(1, 4))
        {
            case 1: tilemap.SetTile(new Vector3Int(x, y), grass1); break;
            case 2: tilemap.SetTile(new Vector3Int(x, y), grass2); break;
            case 3: tilemap.SetTile(new Vector3Int(x, y), grass3); break;
            case 4: tilemap.SetTile(new Vector3Int(x, y), grass4); break;
        }
        SoundMasterScript soundmaster = GameObject.Find("SoundMaster").GetComponent<SoundMasterScript>();
        soundmaster.Play("crush");
        return true;
    }
}