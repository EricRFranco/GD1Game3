using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {

    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private Vector3 spawnOffset;
    [SerializeField]
    private Vector3 impulse;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)) {
            Vector3 spawnPoint = transform.position + spawnOffset;
            GameObject projectile = (GameObject)Instantiate(projectilePrefab, spawnPoint, transform.rotation);
            projectile.GetComponent<Rigidbody>().AddForce(impulse, ForceMode.Impulse);
        }
	}
}
