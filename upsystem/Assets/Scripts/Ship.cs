using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {
    private ActionSelector mActionSelector;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            if(this.gameObject.transform.GetChild(i).CompareTag("ActionSelectors"))
            {
                mActionSelector = this.gameObject
                                      .transform
                                      .GetChild(i)
                                      .gameObject
                                      .GetComponent<ActionSelector>();
                break;
            }
        }

    }

    void OnMouseUp()
    {
        mActionSelector.Toggle();
    }

    // Update is called once per frame
    void Update()
    {}
}
