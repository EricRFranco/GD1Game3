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

        if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(Rotate(Vector3.up, -90f));
            
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            StartCoroutine(Rotate(Vector3.up, 90f));
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(shirt, spawnPoint.position, Quaternion.identity);
        }
	}

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
}
