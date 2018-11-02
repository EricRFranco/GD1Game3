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
    private Animator anim;
    private bool shirtAvailable = false;
    private int shirtCount = 0;

    private BoomBox boomBox;
    //private bool rotating = false;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        MovePlayer();
        GainShirt();
        ToggleBoombox();
        if(Input.GetKeyDown(KeyCode.Space) && shirtCount > 0)
        {
            anim.SetTrigger("placeShirt");
            //Instantiate(shirt, spawnPoint.position, Quaternion.identity);
            shirtCount--;
        }
        anim.SetFloat("velocity", rb.velocity.z);
        anim.SetInteger("shirts", shirtCount);
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
            anim.SetBool("turningLeft", true);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0f, angularSpeed, 0f, Space.World);
            anim.SetBool("turningRight", true);
        }

        if (Input.GetKeyUp(KeyCode.A))
            anim.SetBool("turningLeft", false);

        if (Input.GetKeyUp(KeyCode.D))
            anim.SetBool("turningRight", false);
    }

    void GainShirt()
    {
        if(shirtAvailable && Input.GetKeyDown(KeyCode.E))
        {
            shirtCount++;
        }
    }

    void ToggleBoombox()
    {
        if(boomBox != null && Input.GetKeyDown(KeyCode.E))
        {
            boomBox.ToggleBoombox();
            anim.SetTrigger("interact");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ShirtBin") 
            shirtAvailable = true;

        if (other.gameObject.tag == "Boombox")
        { 
            boomBox = other.gameObject.GetComponent<BoomBox>();
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "ShirtBin")
            shirtAvailable = false;

        if (other.gameObject.tag == "Boombox")
        {
            boomBox = null;
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
