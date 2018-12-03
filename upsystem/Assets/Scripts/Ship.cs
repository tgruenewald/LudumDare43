using System;
using UnityEngine;

public enum ShipType { Unknown, Scout, Supply, Fuel, Passenger };
public enum ShipState { Idle, Scouting, Transfering, Repairing, Destroyed };
public enum Resource { Crew,Supply,Fuel};

public class Ship : MonoBehaviour
{
    //Current variables
    [SerializeField]
    private int _crew = 0;
    [SerializeField]
    private int _supply = 0;
    [SerializeField]
    private int _fuel = 0;
    private bool _healthy = true;
    private ShipType _type = ShipType.Unknown;
    private ShipState _state = ShipState.Idle;
    private GameObject mHazardSprite;
    private GameObject mDamageEffect;

    //max variables
    [SerializeField]
    private int _maxCrew = 2;
    [SerializeField]
    private int _maxSupply = 2;
    [SerializeField]
    private int _maxFuel = 2;

    public int MaxCrew { get { return _maxCrew; } }
    public int MaxSupply { get { return _maxSupply; } }
    public int MaxFuel { get { return _maxFuel; } }
    public int Crew { get { return _crew; } }
    public int Supply { get { return _supply; } }
    public int Fuel { get { return _fuel; } }
    public ShipType Type {  get { return _type; } }
    private ActionSelector mActionSelector;
    private StatsSliders mStatsSliders;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            if (this.gameObject.transform.GetChild(i).CompareTag("ActionSelectors"))
            {
                mActionSelector = this.gameObject
                                      .transform
                                      .GetChild(i)
                                      .gameObject
                                      .GetComponent<ActionSelector>();
            }
            else if (this.gameObject.transform.GetChild(i).CompareTag("StatsSliders"))
            {
                mStatsSliders = this.gameObject
                                      .transform
                                      .GetChild(i)
                                      .gameObject
                                      .GetComponent<StatsSliders>();
            }
            else if (this.gameObject.transform.GetChild(i).CompareTag("HazardSprite"))
            {
                mHazardSprite = this.gameObject
                                      .transform
                                      .GetChild(i)
                                      .gameObject;
            }
            else if (this.gameObject.transform.GetChild(i).CompareTag("DamageEffect"))
            {
                mDamageEffect = this.gameObject
                                      .transform
                                      .GetChild(i)
                                      .gameObject;
            }
        }

    }

	void Update()
	{
        mHazardSprite.SetActive(_fuel <= 0 || _supply <= 0);
        mDamageEffect.SetActive(!_healthy);
	}

	public void EndShipTurn()
    {
        mActionSelector.Hide();
        SpriteRenderer renderer = this.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        renderer.color = new Color(1f, 1f, 1f, .5f);
    }

    void OnMouseUp()
    {
        if(_state == ShipState.Idle)
        {
            if (GameStateManager.Instance.GetState() == GameStateManager.GameState.defaultState)
            {
                mActionSelector.Toggle();
            }
            else if (GameStateManager.Instance.GetState() == GameStateManager.GameState.sacrifice)
            {
                _state = ShipState.Destroyed;
                GameStateManager.Instance.fleetManager.ShipAction(FleetManager.ShipActions.sacrifice, this);
                EndShipTurn();
            }
            else if (GameStateManager.Instance.GetState() == GameStateManager.GameState.transfer)
            {

                GameStateManager.Instance.fleetManager.ShipAction(FleetManager.ShipActions.transfer, this);
            }
        }
        mStatsSliders.Show();
        DialogManager.showShipSpec(this);
    }

    public void HideActions()
    {
        mActionSelector.Hide();
        DialogManager.hideShipSpec();
    }

	void OnMouseDown()
	{
        // more game jam bandaid silliness!
        mStatsSliders.Show();
        DialogManager.showShipSpec(this);
	}

	private void OnMouseOver()
    {
        mStatsSliders.Show();
        DialogManager.showShipSpec(this);
    }

    private void OnMouseExit()
    {
        mStatsSliders.Hide();
        DialogManager.hideShipSpec();
    }


    protected void Initialize(int CurrentCrew, int CurrentSupply, int CurrentFuel, bool Healthy, ShipType SType, int MaxCrew, int MaxSupply, int MaxFuel)
    {
        //Set up  the new ship
        //_crew = CurrentCrew;
        //_supply = CurrentSupply;
        //_fuel = CurrentFuel;
        _healthy = Healthy;
        _type = SType;
        //_maxCrew = MaxCrew;
        //_maxSupply = MaxSupply;
        //_maxFuel = MaxFuel;
        _state = ShipState.Idle;
    }

    //Properties
    private String _name = "null";
    public virtual String Name { get { return _name; } set { this._name += value; } }


    public String State { get { return _state.ToString(); } }


    /// <summary>
    /// Update the ship status during a jump
    /// </summary>
    public virtual void Jump()
    {
        if (!_healthy) _state = ShipState.Destroyed;
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
            if (_crew < 1) 
            {
                _state = ShipState.Destroyed;
                DialogManager.DisplayMessage("A ships crew starved and didn't make the jump");
            } 
        }
        else
        {
            _state = ShipState.Destroyed;
            DialogManager.DisplayMessage("A ship ran out of fuel and couldn't make the jump.");
        }
        //Check if the ship takes damage
        if(_state != ShipState.Destroyed)
        {
            float rand = UnityEngine.Random.Range(1, 100);
            //Ship takes damage if result less than a certain number
            if(rand < 34)
            {
                if (_healthy)
                {
                    _healthy = false;
                }
            }
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
        AudioManager.Instance.PlaySound(AudioClips.Transfer);
        ShipTo.EndShipTurn();
        ShipFrom.EndShipTurn();
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
                ShipTo.Transfer(true, ResourceType, ref Amount);
                ShipFrom.Transfer(false, ResourceType, ref Amount);

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

    private void Transfer(bool doIAdd, Resource ResourceType, ref int Amount)
    {
        if(doIAdd)
        {
            switch (ResourceType)
            {
                case Resource.Crew:
                    if (_crew + Amount > _maxCrew) {
                        Amount = _maxCrew - _crew;
                        _crew = _maxCrew;
                    } 
                    else {
                        _crew += Amount;
                    }
                    break;
                case Resource.Supply:
                    if (_supply + Amount > _maxSupply) {
                        Amount = _maxSupply - _supply;
                        _supply = _maxSupply;
                    } 
                    else {
                        _supply += Amount;
                    }
                    break;
                case Resource.Fuel:
                    if (_fuel + Amount > _maxFuel) {
                        Amount = _maxFuel - _fuel;
                        _fuel = _maxFuel;
                    } 
                    else {
                        _fuel += Amount;
                    }
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
        _state = ShipState.Transfering;
        mStatsSliders.UpdateSliders();
    }

    /// <summary>
    /// Send a ship scouting for a turn
    /// </summary>
    public virtual void Scout()
    {
        AudioManager.Instance.PlaySound(AudioClips.Scout);
        EndShipTurn();
        Debug.Log("Scouting.");
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
        AudioManager.Instance.PlaySound(AudioClips.Repair);
        EndShipTurn();

        Debug.Log("Repairing.");
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
        AudioManager.Instance.PlaySound(AudioClips.RedAlert);
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
        SpriteRenderer renderer = this.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        renderer.color = new Color(1f, 1f, 1f, 1f);
        if (_state == ShipState.Repairing)
        {
            _healthy = true;
            _state = ShipState.Idle;
        }
        else if(_state == ShipState.Scouting)
        {
            _state = ShipState.Idle;
        }
        else
        {
            _state = ShipState.Idle;
        }
    }

    public void SetResource(Resource ResourceType, int Amount)
    {
        switch (ResourceType)
        {
            case Resource.Crew:
                if (Amount < _maxCrew)
                {
                    _crew = Amount;
                }
                else
                {
                    _crew = _maxCrew;
                }
                break;
            case Resource.Supply:
                if (_supply < _maxSupply)
                {
                    _supply = Amount;
                }
                else
                {
                    _supply = _maxSupply;
                }
                break;
            case Resource.Fuel:
                if (_fuel < _maxFuel)
                {
                    _fuel = Amount;
                }
                else
                {
                    _fuel = _maxFuel;
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// When scouting, use this method to add resources
    /// </summary>
    /// <param name="ResourceType"></param>
    /// <param name="Amount"></param>
    public void AddResource(Resource ResourceType, ref int Amount)
    {
        switch (ResourceType)
        {
            case Resource.Crew:
                if (_crew < _maxCrew)
                {
                    _crew += Amount;
                    if (_crew > _maxCrew)
                    {
                        _crew = _maxCrew;
                        Amount = _crew;
                    }
                }
                break;
            case Resource.Supply:
                if(_supply < _maxSupply)
                {
                    _supply += Amount;
                    if (_supply > _maxSupply)
                    {
                        _supply = _maxSupply;
                        Amount = _supply;
                    }
                }
                break;
            case Resource.Fuel:
                if(_fuel < _maxFuel)
                {
                    _fuel += Amount;
                    if (_fuel > _maxFuel)
                    {
                        _fuel = _maxFuel;
                        Amount = _fuel;
                    } 
                }
                break;
            default:
                break;
        }
    }
}