//using System;
//enum ShipType { Unknown, Scout, Supply, Fuel, Passenger };
//enum ShipState { Idle, Scouting, Transfering, Repairing, Destroyed };
//enum Resource { Crew,Supply,Fuel};

//public class Ship
//{
//    //Current variables
//    private int _crew = 0;
//    private int _supply = 0;
//    private int _fuel = 0;
//    private bool _healthy = true;
//    private ShipType _type = ShipType.Unknown;
//    private ShipState _state = ShipState.Idle;

//    //max variables
//    private int _maxCrew = 2;
//    private int _maxSupply = 2;
//    private int _maxFuel = 2;
    
//    private Ship(int CurrentCrew, int CurrentSupply, int CurrentFuel, bool Healthy, ShipType SType, int MaxCrew, int MaxSupply, int MaxFuel)
//	{
//        //Set up  the new ship
//        _crew = CurrentCrew;
//        _supply = CurrentSupply;
//        _fuel = CurrentFuel;
//        _healthy = Healthy;
//        _type = SType ;
//        _maxCrew = MaxCrew;
//        _maxSupply = MaxSupply;
//        _maxFuel = MaxFuel;
//        _state = ShipState.Idle;
//	}

//    public string Name { get; set; }
//    public string State { get { return _state.ToString; } }

//    /// <summary>
//    /// Update the ship status during a jump
//    /// </summary>
//    public virtual void Jump()
//    {
//        //Check fuel status
//        if (_fuel > 0)
//        {
//            //Use fuel
//            _fuel -= 1;
//            //Consume Supply
//            if(_supply >= _crew)
//            {
//                _supply -= _crew;
//            }
//            else
//            {
//                //Only the crew will supply survives
//                _crew = _supply;
//                //Although they use up their supply
//                _supply = 0;
//            }
//            //Check survival
//            if (_crew < 1) _state = ShipState.Destroyed;
//        }
//        else
//        {
//            _state = ShipState.Destroyed;
//        }
//    }

//    public virtual void Transfer(bool TransferringToShip, Resource ResourceType, int Amount)
//    {
//        if(TransferringToShip)
//        {
//            switch (ResourceType)
//            {
//                case Resource.Crew:
//                    _crew += Amount;
//                    if (_crew > _maxCrew) _crew = _maxCrew;
//                    break;
//                case Resource.Supply:
//                    _supply += Amount;
//                    if (_supply > _maxSupply) _supply = _maxSupply;
//                    break;
//                case Resource.Fuel:
//                    _fuel += Amount;
//                    if (_fuel > _maxFuel) _fuel = _maxFuel;
//                    break;
//                default:
//                    break;
//            }
//        }
//        else //Transferring from this ship
//        {
//            switch (ResourceType)
//            {
//                case Resource.Crew:
//                    _crew -= Amount;
//                    if (_crew < 0) _crew = 0;
//                    break;
//                case Resource.Supply:
//                    _supply -= Amount;
//                    if (_supply < 0) _supply = 0;
//                    break;
//                case Resource.Fuel:
//                    _fuel -= Amount;
//                    if (_fuel < 0) _fuel = 0;
//                    break;
//                default:
//                    break;
//            }
//        }
//    }

//    /// <summary>
//    /// Send a ship scouting for a turn
//    /// </summary>
//    public virtual void Scout()
//    {
//        if (_state = ShipState.Idle)
//        {
//            _state = ShipState.Scouting;
//        }
//    }

//    /// <summary>
//    /// Set the ship to repair for the turn
//    /// </summary>
//    public virtual void Repair()
//    {
//        if (_state = ShipState.Idle)
//        {
//            _state = ShipState.Repairing;
//        }
//    }

//    /// <summary>
//    /// Call to damage the ship. If the ship survivies then true is returned. Otherwise the ship is destroyed when false is returned.
//    /// </summary>
//    /// <returns>Indicates if the ship survives the damage</returns>
//    public virtual bool TakeDamage()
//    {
//        if (_healthy)
//        {
//            _healthy = false;
//            return true;
//        }
//        else
//        {
//            _state = ShipState.Destroyed;
//            return false;
//        }
//    }

//    /// <summary>
//    /// Called to rest the ship's status
//    /// </summary>
//    public virtual void EndTurn()
//    {
//        if(_state = ShipState.Repairing)
//        {
//            _healthy = true;
//            _state = ShipState.Idle;
//        }
//        else if(_state = ShipState.Scouting)
//        {
//            //When happens here?
//        }
//        else
//        {
//            _state = ShipState.Idle;
//        }
//    }
//}
