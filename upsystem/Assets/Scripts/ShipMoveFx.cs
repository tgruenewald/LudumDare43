using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ShipMoveFx : MonoBehaviour {
    public float speedMin = 1.01f;
    public float speedMax = 2f;
	
	void Start () 
    {
       Material mat = GetComponent<SpriteRenderer>().material;
       mat.SetFloat("_Speed", Random.Range(speedMin, speedMax));
    }
}
