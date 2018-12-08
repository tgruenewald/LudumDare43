using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject tradeShip1;
    public GameObject tradeShip2;
    public GameObject repairShip;
    public GameObject scoutShip1;
    public GameObject scoutShip2;

    public string trade = "One of these ships is low on fuel and the other is low on supply. You can trade between them. Select one and press the transfer(yellow) icon.";
    public string repair = "This ship is damaged. You can select it and hit the red button to repair.";
    public string scout = "These other two ships can go out to scout. Select them and select the wave button. They may return with more resources... or they may not return at all. Only time will tell.";
    public string endTurn = "When you are done assigning actions to your ships you can end the turn.";

    public enum tutorialStates { trade, repair, scout };
    public tutorialStates currentState;

    int endTurnCount = 0;
    public void TeachEndTurn()
    {
        if (endTurnCount == 0)
        {
            endTurnCount++;
            return;
        }
        HighlightShip highlight1 = scoutShip1.GetComponent<HighlightShip>();
        HighlightShip highlight2 = scoutShip2.GetComponent<HighlightShip>();

        highlight1.StopHighlighting();
        highlight2.StopHighlighting();

        DialogManager.DisplayMessage(endTurn);
    }

    void SetShipsToMod(List<GameObject> shipObjs)
    {
        foreach (GameObject obj in shipObjs)
        {
            Ship ship = obj.GetComponent<Ship>() as Ship;
            ship.tutorialDisabled = false;
        }
    }

    public void TeachTrade()
    {
        List<GameObject> shipObjs = new List<GameObject>();
        shipObjs.Add(tradeShip1);
        shipObjs.Add(tradeShip2);
        SetShipsToMod(shipObjs);

        HighlightShip highlight1 = tradeShip1.GetComponent<HighlightShip>();
        HighlightShip highlight2 = tradeShip2.GetComponent<HighlightShip>();

        highlight1.HighLight();
        highlight2.HighLight();
        
        DialogManager.DisplayMessage(trade);
        DialogManager.DisplayMessage("The journey in hyperspace is long, and you will need 1 supply for each crew as well.");
        DialogManager.DisplayMessage("To make the journey you will need to make a series of jumps.  Each ship must have at least 1 hyper fuel to jump.");
        DialogManager.DisplayMessage("The route is treacherous and Bearlon ambushers threaten us at every turn...");
        DialogManager.DisplayMessage("The time has come for our fleet to make its way to the home world.");
    }

    public void TeachRepair()
    {
        List<GameObject> shipObjs = new List<GameObject>();
        shipObjs.Add(repairShip);
        SetShipsToMod(shipObjs);

        currentState = tutorialStates.repair;

        HighlightShip highlight1 = tradeShip1.GetComponent<HighlightShip>();
        HighlightShip highlight2 = tradeShip2.GetComponent<HighlightShip>();

        highlight1.StopHighlighting();
        highlight2.StopHighlighting();

        HighlightShip highlight3 = repairShip.GetComponent<HighlightShip>();

        highlight3.HighLight();
        DialogManager.DisplayMessage(repair);
    }

    public void TeachScout()
    {
        List<GameObject> shipObjs = new List<GameObject>();
        shipObjs.Add(scoutShip1);
        shipObjs.Add(scoutShip2);
        SetShipsToMod(shipObjs);

        currentState = tutorialStates.scout;
        HighlightShip highlight3 = repairShip.GetComponent<HighlightShip>();
        highlight3.StopHighlighting();

        HighlightShip highlight1 = scoutShip1.GetComponent<HighlightShip>();
        HighlightShip highlight2 = scoutShip2.GetComponent<HighlightShip>();

        highlight1.HighLight();
        highlight2.HighLight();
        DialogManager.DisplayMessage(scout);
    }

    public void EndTutorial()
    {
        
        HighlightShip highlight1 = tradeShip1.GetComponent<HighlightShip>();
        HighlightShip highlight2 = tradeShip2.GetComponent<HighlightShip>();
        HighlightShip highlight3 = repairShip.GetComponent<HighlightShip>();
        HighlightShip highlight4 = scoutShip1.GetComponent<HighlightShip>();
        HighlightShip highlight5 = scoutShip2.GetComponent<HighlightShip>();

        Ship ship1 = tradeShip1.GetComponent<Ship>() as Ship;
        ship1.tutorialDisabled = false;

        Ship ship2 = tradeShip2.GetComponent<Ship>() as Ship;
        ship2.tutorialDisabled = false;

        Ship ship3 = repairShip.GetComponent<Ship>() as Ship;
        ship3.tutorialDisabled = false;

        Ship ship4 = scoutShip1.GetComponent<Ship>() as Ship;
        ship4.tutorialDisabled = false;

        Ship ship5 = scoutShip2.GetComponent<Ship>() as Ship;
        ship5.tutorialDisabled = false;

        highlight1.StopHighlighting();
        highlight2.StopHighlighting();
        highlight3.StopHighlighting();
        highlight4.StopHighlighting();
        highlight5.StopHighlighting();
    }

    void Awake()
    {
        currentState = tutorialStates.trade;

        Ship ship1 = tradeShip1.GetComponent<Ship>() as Ship;
        ship1.tutorialDisabled = true;

        Ship ship2 = tradeShip2.GetComponent<Ship>() as Ship;
        ship2.tutorialDisabled = true;

        Ship ship3 = repairShip.GetComponent<Ship>() as Ship;
        ship3.tutorialDisabled = true;

        Ship ship4 = scoutShip1.GetComponent<Ship>() as Ship;
        ship4.tutorialDisabled = true;

        Ship ship5 = scoutShip2.GetComponent<Ship>() as Ship;
        ship5.tutorialDisabled = true;

    }
    // Update is called once per frame
    void Update()
    {

    }
}