using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsSliders : MonoBehaviour {

    private Slider mCrewSlider, mSuppliesSlider, mFuelSlider;
    private Ship mParentShip;

	// Use this for initialization
	void Start () {
        mCrewSlider = this.transform
                          .GetChild(0)
                           .gameObject
                           .GetComponent<Slider>();

        mSuppliesSlider = this.transform
                          .GetChild(1)
                          .gameObject
                          .GetComponent<Slider>();

        mFuelSlider = this.transform
                            .GetChild(2)
                            .gameObject
                            .GetComponent<Slider>();
        mParentShip = this.transform.parent.GetComponent<Ship>();

        mCrewSlider.maxValue = mParentShip.MaxCrew;
        mSuppliesSlider.maxValue = mParentShip.MaxSupply;
        mFuelSlider.maxValue = mParentShip.MaxFuel;

        Initialize( mParentShip.Crew, mParentShip.Supply, mParentShip.Fuel );
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Initialize(int inCrew, int inSupplies, int inFuel)
    {
        SetCrew(inCrew);
        SetSupplies(inSupplies);
        SetFuel(inFuel);
    }

    public void SetCrew(int inCrew)
    {
        mCrewSlider.value = inCrew;
    }

    public void SetSupplies(int inSupplies)
    {
        mSuppliesSlider.value = inSupplies;
    }

    public void SetFuel(int inFuel)
    {
        mFuelSlider.value = inFuel;
    }

    // Set the buttons to show or not show depending on their current state
    public void Toggle()
    {
        mCrewSlider.gameObject.SetActive(!mCrewSlider.gameObject.activeInHierarchy);
        mSuppliesSlider.gameObject.SetActive(!mSuppliesSlider.gameObject.activeInHierarchy);
        mFuelSlider.gameObject.SetActive(!mFuelSlider.gameObject.activeInHierarchy);
    }

    // Show the buttons
    public void Show()
    {
        mCrewSlider.gameObject.SetActive(true);
        mSuppliesSlider.gameObject.SetActive(true);
        mFuelSlider.gameObject.SetActive(true);
    }

    // Hide the buttons
    public void Hide()
    {
        mCrewSlider.gameObject.SetActive(false);
        mSuppliesSlider.gameObject.SetActive(false);
        mFuelSlider.gameObject.SetActive(false);
    }

}
