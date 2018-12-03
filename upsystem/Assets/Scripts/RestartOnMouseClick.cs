using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using UnityEngine;

public class RestartOnMouseClick : MonoBehaviour {

    public void PlayAgain()
    {
        SceneManager.LoadScene(0);
    }
    // Use this for initialization
    void Start () {
		
	}
}
