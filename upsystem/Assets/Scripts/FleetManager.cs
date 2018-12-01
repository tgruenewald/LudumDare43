using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FleetManager : MonoBehaviour {

    List<Ship> fleet = new List<Ship>();
    public List<Ship> startingShips = new List<Ship>();

    public void AddShipToFleet(Ship ship)
    {
        fleet.Add(ship);
        GameStateManager.Instance.Jumped += ship.Jump;
        GameStateManager.Instance.TurnEnded += ship.EndTurn;
    }

    public void RemoveShipFromFleet(Ship ship)
    {
        GameStateManager.Instance.Jumped -= ship.Jump;
        GameStateManager.Instance.TurnEnded -= ship.EndTurn;
        fleet.Remove(ship);
    }

    void FillStartingFleet()
    {
        foreach(Ship ship in startingShips)
        {
            AddShipToFleet(ship);
        }
    }

    void Start () 
    {
        FillStartingFleet();
    }
}
