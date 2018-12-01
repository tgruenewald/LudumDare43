using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public enum GameState { sacrifice, transfer, defaultState};
    public static GameStateManager Instance;

    public delegate void TurnEndEventHandler();
    public event TurnEndEventHandler TurnEnded;

    public delegate void JumpEventHandler();
    public event JumpEventHandler Jumped;

    public int minTurnsForBearAttack = 2;
    public int maxTurnsForBearAttack = 6;

    public int minJumpsToWin = 3;
    public int maxJumpsToWin = 6;

    bool bearsArrived = false;
    public bool GetBearsArrived()
    {
        return bearsArrived;
    }

    private GameState gameState;
    bool canJump = true;
    int jumpNumber = 0;
    int turnNumber = 0;
    int turnOfBearArrival = 4;
    int numberOfJumpsToWin = 4;

    void ShipActionEventHandler(Ship.ShipState state)
    {
        canJump = false; // As soon as a ship takes an action we can not jump
        if(state = Ship.Transfering)
        {
            // two ships are transfering
            if(gameState = Ship.Transfering)
            {
                gameState = defaultState;
            }
            else
            {
                gameState = transfer;
            }
        }
        // TODO
        //else if(state = //Ship.Sacrifice)
        //{
        // 
        //  gameState = default;
        //}
    }

    public GameState GetState()
    {
        return gameState;
    }

    public void EndTurn()
    {
        turnNumber++;
        canJump = true;
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
        if (!canJump)
            Debug.LogError("Shouldnt be able to jump");
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
        bearsArrived = false;
        canJump = true;
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
        gameState = sacrifice;
        bearsArrived = true;
        Debug.Log("Bears Arrived =-0");
    }

    void Start () 
    {
        ResetTurns();
        ResetJumps();
    }
}
