using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FleetManager : MonoBehaviour {

    List<Ship> fleet;
    public List<Ship> startingShips = new List<Ship>();

    public void AddShipToFleet(Ship ship)
    {
        fleet.Add(ship);
        // TODO : Listen to events
    }

    void FillStartingFleet()
    {
        foreach(Ship ship in startingShips)
        {
            AddShipToFleet(ship);
        }
    }

    // Use this for initialization
    void Start () 
    {
        FillStartingFleet();
    }
}
