﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameStateManager : MonoBehaviour
{
    public TradeLine tradeLine;
    public Tutorial tutorial;
    public GameObject defeatScreen;
    public GameObject victoryScreen;
    [HideInInspector]
    public Ship transferShip1;
    [HideInInspector]
    public Ship transferShip2;
    Button jumpButton;
    Button endRoundButton;
    GameObject bearFleet;
    public bool tutorialOn = true;
    public enum GameState { sacrifice, transfer, defaultState, gameOver };
    public static GameStateManager Instance;

    public delegate void TurnEndEventHandler();
    public event TurnEndEventHandler TurnEnded;

    public delegate void JumpEventHandler();
    public event JumpEventHandler Jumped;

    public int minTurnsForBearAttack = 4;
    public int maxTurnsForBearAttack = 8;

    public int minJumpsToWin = 14;
    public int maxJumpsToWin = 18;

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
                transferShip1 = ship;
                // this is when the final ship gets selected
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
            AudioManager.Instance.PlaySound(AudioClips.Sacrifice);
            fleetManager.HighlightShips(false);
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

        bool bears = false;
        if(tutorialOn)
        {
            bears = true;
            tutorialOn = false;
            tutorial.EndTutorial();
            turnOfBearArrival = 0;
        }
        fleetManager.HighlightShips(false);
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
        if (turnNumber >= turnOfBearArrival || bears)
        {
            BearsArrive();
        }
    }

    public void Jump()
    {
        fleetManager.HighlightShips(false);
        DialogManager.SacrificeShipMessage(false);
        jumpNumber++;
        updateJumpsToWin();
        gameState = GameState.defaultState;

        if (tutorialOn)
        {
            tutorialOn = false;
            tutorial.EndTutorial();

        }
        if (Jumped != null)
        {
            Jumped();
        }
        fleetManager.RemoveDestroyedShips();
        if(fleetManager.fleet.Count == 0)
        {
            Defeat();
            return;
        }
        if (jumpNumber == numberOfJumpsToWin)
        {
            Victory();
            return;
        }
        if (fleetManager.getScoutingShipCount() > 0) {
            DialogManager.DisplayMessage("You jumped, but you abandoned " + fleetManager.getScoutingShipCount() + " scouting ships.");
        }
        else {
            DialogManager.DisplayMessage("You jumped to another system.  You are safe...for now." );
        }
        
        fleetManager.ClearScoutingShips();

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
        
        int level = PlayerPrefs.GetInt("level");
        Debug.Log("Level " + level);
        switch(level)
        {
            case 1:
                minJumpsToWin = 4;
                maxJumpsToWin = 6;
                break;
            case 2:
                minJumpsToWin = 8;
                maxJumpsToWin = 12;
                break;
            case 3:
                minJumpsToWin = 16;
                maxJumpsToWin = 20;
                break;
        }
        Debug.Log("min = " + minJumpsToWin + ", max = " + maxJumpsToWin);
        numberOfJumpsToWin = Random.Range(minJumpsToWin, maxJumpsToWin);
        updateJumpsToWin();
    }
    public void updateJumpsToWin() {
        GameObject counter = GameObject.Find("JumpsLeftCount");

        counter.GetComponent<Text>().text = "" + (numberOfJumpsToWin - jumpNumber);
    }
    void Victory()
    {
        GameObject camera = GameObject.Find("Main Camera");
        camera.SetActive(false);
        victoryScreen.SetActive(true);
        gameState = GameState.gameOver;

        GameObject.Find("FleetStat").SetActive(false);
    }

    void Defeat()
    {
        AudioManager.Instance.PlaySound(AudioClips.RedAlert);
        GameObject camera = GameObject.Find("Main Camera");
        camera.SetActive(false);
        defeatScreen.SetActive(true);
        gameState = GameState.gameOver;
        GameObject.Find("FleetStat").SetActive(false);

    }

    void BearsArrive()
    {
        fleetManager.HighlightShips(true);
        AudioManager.Instance.PlaySound(AudioClips.BearGrowl);
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
        GameObject jump = GameObject.FindWithTag("JumpButton");
        jumpButton = jump.GetComponent(typeof(Button)) as Button;
        if(!jumpButton)
        {
            Debug.LogError("jump button not found");
        }

        GameObject endRound = GameObject.Find("EndTurn2");

        endRoundButton = endRound.GetComponent(typeof(Button)) as Button;

        bearFleet = GameObject.Find("BearFleet");
        bearFleet.SetActive(false);

        gameState = GameState.defaultState;
        ResetTurns();
        ResetJumps();
        tutorial.TeachTrade();
    }

    void Update()
    {
        if(gameState == GameState.transfer && Input.GetMouseButtonUp(1))
        {
            tradeLine.ClearLine();
            gameState = GameState.defaultState;
        }
    }
}
