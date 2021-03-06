﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsSliders : MonoBehaviour {

    private Slider mCrewSlider, mSuppliesSlider, mFuelSlider;
    private Text mCrewSliderText, mSuppliesSliderText, mFuelSliderText;
    private Text mNameText;
    private Ship mParentShip;


    private int mCrew, mSupplies, mFuel;

    void Refresh()
    {
        needsRefresh = true;
    }

    private bool needsRefresh = false;
    public void UpdateSliders()
    {
        SetCrew(mParentShip.Crew);
        SetSupplies(mParentShip.Supply);
        SetFuel(mParentShip.Fuel);
    }

	// Use this for initialization
	void Start () {

        GameStateManager.Instance.Jumped += Refresh;
        GameStateManager.Instance.TurnEnded += Refresh;

        mCrewSlider = this.transform
                          .GetChild(0)
                           .gameObject
                           .GetComponent<Slider>();

        mCrewSliderText = this.transform
                              .GetChild(0)
                              .GetChild(2)
                              .gameObject.GetComponent<Text>();
        mCrewSliderText.fontSize = 15;

        mSuppliesSlider = this.transform
                          .GetChild(1)
                          .gameObject
                          .GetComponent<Slider>();

        mSuppliesSliderText = this.transform
                              .GetChild(1)
                              .GetChild(2)
                              .gameObject.GetComponent<Text>();
        mSuppliesSliderText.fontSize = 15;

        mFuelSlider = this.transform
                            .GetChild(2)
                            .gameObject
                            .GetComponent<Slider>();

        mFuelSliderText = this.transform
                              .GetChild(2)
                              .GetChild(2)
                              .gameObject.GetComponent<Text>();

        mFuelSliderText.fontSize = 15;

        mParentShip = this.transform.parent.GetComponent<Ship>();

        mCrewSlider.maxValue = mParentShip.MaxCrew;
        mSuppliesSlider.maxValue = mParentShip.MaxSupply;
        mFuelSlider.maxValue = mParentShip.MaxFuel;

        mNameText = this.transform
                              .GetChild(3)
                        .gameObject.GetComponent<Text>();
        mNameText.fontSize = 10;
        //Debug.Log(mNameText.text);

        Initialize( mParentShip.Crew, mParentShip.Supply, mParentShip.Fuel );
		
	}
	
	// Update is called once per frame
	void Update () 
    {
        // commented out.. addressing symptom of disappearing status bars. 
        //if (needsRefresh)
        //{
            UpdateSliders();
        //}
	}

    public void Initialize(int inCrew, int inSupplies, int inFuel)
    {
        mNameText.text = mParentShip.Name;
        SetCrew(inCrew);
        SetSupplies(inSupplies);
        SetFuel(inFuel);
    }

    public void SetCrew(int inCrew)
    {
        mCrew = inCrew;
        mCrewSlider.value = inCrew;
        mCrewSliderText.text = inCrew.ToString();
    }

    public void SetSupplies(int inSupplies)
    {
        mSupplies = inSupplies;
        mSuppliesSlider.value = inSupplies;
        mSuppliesSliderText.text = inSupplies.ToString();
    }

    public void SetFuel(int inFuel)
    {
        mFuel = inFuel;
        mFuelSlider.value = inFuel;
        mFuelSliderText.text = inFuel.ToString();
    }

    // Set the buttons to show or not show depending on their current state
    public void Toggle()
    {
        mCrewSlider.gameObject.SetActive(!mCrewSlider.gameObject.activeInHierarchy);
        mSuppliesSlider.gameObject.SetActive(!mSuppliesSlider.gameObject.activeInHierarchy);
        mFuelSlider.gameObject.SetActive(!mFuelSlider.gameObject.activeInHierarchy);
        mNameText.gameObject.SetActive(!mNameText.gameObject.activeInHierarchy);
        // Hacky game jam code!
        this.Initialize(mCrew, mSupplies, mFuel);
    }

    // Show the buttons
    public void Show()
    {
        mCrewSlider.gameObject.SetActive(true);
        mSuppliesSlider.gameObject.SetActive(true);
        mFuelSlider.gameObject.SetActive(true);
        mNameText.gameObject.SetActive(true);

        // Hacky game jam code!
        this.Initialize(mCrew, mSupplies, mFuel);
    }

    // Hide the buttons
    public void Hide()
    {
        mCrewSlider.gameObject.SetActive(false);
        mSuppliesSlider.gameObject.SetActive(false);
        mFuelSlider.gameObject.SetActive(false);
        mNameText.gameObject.SetActive(false);
    }

}
