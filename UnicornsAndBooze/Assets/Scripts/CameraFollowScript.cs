using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour {
	GameObject player;
	public Vector3 distanceFromPlayer;
	Vector3 goalPoint;
	public float easing;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (player == null) {
			player = GameObject.FindGameObjectWithTag ("Player");
			goalPoint = player.transform.position + distanceFromPlayer;
		} else {
			
			goalPoint = player.transform.position + distanceFromPlayer;
			transform.position = Vector3.Lerp (transform.position, goalPoint, easing);

		}
	}
}
