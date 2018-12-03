using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {
    public GameObject tradeShip1;
    public GameObject tradeShip2;
    public GameObject repairShip;
    public GameObject scoutShip1;
    public GameObject scoutShip2;

    public string trade = "One of these ships is low on fuel and the other is low on supply. You can trade between them. Select one and press the transfer(yellow) icon.";
    public string repair = "This ship is damaged. You can select it and hit the red button to repair.";
    public string scout = "These other two ships can go out to scout. Select them and select the wave button.";
    
    public enum tutorialStates { trade, repair, scout};
    public tutorialStates currentState;
    public void TeachTrade()
    {
        HighlightShip highlight1 = tradeShip1.GetComponent<HighlightShip>();
        HighlightShip highlight2 = tradeShip2.GetComponent<HighlightShip>();

        highlight1.HighLight();
        highlight2.HighLight();
        DialogManager.DisplayMessage(trade);
    }

    public void TeachRepair()
    {
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
        currentState = tutorialStates.scout;
        HighlightShip highlight3 = repairShip.GetComponent<HighlightShip>();
        highlight3.StopHighlighting();

        HighlightShip highlight1 = scoutShip1.GetComponent<HighlightShip>();
        HighlightShip highlight2 = scoutShip2.GetComponent<HighlightShip>();

        highlight1.HighLight();
        highlight2.HighLight();
        DialogManager.DisplayMessage(scout);
    }

    void Start()
    {
        currentState = tutorialStates.trade;
    }
    // Update is called once per frame
    void Update () {
		
	}
}
