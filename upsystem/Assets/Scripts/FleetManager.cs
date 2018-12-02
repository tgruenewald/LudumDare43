using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FleetManager : MonoBehaviour {

    public int startingFuelMin = 1;
    public int startingFuelMax = 4;
    public int startingSupplyMin = 1;
    public int startingSupplyMax = 4;
    public int startingCrewMin = 1;
    public int startingCrewMax = 4;

    public List<Ship> fleet = new List<Ship>();
    List<ScoutingFinds> scoutingShips = new List<ScoutingFinds>();
    Canvas canvas;
    public GameObject canidateSpotObject;
    List<Vector3> canidateSpots = new List<Vector3>();
    public List<GameObject> shipsWeCanSpawn;

    GameObject ShipsScoutingCount = null;

    public class ScoutingFinds
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

    public void ClearScoutingShips()
    {
        scoutingShips.Clear();
        updateShipsScoutingCount();
    }

    public int getScoutingShipCount() {
        return scoutingShips.Count;
    }
    public void ShipAction(ShipActions action, Ship ship)
    {
        GameStateManager.Instance.ShipActionHandler(action, ship);

        if (action == ShipActions.repair)
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
            updateShipsScoutingCount();
            fleet.Remove(ship);
        }
    }

    public void updateShipsScoutingCount() {
        if (ShipsScoutingCount == null) {
            ShipsScoutingCount = GameObject.Find("ShipsScoutingCount");
        }

        ShipsScoutingCount.GetComponent<Text>().text = "" + scoutingShips.Count;
    }
    
    public void AddShipToFleet(Ship ship)
    {
        string result = string.Empty;
        int value = (int)(Random.Range(0.0f, 100.0f));
        while (--value >= 0)
        {
            result = (char)('A' + value % 26) + result;
            value /= 26;
        }
        ship.Name = result + fleet.Count.ToString() + ((int)(Random.Range(0.0f, 100.0f))).ToString();
        fleet.Add(ship);
        GameStateManager.Instance.Jumped += ship.Jump;
        GameStateManager.Instance.TurnEnded += ship.EndTurn;
    }

    public void RemoveShipFromFleet(Ship ship)
    {
        GameStateManager.Instance.Jumped -= ship.Jump;
        GameStateManager.Instance.TurnEnded -= ship.EndTurn;
        canidateSpots.Add(ship.transform.position);
        Destroy(ship.gameObject);
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
        updateShipsScoutingCount();
    }

    public void RemoveDestroyedShips()
    {
        List <Ship> fleet2 = new List<Ship>();
        foreach (Ship ship in fleet)
        {
            if(ship.State != "Destroyed")
            {
                fleet2.Add(ship);
            }
            else
            {
                Debug.Log("Destroy");
                RemoveShipFromFleet(ship);
            }
        }
        fleet = fleet2;
        updateShipsScoutingCount();
    }

    void CreateShip()
    {
        GameObject shipObject = Instantiate(shipsWeCanSpawn[Random.Range(0, shipsWeCanSpawn.Count)]);
        int index = Random.Range(0, canidateSpots.Count);
        Vector3 position = canidateSpots[index];
        canidateSpots.RemoveAt(index);
        shipObject.transform.parent = canvas.transform;
        shipObject.transform.position = position;
        Ship ship = shipObject.GetComponent(typeof(Ship)) as Ship;
        AddShipToFleet(ship);
        // Set starting values
        int fuel = Random.Range(startingFuelMin, startingFuelMax);
        int supply = Random.Range(startingFuelMin, startingFuelMax);
        int crew = Random.Range(startingFuelMin, startingFuelMax);
        int i = Random.Range(0, 3);
        if(i == 0)
        {
            fuel = 0;
        }
        if (i == 1)
        {
            supply = 0;
        }
        if (i == 2)
        {
            crew = 0;
        }
        ship.SetResource(Resource.Fuel, fuel);
        ship.SetResource(Resource.Supply, supply);
        ship.SetResource(Resource.Crew, crew);
    }
    void ReturnShip(ScoutingFinds ship)
    {
        for(int i = 0; i < ship.shipsFound; i++)
        {
            CreateShip();
        }
        ship.ship.gameObject.SetActive(true);

        fleet.Add(ship.ship);
        ship.doneScouting = true;
        updateShipsScoutingCount();

        DialogManager.ScoutingReturnedMessage(ship);
    }


    void Scout(ScoutingFinds scoutingFind)
    {
        Ship ship = scoutingFind.ship;
        // fuel ship
    //    if (ship.Type == ShipType.Fuel)
        {
            int foundStuff = Random.Range(0, 100);
            if(foundStuff < 40)
            {
                Debug.Log("Found fuel");
                int fuelFound = Random.Range(minFuelFound, maxFuelFound);
                ship.AddResource(Resource.Fuel, ref fuelFound);
                scoutingFind.fuelFound += fuelFound;
                if (ship.Fuel == ship.MaxFuel)
                {
                    ReturnShip(scoutingFind);
                    return;
                }
            }
        }
      //  else if(ship.Type == ShipType.Supply) // Supply ship
        {
            int foundStuff = Random.Range(0, 100);
            if (foundStuff < 40)
            {
                Debug.Log("Found supply");

                int supplyFound = Random.Range(minSupplyFound, maxSupplyFound);
                ship.AddResource(Resource.Supply, ref supplyFound);
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
            int index = Random.Range(0, canidateSpots.Count);
            Vector3 position = canidateSpots[index];
            canidateSpots.RemoveAt(index);
            ship.transform.position = position;
            ship.transform.parent = canvas.transform;
            AddShipToFleet(ship);
        }
    }

    void Start () 
    {
        GameObject canvasObj = GameObject.Find("Canvas");
        canvas = canvasObj.GetComponent(typeof(Canvas)) as Canvas;
        foreach (Transform trans in canidateSpotObject.transform)
        {
            canidateSpots.Add(trans.position);
        }
        FillStartingFleet();
    }
}
