using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneNavigator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void load_title_scene() {
		SceneManager.LoadScene("title");
	}

	public void load_credit_scene() {
		SceneManager.LoadScene("credit");
	}	

	public void load_space_scene() {
		SceneManager.LoadScene("spacefield");

	}	
}

