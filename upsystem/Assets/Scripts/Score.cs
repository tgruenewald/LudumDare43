using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Score : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        Text text = this.GetComponent<Text>();
        int score = GameStateManager.Instance.fleetManager.NumberOfCrew();
        text.text = "You brought " + score +  " sal-mon to the home world";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
