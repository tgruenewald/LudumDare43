using System;
using UnityEngine;

public enum ShipType { Unknown, Scout, Supply, Fuel, Passenger };
public enum ShipState { Idle, Scouting, Transfering, Repairing, Destroyed };
public enum Resource { Crew,Supply,Fuel};

public abstract class Ship : MonoBehaviour
{
    //Current variables
    private int _crew = 0;
    private int _supply = 0;
    private int _fuel = 0;
    private bool _healthy = true;
    private ShipType _type = ShipType.Unknown;
    private ShipState _state = ShipState.Idle;

    //max variables
    private int _maxCrew = 2;
    private int _maxSupply = 2;
    private int _maxFuel = 2;

    protected void Initialize(int CurrentCrew, int CurrentSupply, int CurrentFuel, bool Healthy, ShipType SType, int MaxCrew, int MaxSupply, int MaxFuel)
    {
        //Set up  the new ship
        _crew = CurrentCrew;
        _supply = CurrentSupply;
        _fuel = CurrentFuel;
        _healthy = Healthy;
        _type = SType;
        _maxCrew = MaxCrew;
        _maxSupply = MaxSupply;
        _maxFuel = MaxFuel;
        _state = ShipState.Idle;
    }

    //Properties
    public string Name { get; set; }
    public string State { get { return _state.ToString; } }
    public int Crew { get { return _crew; } }
    public int Supply { get { return _supply; } }
    public int Fuel { get { return _fuel; } }
    public int MaxCrew { get { return _maxCrew; } }
    public int MaxSupply { get { return _maxSupply; } }
    public int MaxFuel { get { return _maxFuel; } }

    /// <summary>
    /// Update the ship status during a jump
    /// </summary>
    public virtual void Jump()
    {
        //Check fuel status
        if (_fuel > 0)
        {
            //Use fuel
            _fuel -= 1;
            //Consume Supply
            if(_supply >= _crew)
            {
                _supply -= _crew;
            }
            else
            {
                //Only the crew will supply survives
                _crew = _supply;
                //Although they use up their supply
                _supply = 0;
            }
            //Check survival
            if (_crew < 1) _state = ShipState.Destroyed;
        }
        else
        {
            _state = ShipState.Destroyed;
        }
    }

    /// <summary>
    /// Initiates a resource transfer between ships
    /// </summary>
    /// <param name="ShipTo">The Ship that resources are being transferred to. The ship that resources are being transferred from.</param>
    /// <param name="ShipFrom"></param>
    /// <param name="ResourceType">Resource to transfer.</param>
    /// <param name="Amount">Amount that is transferred between ships. This amount is updated based on ship's stock.</param>
    /// <returns></returns>
    public static bool Transfer(Ship ShipTo, Ship ShipFrom, Resource ResourceType, ref int Amount)
    {
        //Check if either ship is busy
        if ((ShipFrom._state == ShipState.Idle || ShipFrom._state == ShipState.Transfering) &&
            (ShipTo._state == ShipState.Idle || ShipTo._state == ShipState.Transfering))
        {
            //Update the transfer amount based on the ship's current amount
            switch (ResourceType)
            {
                case Resource.Crew:
                    if (ShipFrom.Crew <= 0) return false;
                    if (ShipFrom.Crew < Amount) Amount = ShipFrom.Crew;
                    break;
                case Resource.Supply:
                    if (ShipFrom.Supply <= 0) return false;
                    if (ShipFrom.Supply < Amount) Amount = ShipFrom.Supply;
                    break;
                case Resource.Fuel:
                    if (ShipFrom.Fuel <= 0) return false;
                    if (ShipFrom.Fuel < Amount) Amount = ShipFrom.Fuel;
                    break;
                default:
                    break;
            }
            if (Amount > 0)
            {
                ShipFrom.Transfer(false, ResourceType, Amount);
                ShipTo.Transfer(true, ResourceType, Amount);
                return true;
            }
            //nothing to transfer
            else return false;
        }
        else //One or more ships are already busy
        {
            return false;
        }
    }

    public virtual void Transfer(bool TransferringToShip, Resource ResourceType, int Amount)
    {
        if(TransferringToShip)
        {
            switch (ResourceType)
            {
                case Resource.Crew:
                    _crew += Amount;
                    if (_crew > _maxCrew) _crew = _maxCrew;
                    break;
                case Resource.Supply:
                    _supply += Amount;
                    if (_supply > _maxSupply) _supply = _maxSupply;
                    break;
                case Resource.Fuel:
                    _fuel += Amount;
                    if (_fuel > _maxFuel) _fuel = _maxFuel;
                    break;
                default:
                    break;
            }
        }
        else //Transferring from this ship
        {
            switch (ResourceType)
            {
                case Resource.Crew:
                    _crew -= Amount;
                    if (_crew < 0) _crew = 0;
                    break;
                case Resource.Supply:
                    _supply -= Amount;
                    if (_supply < 0) _supply = 0;
                    break;
                case Resource.Fuel:
                    _fuel -= Amount;
                    if (_fuel < 0) _fuel = 0;
                    break;
                default:
                    break;
            }
        }
        _state = ShipState.Transfering
    }

    /// <summary>
    /// Send a ship scouting for a turn
    /// </summary>
    public virtual void Scout()
    {
        if (_state == ShipState.Idle)
        {
            _state = ShipState.Scouting;
        }
    }

    /// <summary>
    /// Set the ship to repair for the turn
    /// </summary>
    public virtual void Repair()
    {
        if (_state == ShipState.Idle)
        {
            _state = ShipState.Repairing;
        }
    }

    /// <summary>
    /// Call to damage the ship. If the ship survivies then true is returned. Otherwise the ship is destroyed when false is returned.
    /// </summary>
    /// <returns>Indicates if the ship survives the damage</returns>
    public virtual bool TakeDamage()
    {
        if (_healthy)
        {
            _healthy = false;
            return true;
        }
        else
        {
            _state = ShipState.Destroyed;
            return false;
        }
    }

    /// <summary>
    /// Called to rest the ship's status
    /// </summary>
    public virtual void EndTurn()
    {
        if(_state == ShipState.Repairing)
        {
            _healthy = true;
            _state = ShipState.Idle;
        }
        else if(_state == ShipState.Scouting)
        {
            //When happens here?
        }
        else
        {
            _state = ShipState.Idle;
        }
    }
}

class Scout : Ship
{
    public void initialize(int StartingCrew, int StartingSupply, int StartingFuel, bool Damaged)
    {
        base.Initialize(StartingCrew, StartingSupply, StartingFuel, !Damaged, ShipType.Scout, 2, 2, 4);
    }
}

class Supply : Ship
{
    public void initialize(int StartingCrew, int StartingSupply, int StartingFuel, bool Damaged)
    {
        base.Initialize(StartingCrew, StartingSupply, StartingFuel, !Damaged, ShipType.Supply, 4, 32, 2);
        }
}

class Fuel : Ship
{
    public void initialize(int StartingCrew, int StartingSupply, int StartingFuel, bool Damaged)
    {
        base.Initialize(StartingCrew, StartingSupply, StartingFuel, !Damaged, ShipType.Fuel, 4, 2, 12);
    }
}

class Passenger : Ship
{
    public void initialize(int StartingCrew, int StartingSupply, int StartingFuel, bool Damaged)
    {
        base.Initialize(StartingCrew, StartingSupply, StartingFuel, !Damaged, ShipType.Passenger, 8, 2, 2);
    }
} 