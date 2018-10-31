using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomBox : MonoBehaviour {

    private bool isPlaying = false;

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
            isPlaying = true;
            Debug.Log(GetComponent<AudioSource>().isPlaying);
        }  
        else
        {
            GetComponent<AudioSource>().Stop();
            isPlaying = false;
        }
            
    }
}
