using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightShip : MonoBehaviour {
    
    
    bool highlight = false;
    Material mat;
	// Use this for initialization
	public void HighLight () 
    {
        mat.SetFloat("_Outline", 0.03f);
    }


    public void StopHighlighting()
    {
        mat.SetFloat("_Outline", 0);
    }
    void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
    }
    // Update is called once per frame
    void Update () 
    {
    }
}
