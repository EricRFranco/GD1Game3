using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    private float speed;
    [SerializeField]
    private GameObject shirt;
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private Vector3 spawnOffset;
    [SerializeField]
    private float angularSpeed;
    //private bool rotating = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.W))
        {
            transform.position += (transform.forward * speed);
        }
        else if(Input.GetKey(KeyCode.S))
        {
            transform.position -= (transform.forward * speed);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0f, -angularSpeed, 0f, Space.World);  
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0f, angularSpeed, 0f, Space.World);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(shirt, spawnPoint.position, Quaternion.identity);
        }
	}

    //Coroutine out of date due to change in design
    /*
    IEnumerator Rotate(Vector3 axis, float angle, float duration = 1.0f)
    {
        Quaternion from = transform.rotation;
        Quaternion to = transform.rotation;
        to *= Quaternion.Euler(axis * angle);

        float elapsed = 0.0f;
        while(elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = to;
        //rotating = false;
    }
    */
}
