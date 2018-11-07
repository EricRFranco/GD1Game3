using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomBox : MonoBehaviour {

    private bool isPlaying = false;
    [SerializeField]
    private AudioSource chillTrack;
    public AudioClip button;

    public bool IsPlaying
    {
        get { return isPlaying; }
        set { isPlaying = value; }
    }

	void Awake(){
		chillTrack = GameObject.FindGameObjectWithTag ("ChillPlayer").GetComponent<AudioSource> ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.F)) {
			ToggleBoombox ();

		}
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
