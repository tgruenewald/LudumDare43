using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ShipTest : MonoBehaviour {

    public ActionSelector mActionSelector;
	// Use this for initialization
	void Start () {
		
	}

    void OnMouseUp()
    {
        mActionSelector.Toggle();
    }
}
