using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public Ship transferShip1;  // 2nd ship selected
    public Ship transferShip2; // the 1st ship selected

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

    static GameObject transferDialog;

    public void IncreaseBearAttack()
    {
        turnOfBearArrival--;
    }

    public static void CloseTransferDialog() {
        Destroy(transferDialog);
    }


    public void ShipActionHandler(FleetManager.ShipActions action, Ship ship)
    {
        canJump = false; // As soon as a ship takes an action we can not jump
        if(action == FleetManager.ShipActions.transfer)
        {
            // two ships are transfering
            if(gameState == GameState.transfer)
            {
                // this is when the final ship gets selected
                transferShip1 = ship;
                gameState = GameState.defaultState;
                Debug.Log(GameStateManager.Instance);

                Debug.Log("The real transfer");



                transferDialog = (GameObject) Instantiate(Resources.Load("prefab/TransferDialog"),new Vector3(0, 0, 0), Quaternion.identity); //GameObject.Find("TransferDialog");
                transferDialog.transform.SetParent(GameObject.Find("DialogCanvas").transform);
                transferDialog.transform.localPosition =  new Vector3(0f, 0f, 0f);
                transferDialog.transform.localScale = new Vector3(1f, 1f, 1f);
                

                // yeah, yeah, it is kinda backwards.  Ship2 is the first ship
                transferDialog.GetComponent<TransferDialog>().setShip1(transferShip2);

                // annd.. ship 1 is the 2nd ship selected.
                transferDialog.GetComponent<TransferDialog>().setShip2(transferShip1);
                
                GameObject dialogCam = GameObject.Find("DialogCamera");
                dialogCam.GetComponent<Camera>().enabled = true;                
            }
            else
            {
                transferShip2 = ship;
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
        gameState = GameState.defaultState;
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
        fleetManager.RemoveDestroyedShips();
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
        gameState = GameState.defaultState;
        ResetTurns();
        ResetJumps();
    }
}
