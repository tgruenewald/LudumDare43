using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FleetManager : MonoBehaviour {

    List<Ship> fleet = new List<Ship>();
    List<ScoutingFinds> scoutingShips = new List<ScoutingFinds>();

    struct ScoutingFinds
    {
        public Ship ship;
        public int fuelFound;
        public int supplyFound;
        public int shipsFound;
    }

    public List<Ship> startingShips = new List<Ship>();
    public enum ShipActions { sacrifice, transfer, scout, repair}

    public int minFuelFound = 1;
    public int maxFuelFound = 10;
    public int minSupplyFound = 1;
    public int maxSupplyFound = 10;

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
            ScoutingFinds scoutingFind = new ScoutingFinds();
            scoutingFind.fuelFound = 0;
            scoutingFind.shipsFound = 0;
            scoutingFind.supplyFound = 0;
            scoutingFind.ship = ship;
            scoutingShips.Add(scoutingFind);
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
        foreach (ScoutingFinds scoutingFind in scoutingShips)
        {
            Scout(scoutingFind);
        }
    }

    void ReturnShip(ScoutingFinds ship)
    {
        for(int i = 0; i < ship.shipsFound; i++)
        {
            // add ships;
        }
        fleet.Add(ship.ship);
        scoutingShips.Remove(ship);
    }

    void Scout(ScoutingFinds scoutingFind)
    {
        Ship ship = scoutingFind.ship;
        // fuel ship
        if (ship.Type == ShipType.Fuel)
        {
            int foundStuff = Random.Range(0, 100);
            if(foundStuff < 30)
            {
                int fuelFound = Random.Range(minFuelFound, maxFuelFound);
                ship.AddResource(Resource.Fuel, fuelFound);
                scoutingFind.fuelFound += fuelFound;
                if (ship.Fuel == ship.MaxFuel)
                {
                    ReturnShip(scoutingFind);
                    return;
                }
            }
        }
        else if(ship.Type == ShipType.Supply) // Supply ship
        {
            int foundStuff = Random.Range(0, 100);
            if (foundStuff < 30)
            {
                int supplyFound = Random.Range(minSupplyFound, maxSupplyFound);
                ship.AddResource(Resource.Supply, supplyFound);
                scoutingFind.supplyFound += supplyFound;

                if (ship.Supply == ship.MaxSupply)
                {
                    ReturnShip(scoutingFind);
                    return;
                }
            }
        }


        int whatFind = Random.Range(0, 100);
        if(whatFind < 20)
        {
            scoutingFind.shipsFound++;
            if(scoutingFind.shipsFound == 2)
            {
                ReturnShip(scoutingFind);
            }
        }
        else if(whatFind < 40)
        {
            ReturnShip(scoutingFind);
        }
        else if(whatFind < 60)
        {
            // increase bearlon attack
            GameStateManager.Instance.IncreaseBearAttack();
            Debug.Log("Bears coming sooner");
            ReturnShip(scoutingFind);
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
