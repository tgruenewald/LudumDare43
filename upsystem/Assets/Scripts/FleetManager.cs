using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FleetManager : MonoBehaviour {

    List<Ship> fleet = new List<Ship>();
    List<Ship> scoutingShips = new List<Ship>();

    public List<Ship> startingShips = new List<Ship>();
    public enum ShipActions { sacrifice, transfer, scout, repair}
    
    public void ShipAction(ShipActions action, Ship ship)
    {
        GameStateManager.Instance.ShipActionHandler(action);

        if (action == ShipActions.sacrifice)
        {
            fleet.Remove(ship);
        }
        else if (action == ShipActions.repair)
        {
            ship.Repair();
        }
        else if(action == ShipActions.transfer)
        {
            //TODO
        }
        else if(action == ShipActions.scout)
        {
            ship.Scout();
            scoutingShips.Add(ship);
            fleet.Remove(ship);
        }
    }
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

    public void UpdateScoutingShips()
    {
        foreach (Ship ship in scoutingShips)
        {
            
        }
    }

    void ReturnShip(Ship ship)
    {
        fleet.Add(ship);
        scoutingShips.Remove(ship);
    }
    void Scout(Ship ship)
    {
        
        int whatFind = Random.Range(0, 100);
        if(whatFind < 20)
        {
            // find ship
            // TODO : Instantiate Ship add it to our fleet
        }
        else if(whatFind < 40)
        {
            ReturnShip(ship);
        }
        else if(whatFind < 60)
        {
            // increase bearlon attack
            GameStateManager.Instance.IncreaseBearAttack();
            Debug.Log("Bears coming sooner");
            ReturnShip(ship);
        }

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
