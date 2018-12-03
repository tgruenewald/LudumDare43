using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusDialog : MonoBehaviour {

	Ship ship;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void setShipToBeDestroyed(Ship myship)
	{
		ship = myship;
	}

	public void close() {
		Destroy(gameObject);
	}

	public void letlive() {
		close();
	}

	public void theyshoulddie() {
		DialogManager.SacrificeShip();
		close();
	}
}
