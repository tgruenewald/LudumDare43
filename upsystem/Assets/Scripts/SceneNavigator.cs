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
	public void load_level_scene() {
		SceneManager.LoadScene("choose_level");
	}	
	public void load_credit_scene() {
		SceneManager.LoadScene("credit");
	}	

	public void load_space_scene_easy() {
		PlayerPrefs.SetInt("level", 1);
		SceneManager.LoadScene("spacefield");
	}	

	public void load_space_scene_normal() {
		PlayerPrefs.SetInt("level", 2);
		SceneManager.LoadScene("spacefield");
	}		

	public void load_space_scene_hard() {
		PlayerPrefs.SetInt("level", 3);
		SceneManager.LoadScene("spacefield");
	}	
		
}

