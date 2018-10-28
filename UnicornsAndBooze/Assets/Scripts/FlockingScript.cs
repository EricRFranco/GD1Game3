using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingScript : MonoBehaviour {

	GameObject[] boidList;
	Rigidbody rbody;

	public float repulsionConstant;
	public float repulseCenterConstant;
	public float seekingTargetConstant;
	public float maxAcceleration;
	public float maxVelocity;

	public float slowRadius;
	public float timeToTarget;

	public Vector3 target;

	public float closeEnoughDistance;

	// Use this for initialization
	void Start () {
		rbody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateBoidList ();
		Vector3 acceleration = GetFlockAcceleration ();
		if (acceleration.magnitude > maxAcceleration) {
			acceleration = acceleration.normalized * maxAcceleration;
		}
		rbody.velocity += acceleration;
		if (rbody.velocity.magnitude > maxVelocity) {
			rbody.velocity = rbody.velocity.normalized * maxVelocity;
		}
	}

	void UpdateBoidList(){
		boidList = GameObject.FindGameObjectsWithTag ("Boid");

	}

	Vector3 GetFlockAcceleration() {
		Vector3 strength1 = AvoidCollisions ();
		Vector3 strength2 = MatchVelocity ();
		Vector3 strength3 = seekingTargetConstant * FlockToTarget ();
		Vector3 strength4 = Mathf.Max(repulseCenterConstant/Vector3.Distance(transform.position, target), maxAcceleration) * DynamicEvade (transform.position, target);
		return strength1 + strength2 + strength3 + strength4;
	}

	Vector3 DynamicSeek(Vector3 position, Vector3 target){
		Vector3 linearAcceleration = target - position;
		return linearAcceleration;
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

	Vector3 AvoidCollisions(){
		Vector3 seperation = Vector3.zero;
		int numClose = 0;

		foreach (GameObject boid in boidList) {
			float distance = Vector3.Distance (transform.position, boid.transform.position);
			if (boid != gameObject) {
				FlockingScript other = boid.GetComponent<FlockingScript> ();
				float strength = Mathf.Max (other.repulsionConstant / distance, maxAcceleration);
				print (strength);
				seperation += strength * (transform.position - boid.transform.position).normalized;
				numClose++;
			}
		}

		print (seperation);
			
		if (numClose > 0) {
			seperation *= 1 / (float)numClose; 
		}

		print (seperation);

		return seperation;
	}

	Vector3 MatchVelocity() {
		Vector3 averageVelocity = Vector3.zero;
		foreach (GameObject boid in boidList) {
			averageVelocity += boid.GetComponent<Rigidbody> ().velocity;
		}
		averageVelocity *= 1 / (float)boidList.Length;
		return averageVelocity;
	}

	Vector3 FlockToTarget() {
		return DynamicSeek (transform.position, target) + DynamicArrive (transform.position, target, rbody.velocity);
	}
}
