using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public enum GameState { sacrifice, transfer, defaultState };
    public static GameStateManager Instance;

    public delegate void TurnEndEventHandler();
    public event TurnEndEventHandler TurnEnded;

    public delegate void JumpEventHandler();
    public event JumpEventHandler Jumped;

    public int minTurnsForBearAttack = 2;
    public int maxTurnsForBearAttack = 6;

    public int minJumpsToWin = 3;
    public int maxJumpsToWin = 6;

    public FleetManager fleetManager;
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

    // TODO: Remove later
    public Ship ship1;
    public Ship ship2;

    public void IncreaseBearAttack()
    {
        turnOfBearArrival--;
    }

    public void ShipActionHandler(FleetManager.ShipActions action)
    {
        canJump = false; // As soon as a ship takes an action we can not jump
        if(action == FleetManager.ShipActions.transfer)
        {
            // two ships are transfering
            if(gameState == GameState.transfer)
            {
                gameState = GameState.defaultState;
            }
            else
            {
                gameState = GameState.transfer;
            }
        }
        else if (action == FleetManager.ShipActions.sacrifice)
        {
            gameState = GameState.defaultState;
        }
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
        fleetManager.UpdateScoutingShips();
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
        gameState = GameState.sacrifice;
        bearsArrived = true;
        Debug.Log("Bears Arrived =-0");
    }

    void Awake()
    {
        Instance = this;
    }
    void Start () 
    {
        ResetTurns();
        ResetJumps();
    }
}
