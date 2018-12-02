using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransferDialog : MonoBehaviour {

	// Use this for initialization
	public GameObject crew1;
	public GameObject crew2;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void close() {
        GameObject dialogCam = GameObject.Find("DialogCamera");
        dialogCam.GetComponent<Camera>().enabled = false;		
	}

	public void setCrew1(int crewCount) {
		crew1.GetComponent<Text>().text = "" + crewCount;
	}
}
