using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public delegate void TurnEndEventHandler();
    public event TurnEndEventHandler TurnEnded;

    public delegate void JumpEventHandler();
    public event JumpEventHandler Jumped;

    public int minTurnsForBearAttack = 2;
    public int maxTurnsForBearAttack = 6;

    public int minJumpsToWin = 3;
    public int maxJumpsToWin = 6;

    int jumpNumber = 0;
    int turnNumber = 0;
    int turnOfBearArrival = 4;
    int numberOfJumpsToWin = 4;

    public void EndTurn()
    {
        turnNumber++;
        if (TurnEnded != null)
        {
            TurnEnded();
        }
        if (turnNumber >= turnOfBearArrival)
        {
            BearsArrive();
        }
    }

    public void Jump()
    {
        jumpNumber++;
        if (jumpNumber == numberOfJumpsToWin)
        {
            Victory();
        }
        if (Jumped != null)
        {
            Jumped();
        }
        ResetTurns();
    }

    void ResetTurns()
    {
        turnNumber = 0;
        turnOfBearArrival = Random.Range(minTurnsForBearAttack, maxTurnsForBearAttack);
    }

    void ResetJumps()
    {
        jumpNumber = 0;
        numberOfJumpsToWin = Random.Range(minJumpsToWin, maxJumpsToWin);
    }

    void Victory()
    {
        // TODO
    }
    void BearsArrive()
    {
        // TODO
        Debug.Log("Bears Arrived =-0");
    }

    void Start () 
    {
        ResetTurns();
        ResetJumps();
    }
}
