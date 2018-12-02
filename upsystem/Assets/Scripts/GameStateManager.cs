using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameStateManager : MonoBehaviour
{
    Ship transferShip1;
    Ship transferShip2;
    Button jumpButton;
    Button endRoundButton;
    GameObject bearFleet;
    public enum GameState { sacrifice, transfer, defaultState };
    public static GameStateManager Instance;

    public delegate void TurnEndEventHandler();
    public event TurnEndEventHandler TurnEnded;

    public delegate void JumpEventHandler();
    public event JumpEventHandler Jumped;

    public int minTurnsForBearAttack = 4;
    public int maxTurnsForBearAttack = 8;

    public int minJumpsToWin = 3;
    public int maxJumpsToWin = 6;

    public FleetManager fleetManager;
    bool bearsArrived = false;
    public bool GetBearsArrived()
    {
        return bearsArrived;
    }

    private GameState gameState;
    int jumpNumber = 0;
    int turnNumber = 0;
    int turnOfBearArrival = 4;
    int numberOfJumpsToWin = 4;

    public void IncreaseBearAttack()
    {
        turnOfBearArrival--;
    }



    public void ShipActionHandler(FleetManager.ShipActions action, Ship ship)
    {
        jumpButton.interactable = false;
        // Also grey out jump button
        if (action == FleetManager.ShipActions.transfer)
        {
            // two ships are transfering
            if (gameState == GameState.transfer)
            {
                // this is when the final ship gets selected
                transferShip1 = ship;
                gameState = GameState.defaultState;
                Debug.Log(GameStateManager.Instance);

                Debug.Log("The real transfer");
                DialogManager.TransferDialog(transferShip2, transferShip1);

            }
            else
            {
                // Transfer is called first time
                DialogManager.DisplayMessage("Choose another to transfer to");
                transferShip2 = ship;
                gameState = GameState.transfer;
            }
        }
        else if (action == FleetManager.ShipActions.sacrifice)
        {
            endRoundButton.interactable = true;
            gameState = GameState.defaultState;
            DialogManager.SacrificeShipMessage(false);
            DialogManager.DisplayMessage("A ship distracted the Bearlons. You get another chance.");

            // TODO: do some animation.
        }
        else
        {
            fleetManager.CloseActionsExceptFor(ship);
        }
    }

    public void CloseActionsExceptFor(Ship ship)
    {
        fleetManager.CloseActionsExceptFor(ship);
    }

    public GameState GetState()
    {
        return gameState;
    }

    public void EndTurn()
    {
        bearFleet.SetActive(false);
        gameState = GameState.defaultState;
        turnNumber++;
        jumpButton.interactable = true;
        fleetManager.RemoveDestroyedShips();
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
        DialogManager.SacrificeShipMessage(false);
        jumpNumber++;
        gameState = GameState.defaultState;

        if (jumpNumber == numberOfJumpsToWin)
        {
            Victory();
        }
        if (Jumped != null)
        {
            Jumped();
        }
        if (fleetManager.getScoutingShipCount() > 0) {
            DialogManager.DisplayMessage("You jumped, but you abandoned " + fleetManager.getScoutingShipCount() + " scouting ships.");
        }
        else {
            DialogManager.DisplayMessage("You jumped to another system.  You are safe...for now." );
        }
        
        fleetManager.ClearScoutingShips();
        fleetManager.RemoveDestroyedShips();
        ResetTurns();
    }

    void ResetTurns()
    {
        bearsArrived = false;
        bearFleet.SetActive(false);
        jumpButton.interactable = true;
        endRoundButton.interactable = true;
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
        endRoundButton.interactable = false;
        bearsArrived = true;
        bearFleet.SetActive(true);
        Debug.Log("Bears Arrived =-0");
        DialogManager.DisplayMessage("Bearlons have arrived.  You may sacrifice a ship to distract them for one more turn or instead just jump now.");
        DialogManager.SacrificeShipMessage(true);
    }

    void Awake()
    {
        Instance = this;
    }
    void Start () 
    {
        GameObject jump = GameObject.Find("Jump");
        jumpButton = jump.GetComponent(typeof(Button)) as Button;

        GameObject endRound = GameObject.Find("EndTurn");
        endRoundButton = endRound.GetComponent(typeof(Button)) as Button;

        bearFleet = GameObject.Find("BearFleet");
        bearFleet.SetActive(false);

        gameState = GameState.defaultState;
        ResetTurns();
        ResetJumps();
    }
}
