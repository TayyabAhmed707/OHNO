using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager gm;
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            gm.ServerStartGame();
        }
    }
}
