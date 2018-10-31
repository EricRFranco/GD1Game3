using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    private float speed;
    private bool rotating = false;

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

        if (Input.GetKeyDown(KeyCode.A) && !rotating)
        {
            StartCoroutine(Rotate(Vector3.up, -90f));
            rotating = true;
        }
        else if (Input.GetKeyDown(KeyCode.D) && !rotating)
        {
            StartCoroutine(Rotate(Vector3.up, 90f));
            rotating = true;
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
        rotating = false;
    }
}
