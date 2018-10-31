using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingScript : MonoBehaviour {

	LevelManagerScript levelManager;

    List<Vector3> currentPath;

	public Vector3 goalPoint;

	public float closeEnoughToPointDistance;

	public float maxAcceleration;
	public float maxVelocity;

	public float slowRadius;
	public float timeToTarget;

	int currentIndexOnPath;
	Rigidbody rbody;

	// Use this for initialization
	void Start () {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManagerScript>();
		rbody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (currentPath == null) {
			currentPath = levelManager.PathFind (transform.position, goalPoint);
			currentIndexOnPath = 0;
		}

		if (Vector3.Distance (transform.position, goalPoint) > closeEnoughToPointDistance) {
			if (Vector3.Distance (transform.position, currentPath [currentIndexOnPath]) <= closeEnoughToPointDistance) {
				currentIndexOnPath++;
				//print (currentPath [currentIndexOnPath]);
			}
			Vector3 acceleration = FollowPath (transform.position, currentPath, currentIndexOnPath, rbody.velocity);
			if (acceleration.magnitude > maxAcceleration) {
				acceleration = acceleration.normalized * maxAcceleration;
			}
			rbody.velocity += acceleration;
			if (rbody.velocity.magnitude > maxVelocity) {
				rbody.velocity = rbody.velocity.normalized * maxVelocity;
			}
		} else {
			rbody.velocity = Vector3.zero;
			rbody.angularVelocity = Vector3.zero;
		}
		
	}
	

	Vector3 DynamicSeek(Vector3 position, Vector3 target){
		Vector3 linearAcceleration = target - position;
		return maxAcceleration * linearAcceleration;
	}

	Vector3 DynamicEvade(Vector3 position, Vector3 target) {
		Vector3 linearAcceleration = position - target;
		return linearAcceleration;
	}

	Vector3 DynamicArrive(Vector3 position, Vector3 target, Vector3 currentVelocity) {
		float targetSpeed = maxVelocity * (Vector3.Distance (position, target) / slowRadius);
		Vector3 directionVector = (target - position).normalized;
		Vector3 targetVelocity = directionVector * targetSpeed;
		Vector3 acceleration = (targetVelocity - currentVelocity) / timeToTarget;
		return acceleration;
	}

	Vector3 FollowPath(Vector3 position, List<Vector3> path, int currentIndex, Vector3 currentVelocity){
		Vector3 linearAcceleration = DynamicSeek(position, path[currentIndex]);
		if (currentIndex == path.Count - 1) {
			linearAcceleration += DynamicArrive (position, path [currentIndexOnPath], currentVelocity);
		}
		return linearAcceleration;
	}

}
