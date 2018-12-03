using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightShip : MonoBehaviour {
    
    
    bool highlight = false;
    Material mat;
	// Use this for initialization
	public void HighLight () 
    {
        highlight = true;
        StartCoroutine("blink");
    }

    IEnumerator blink()
    {
        while(highlight)
        {
            yield return new WaitForSeconds(0.5f);
            mat.SetFloat("_Outline", 0.05f);
            yield return new WaitForSeconds(0.5f);
            mat.SetFloat("_Outline", 0.00f);
        }
    }

    public void StopHighlighting()
    {
        highlight = false;
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
