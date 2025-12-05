using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FrameScript : MonoBehaviour
{
    private static readonly WaitForSecondsRealtime _waitForSecondsRealtime = new(0.5f);
    public Animator anim;
    public int posX, posY;

    public Sprite dummy, baby, incis, broadincis, canine, pawn, premolar, scaredpremolar, molar, bigmolar, king;
    public GameMasterScript.Team team;

    public bool active = true;

    void Update()
    {
        if (!active)
        {
            Destroy(gameObject);
        }
    }

    public void SetUpFrame(int x, int y, GameMasterScript.Team t, string name)
    {
        this.name = name;
        posX = x;
        posY = y;
        team = t;
        NormalizePos();
        SetSprite();
    }


    public void NormalizePos()
    {
        transform.position = new Vector2(posX + 0.5f, posY + 0.5f);
    }

    public void SetSprite()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = name switch
        {
            "baby" => baby,
            "incis" => incis,
            "broadincis" => broadincis,
            "canine" => canine,
            "pawn" => pawn,
            "premolar" => premolar,
            "scaredpremolar" => scaredpremolar,
            "molar" => molar,
            "bigmolar" => bigmolar,
            "king" => king,
            _ => dummy,
        };

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
    public IEnumerator PlayDie()
    {
        yield return new WaitForNextFrameUnit();
        anim.Play("Die 1");
        Debug.Log("Die");
        yield return new WaitForSecondsRealtime(0.5f);
        active = false;
        gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
    }

    public IEnumerator PlayExtract()
    {
        yield return new WaitForNextFrameUnit();
        anim.Play("Extract 1");
        Debug.Log("extract");
        yield return new WaitForSecondsRealtime(0.5f);
        active = false;
        gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;

    }

    public IEnumerator PlayMove(int x, int y, int steps)
    {
        for (int i = 0; i < steps; i++)
        {
            yield return new WaitForSecondsRealtime(0.5f / steps);
            transform.position += new Vector3((float)(x - posX)/steps, (float)(y-posY)/steps);
        }
        active = false;
    }
}
