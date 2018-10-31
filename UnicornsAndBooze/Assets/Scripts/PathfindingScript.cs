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

	GameObject visionCone;
	public Material visionMaterial;
	public float rayCastRange;
	public float rayCastTotalAngle;
	public int amountOfRayCasts;
	// Use this for initialization
	void Start () {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManagerScript>();
		rbody = GetComponent<Rigidbody> ();
		visionCone = new GameObject("vision", typeof(MeshFilter), typeof(MeshRenderer));
		visionCone.transform.position = Vector3.zero;

		visionCone.GetComponent<MeshRenderer> ().material = visionMaterial;
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
			float angle = Mathf.Atan2 (rbody.velocity.x, rbody.velocity.z);
			transform.eulerAngles = new Vector3 (0, Mathf.Rad2Deg * angle, 0);

		} else {
			rbody.velocity = Vector3.zero;
			rbody.angularVelocity = Vector3.zero;
		}
		UpdateVisionCone ();
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

	void UpdateVisionCone(){
		
		float currentAngle = transform.eulerAngles.y;
		float minAngle = currentAngle - rayCastTotalAngle / 2f;

		Vector3[] vertices = new Vector3[amountOfRayCasts + 2];
		vertices [0] = transform.position;
		Vector2[] uvs = new Vector2[amountOfRayCasts + 2];
		uvs [0] = Vector2.zero;
		int[] triangles = new int[(amountOfRayCasts) * 3];
		for (int i = 0; i <= amountOfRayCasts; ++i) {
			
			float angle = Mathf.Deg2Rad * (minAngle + i * rayCastTotalAngle / (float)(amountOfRayCasts));
			Vector3 dir = new Vector3 (Mathf.Sin(angle), 0f, Mathf.Cos(angle));
			RaycastHit raycastHitData; 
			bool hit = Physics.Raycast (transform.position, dir, out raycastHitData, rayCastRange);

			Debug.DrawRay (transform.position, (hit ? raycastHitData.distance : rayCastRange) * dir);

			vertices [i + 1] = transform.position + (hit ? raycastHitData.distance : rayCastRange) * dir;
			uvs [i + 1] = Vector2.zero;
			if (i != amountOfRayCasts) {
				triangles [i * 3] = 0;
				triangles [i * 3 + 1] = i + 1;
				triangles [i * 3 + 2] = i + 2;
			}
		}

		Mesh mesh = new Mesh ();
		mesh.vertices = vertices;
		mesh.uv = uvs;
		mesh.triangles = triangles;

		visionCone.GetComponent<MeshFilter> ().mesh = mesh;


	}

}
