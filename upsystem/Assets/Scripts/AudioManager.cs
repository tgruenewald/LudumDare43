using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioClips
{
    Music = 0,
    BearGrowl,
    Click,
    Jump,
    RedAlert,
    Repair,
    Sacrifice,
    Scout,
    Transfer,
    ShipClick,
    EndTurn,
    TransferInit,
    
};

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance;

    List<AudioSource> _sources = new List<AudioSource>();

    void Awake()
    {
        Instance = this;
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            _sources.Add(this.transform.GetChild(i).gameObject.GetComponent<AudioSource>());
            Debug.Log(_sources[i].name);
        }
    }
    // Use this for initialization
    void Start()
    {

    }

    public void PlaySound(AudioClips clip)
    {
        if((int) clip < this._sources.Count)
            this._sources[(int)clip].Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
};
