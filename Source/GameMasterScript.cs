using System.Collections;
using UnityEngine;

public class GameMasterScript : MonoBehaviour
{
    private static readonly WaitForSeconds _waitForSeconds1 = new(1f);

    public enum Team { PLAYER1, PLAYER2, ENEMY1, ENEMY2, DUMMY}

    public Team activeTeam;
    public DataMasterScript datamaster;
    public BoardMasterScript boardmaster;
    public GameObject pausemenu;
    public GameObject gameovermenu;
    public AgentScript agent;
    public string scenarioName;
    public string gameMode;
    public bool gameOver = false;
    public bool gameActive = false;
    public int AIlevel = 1;


    public int extracted = 0;
    public int extractionLimit = 0;
    public int turn = 0;
    public int turnLimit = 0;
    public int unitcount;
    public int unitLimit = 0;

    public bool player1win;
    public bool player2win;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player1win = player2win = false;
        gameActive = false;
        gameOver = false;

        datamaster = GameObject.Find("DataMaster").GetComponent<DataMasterScript>();
        boardmaster = GameObject.Find("BoardMaster").GetComponent<BoardMasterScript>();
        agent = GameObject.Find("Agent").GetComponent<AgentScript>();
        //QuickLoad();

        
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     agent.Activate(1);
        // }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(gameOver)
            {
                GameOverScreen();
            }
            else if (gameActive && !gameOver)
            {
                PauseGame();
            }

        }

        if (gameActive)
        {
            if (CheckWinConditions() > 0) GameEnd();

            if (activeTeam != Team.PLAYER1 && activeTeam != Team.PLAYER2)
            {
                StartCoroutine(ActivateAgent(AIlevel));
            }
        }
    }
    
    IEnumerator ActivateAgent(int depth)
    {
        yield return _waitForSeconds1;
        if (activeTeam != Team.PLAYER1 && activeTeam != Team.PLAYER2)
            agent.Activate(depth);

    }

    public void EndTurn()
    {
        ++turn;
        ChangeTeam();
        // if (CheckWinConditions() > 0) GameEnd();
        //Debug.Log("endturn");
    }

    public void ChangeTeam()
    {
        switch (gameMode)
        {
            case "pvp": switch (activeTeam)
            {
                case Team.PLAYER1: activeTeam = Team.PLAYER2; break;
                case Team.PLAYER2: activeTeam = Team.PLAYER1; break;
            } break;

            case "tutorial":
            case "puzzle": switch (activeTeam)
            {
                case Team.PLAYER1: activeTeam = Team.ENEMY1; break;
                case Team.ENEMY1: activeTeam = Team.PLAYER1; break;
            } break;

            case "extraction":
            case "vAI": switch (activeTeam)
            {
                case Team.PLAYER1: activeTeam = Team.ENEMY1; break;
                case Team.ENEMY1: activeTeam = Team.PLAYER1; break;
            } break;

            case "AIvAI": switch (activeTeam)
            {
                case Team.ENEMY1: activeTeam = Team.ENEMY1; break;
                case Team.ENEMY2: activeTeam = Team.ENEMY2; break;
            } break; 
        }
        
    }

    public void GameEnd()
    {
        Debug.Log("game end");
        if(CheckWinConditions() > 15)
        switch (gameMode)
        {
            case "tutorial":
            case "puzzle":
            case "extraction":
            case "vAI":
                if (activeTeam == Team.PLAYER1) player1win = true;
                MasterScript master = GameObject.Find("Master").GetComponent<MasterScript>();
                master.UpdateProgress();
                break;

            case "PVP":
                if (activeTeam == Team.PLAYER2) player2win = true;
                if (activeTeam == Team.PLAYER1) player1win = true;

                break;
        }

        gameOver = true;
        gameActive = false;
        GameOverScreen();
    }
    
    public void GameOverScreen()
    {
        gameovermenu.SetActive(true);
        gameovermenu.GetComponent<GameOverScript>().DisplayEndCondition();
    }

    public void QuickSave()
    {
        datamaster.QuickSave();
    }
    public void QuickLoad()
    {
        SaveData savedata = datamaster.savedata;
        scenarioName = savedata.scenarioName;
        gameMode = savedata.gameMode;
        gameOver = savedata.gameOver;
        AIlevel = savedata.AIlevel;
        turn = savedata.turn;
        turnLimit = savedata.turnLimit;
        extracted = savedata.extracted;
        extractionLimit = savedata.extractionLimit;
        activeTeam = datamaster.StringToTeam(savedata.activeTeam);

        boardmaster.LoadSaveData(datamaster.savedata);
        gameActive = true;
    }

    public void PauseGame()
    {
        gameActive = false;
        pausemenu.SetActive(true);
        pausemenu.GetComponent<PauseMenuScript>().Restart();
    }

    public void RefocusGame()
    {
        if (!gameOver) gameActive = true;
    }


    public void Restart()
    {
        turn = 0;
        extracted = 0;
        boardmaster.LoadScenario(datamaster.scenario);
        gameOver = false;
    }

    public void LoadScenario()
    {
        turn = 0;
        extracted = 0;
        gameMode = datamaster.scenario.gameMode;
        AIlevel = datamaster.scenario.AIlevel;
        scenarioName = datamaster.scenario.scenarioName;
        turnLimit = datamaster.scenario.turnLimit;
        extractionLimit = datamaster.scenario.extractionLimit;
        unitLimit = datamaster.scenario.unitLimit;
        unitcount = datamaster.scenario.unitcount;
        activeTeam = datamaster.StringToTeam(datamaster.scenario.activeTeam);
        boardmaster.LoadScenario(datamaster.scenario);
        gameOver = false;
        player1win = player2win = false;
        gameActive = true;
    }

    public int CheckWinConditions()
    {
        int flags = 0;

        if (!boardmaster.CheckTeamUnitsExists(Team.PLAYER1)) flags += 1;
        if (!boardmaster.CheckTeamUnitsExists(Team.ENEMY1)) flags += 16;
        if (extractionLimit > 0 && extracted > extractionLimit) flags += 32;
        if (turnLimit > 0 && turn > turnLimit) flags += 2;
        if (unitLimit > 0 && unitcount < unitLimit) flags += 4;

        Debug.Log("end flags: " + flags);
        return flags;
    }
}
