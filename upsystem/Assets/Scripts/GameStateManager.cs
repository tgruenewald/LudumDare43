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

    public static void DisplayMessage(string msg) {
        GameObject dialog = (GameObject) Instantiate(Resources.Load("prefab/StatusDialog"),new Vector3(0, 0, 0), Quaternion.identity); //GameObject.Find("TransferDialog");
        dialog.transform.SetParent(GameObject.Find("DialogCanvas").transform);
        dialog.transform.localPosition =  new Vector3(0f, 0f, 0f);
        dialog.transform.localScale = new Vector3(1f, 1f, 1f);

        // dialog.GetComponent<DialogTrigger>()

        dialog.GetComponentInChildren<Text>().text = msg;
    }


    public void ShipActionHandler(FleetManager.ShipActions action, Ship ship)
    {
        jumpButton.interactable = false;
        // Also grey out jump button
        if (action == FleetManager.ShipActions.transfer)
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
                
                // GameObject dialogCam = GameObject.Find("DialogCamera");
                // dialogCam.GetComponent<Camera>().enabled = true;                
            }
            else
            {
                transferShip2 = ship;
                gameState = GameState.transfer;
            }
        }
        else if (action == FleetManager.ShipActions.sacrifice)
        {
            endRoundButton.interactable = true;
            gameState = GameState.defaultState;
            DisplayMessage("A ship distracted the Bearlons. You get another chance.");
        }
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
        jumpNumber++;
        if (jumpNumber == numberOfJumpsToWin)
        {
            Victory();
        }
        if (Jumped != null)
        {
            Jumped();
        }
        if (fleetManager.getScoutingShipCount() > 0) {
            GameStateManager.DisplayMessage("You jumped, but you abandoned " + fleetManager.getScoutingShipCount() + " scouting ships.");
        }
        else {
            GameStateManager.DisplayMessage("You jumped to another system.  You are safe...for now." );
        }
        
        fleetManager.ClearScoutingShips();
        fleetManager.RemoveDestroyedShips();
        ResetTurns();
    }

    void ResetTurns()
    {
        bearsArrived = false;
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
        DisplayMessage("Bearlons have arrived.  You may sacrifice a ship to distract them for one more turn or instead just jump now.");
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
