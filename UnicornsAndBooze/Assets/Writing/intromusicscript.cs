using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class intromusicscript : MonoBehaviour {

    public AudioSource audio;
    public AudioClip nonloop;
    public AudioClip loop;

	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(audio.isPlaying == false)
        {
            audio.clip = loop;
            audio.loop = true;
            audio.Play();
        }
	}
}
