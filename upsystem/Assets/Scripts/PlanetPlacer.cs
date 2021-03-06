﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetPlacer : MonoBehaviour {
    public float boundX = 13.1f;
    public float boundY = 5.1F;
    public Transform planet;
    int prvIndex = -1;
    public List<Sprite> planetSprites = new List<Sprite>();
    
    void PlacePlanet()
    {
        planet.transform.localPosition = new Vector3(Random.Range(-boundX, boundX), Random.Range(-boundY, boundY), 0);
        int index = -1;
        bool foundIndex = false;
        while(!foundIndex)
        {
            index = Random.Range(0, planetSprites.Count);
            if(index != prvIndex)
            {
                prvIndex = index;
                foundIndex = true;
                SpriteRenderer renderer = planet.gameObject.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
                renderer.sprite = planetSprites[index];
            }
        }
    }

	// Use this for initialization
	void Start () {
        PlacePlanet();
        GameStateManager.Instance.Jumped += PlacePlanet;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
