using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
 * To use the ActionSelector:
 * Add a Canvas and Event System to your main scene
 * -    Make the canvas in World Space
 * -    Fit the size to the camera
 * 
 * Add ships prefabs as children of the Canvas
 * 
 * Add this prefab as a child of your ship class
 * 
 * Add a collider to the ship to intercept mouse clicks
 * 
 * On mouse click, call the Toggle method on the ActionSelector
 * 
 * When we have a game state manager, connect the event system to the 
 * Scout, Xfer, and Repair callbacks
 * 
 */
public class ActionSelector : MonoBehaviour {

    // Getting children objects 
    private Ship mShip;
    private GameObject mActionContainer;
    private Button mScoutButton, mXferButton, mRepairButton;

	// Use this for initialization
	void Start () 
    {
        mShip = this.transform.parent.gameObject.GetComponent<Ship>();

        mActionContainer = this.gameObject
                               .transform.GetChild(0)
                               .gameObject;

        mScoutButton = this.mActionContainer.transform.GetChild(0)
                           .gameObject
                           .GetComponent<Button>();
        
        mXferButton = this.mActionContainer
                          .transform
                          .GetChild(1)
                          .gameObject
                          .GetComponent<Button>();

        mRepairButton = this.mActionContainer
                            .transform
                            .GetChild(2)
                            .gameObject
                            .GetComponent<Button>();

        //Calls the TaskOnClick/TaskWithParameters/ButtonClicked method when you click the Button
        //mScoutButton.onClick.AddListener(mShip.Scout);
        mXferButton.onClick.AddListener(XferClicked);
        //mRepairButton.onClick.AddListener(mShip.Repair);
	}

    // Set the buttons to show or not show depending on their current state
    public void Toggle()
    {
        AudioManager.Instance.PlaySound(AudioClips.ShipClick);
        GameStateManager.Instance.CloseActionsExceptFor(mShip);
        mActionContainer.SetActive(!mActionContainer.activeInHierarchy);
    }

    // Show the buttons
    public void Show()
    {
        GameStateManager.Instance.CloseActionsExceptFor(mShip);
        mActionContainer.SetActive(true);
    }

    // Hide the buttons
    public void Hide()
    {
        mActionContainer.SetActive(false);
    }

    public void ScoutClicked()
    {
    }

    public void XferClicked()
    {
        GameStateManager.Instance.fleetManager.ShipAction(FleetManager.ShipActions.transfer, mShip);
        AudioManager.Instance.PlaySound(AudioClips.Transfer);
        Debug.Log("Xfer clicked.");  

    }

    public void RepairClicked()
    {
        
    }
}
