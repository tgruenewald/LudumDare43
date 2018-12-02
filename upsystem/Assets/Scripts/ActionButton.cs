using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton : MonoBehaviour {
    public Ship ship;
    public FleetManager.ShipActions action;
    
    public void ShipAction()
    {
        GameStateManager.Instance.fleetManager.ShipAction(action, ship);
    }
}
