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
    private float angularSpeed;

    private Rigidbody rb;
    private bool shirtAvailable = false;
    private int shirtCount = 0;
    //private bool rotating = false;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        MovePlayer();
        GainShirt();
        if(Input.GetKeyDown(KeyCode.Space) && shirtCount > 0)
        {
            Instantiate(shirt, spawnPoint.position, Quaternion.identity);
            shirtCount--;
        }
	}

    void MovePlayer()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.velocity = transform.forward * speed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rb.velocity = transform.forward * -speed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0f, -angularSpeed, 0f, Space.World);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0f, angularSpeed, 0f, Space.World);
        }
    }

    void GainShirt()
    {
        if(shirtAvailable && Input.GetKeyDown(KeyCode.E))
        {
            shirtCount++;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        shirtAvailable = true;
    }

    private void OnTriggerExit(Collider other)
    {
        shirtAvailable = false;
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
