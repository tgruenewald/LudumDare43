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
    Transfer
};

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance;

    List<AudioSource> _sources = new List<AudioSource>();

    void Awake()
    {
        Instance = this;
    }
    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            _sources.Add(this.transform.GetChild(i).gameObject.GetComponent<AudioSource>());
            Debug.Log(_sources[i].name);
        }
    }

    public void PlaySound(AudioClips clip)
    {
        this._sources[(int)clip].Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
};
