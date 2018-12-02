using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FleetManager : MonoBehaviour {

    List<Ship> fleet = new List<Ship>();
    List<ScoutingFinds> scoutingShips = new List<ScoutingFinds>();

    class ScoutingFinds
    {
        public Ship ship;
        public int fuelFound;
        public int supplyFound;
        public int shipsFound;
        public bool doneScouting;
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
            ship.gameObject.SetActive(false);
            scoutingFind.doneScouting = false;
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
        List<ScoutingFinds> scoutingShips2 = new List<ScoutingFinds>();
        foreach (ScoutingFinds scoutingFind in scoutingShips)
        {
            if (!scoutingFind.doneScouting)
                scoutingShips2.Add(scoutingFind);
       
        }
        scoutingShips = scoutingShips2;
    }

    void ReturnShip(ScoutingFinds ship)
    {
        for(int i = 0; i < ship.shipsFound; i++)
        {
            // add ships;
        }
        ship.ship.gameObject.SetActive(true);

        fleet.Add(ship.ship);
        ship.doneScouting = true;
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
                Debug.Log("Found fuel");
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
                Debug.Log("Found supply");

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
            Debug.Log("Found ship");

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
            Debug.Log("found bears");
            ReturnShip(scoutingFind);
        }
        else
        {
            Debug.Log("found nothing");
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
