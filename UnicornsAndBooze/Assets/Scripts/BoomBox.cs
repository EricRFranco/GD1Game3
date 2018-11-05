using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomBox : MonoBehaviour {

    private bool isPlaying = false;
    [SerializeField]
    private AudioSource chillTrack;

    public bool IsPlaying
    {
        get { return isPlaying; }
        set { isPlaying = value; }
    }
	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ToggleBoombox()
    {
        if (!isPlaying)
        {
            GetComponent<AudioSource>().Play();
            chillTrack.Stop();
            isPlaying = true;
        }  
        else
        {
            GetComponent<AudioSource>().Stop();
            chillTrack.Play();
            isPlaying = false;
        }
            
    }
}
