using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine("DespawnProjectile");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }

    //For debugging, projectiles despawn if they don't collide with anything for too long
    //Just to make sure the scene doesn't get bogged down by too many projectiles
    IEnumerator DespawnProjectile()
    {
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }
}
