using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsSliders : MonoBehaviour {

    private Slider mCrewSlider, mSuppliesSlider, mFuelSlider;
    private Text mCrewSliderText, mSuppliesSliderText, mFuelSliderText;
    private Ship mParentShip;

	// Use this for initialization
	void Start () {
        mCrewSlider = this.transform
                          .GetChild(0)
                           .gameObject
                           .GetComponent<Slider>();

        mCrewSliderText = this.transform
                              .GetChild(0)
                              .GetChild(1)
                              .gameObject.GetComponent<Text>();

        mSuppliesSlider = this.transform
                          .GetChild(1)
                          .gameObject
                          .GetComponent<Slider>();

        mSuppliesSliderText = this.transform
                              .GetChild(1)
                              .GetChild(1)
                              .gameObject.GetComponent<Text>();

        mFuelSlider = this.transform
                            .GetChild(2)
                            .gameObject
                            .GetComponent<Slider>();

        mFuelSliderText = this.transform
                              .GetChild(2)
                              .GetChild(1)
                              .gameObject.GetComponent<Text>();

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
        mCrewSliderText.text = inCrew.ToString();
    }

    public void SetSupplies(int inSupplies)
    {
        mSuppliesSlider.value = inSupplies;
        mSuppliesSliderText.text = inSupplies.ToString();
    }

    public void SetFuel(int inFuel)
    {
        mFuelSlider.value = inFuel;
        mFuelSliderText.text = inFuel.ToString();
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
