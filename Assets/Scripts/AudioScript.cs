using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    public AudioClip MusicClip;
    public AudioSource MusicSource;

    // Start is called before the first frame update
    void Start()
    {
        MusicSource.clip = MusicClip;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // te funkcje trzeba wywolac w Gridzie tam gdzie sie Matchuja przedmioty, mozna tez to wykorzystac przy spadaniu paska zycia
    void PlaySound()
    {
        MusicSource.Play();
    }
}
