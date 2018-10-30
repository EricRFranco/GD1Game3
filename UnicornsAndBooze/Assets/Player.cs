using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    private float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(transform.position);
		if(Input.GetKey(KeyCode.W))
        {
            transform.position += (transform.forward * speed);
        }
        else if(Input.GetKey(KeyCode.S))
        {
            transform.position -= (transform.forward * speed);
        }
	}
}
