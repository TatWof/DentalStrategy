using System;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameMasterScript gamemaster;
    public BoardMasterScript boardmaster;

    public float cameraSpeed;
    public bool activated;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = new Vector3(0, 0, -10);
    }

    // Update is called once per frame
    void Update()
    {
        if (gamemaster.gameActive && activated)
        {
            float movex = 0, movey = 0;

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                movex = -cameraSpeed;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                movex = +cameraSpeed;
            }
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                movey = +cameraSpeed;
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                movey = -cameraSpeed;
            }
            transform.position = new Vector3(transform.position.x + movex, transform.position.y + movey, -10);
        }
    }

    // void LateUpdate()
    // {
    //     Vector3 pos = transform.position;
    //     pos.x = Math.Clamp(transform.position.x, boardmaster.maxW + 4, boardmaster.maxE - 3);
    //     pos.y = Math.Clamp(transform.position.y, boardmaster.maxS + 4, boardmaster.maxN - 3);
    //     transform.position = pos;
    // }
}
