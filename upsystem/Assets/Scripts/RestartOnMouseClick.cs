using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using UnityEngine;

public class RestartOnMouseClick : MonoBehaviour {

    void OnMouseUp()
    {
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonUp(0))
        {
            SceneManager.LoadScene(0);
        }
    }
}
