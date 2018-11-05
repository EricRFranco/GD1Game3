using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum State{
	ONSETPATH,
	DELIVERINGSHIRT,
	RETURNINGTOPATH,
	GOINGTODANCEFLOOR,
	DANCING
}

public class PathfindingScript : MonoBehaviour {

	LevelManagerScript levelManager;

	List<Vector3> originalPath;
    List<Vector3> currentPath;

	public float closeEnoughToPointDistance;

	public float maxAcceleration;
	public float maxVelocity;

	public float slowRadius;
	public float timeToTarget;

	int currentIndexOnPath;
	Rigidbody rbody;
	public float rayCastOffsetHeight;
	GameObject visionCone;
	public Material visionMaterial;
	public float rayCastRange;
	public float rayCastTotalAngle;
	public int amountOfRayCasts;

	State currentState;

	int startingIndexWhenReturningToPath;


	GameObject boombox;

	// Use this for initialization
	void Awake () {
		
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManagerScript>();
		rbody = GetComponent<Rigidbody> ();
		visionCone = new GameObject("vision", typeof(MeshFilter), typeof(MeshRenderer));
		visionCone.transform.position = Vector3.zero;
		visionCone.GetComponent<MeshRenderer> ().material = visionMaterial;
		currentState = State.ONSETPATH;
		boombox = GameObject.FindGameObjectWithTag ("Boombox");
	}
	
	// Update is called once per frame
	void Update () {
		
		if (currentPath == null) {
			//currentPath = levelManager.PathFind (transform.position, goalPoint);
			//currentIndexOnPath = 0;
			currentPath = originalPath;
		}
		//print (currentIndexOnPath + " " + currentPath[0]);
		if (Vector3.Distance (transform.position, currentPath[currentPath.Count - 1]) > closeEnoughToPointDistance) {
			if (Vector3.Distance (transform.position, currentPath [currentIndexOnPath]) <= closeEnoughToPointDistance) {
				currentIndexOnPath++;

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
			/*rbody.velocity = Vector3.zero;
			rbody.angularVelocity = Vector3.zero;*/
			switch (currentState) {
			case(State.ONSETPATH):
				currentPath.Reverse ();
				currentIndexOnPath = 0;
				break;
			case(State.RETURNINGTOPATH):
				currentIndexOnPath = startingIndexWhenReturningToPath;
				currentPath = originalPath;
				currentState = State.ONSETPATH;
				break;
			default:
				break;
			}



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
		Vector3 rayCastStart = transform.position;
		rayCastStart.y -= rayCastOffsetHeight;
		Vector3[] vertices = new Vector3[amountOfRayCasts + 2];
		vertices [0] = rayCastStart;
		Vector2[] uvs = new Vector2[amountOfRayCasts + 2];
		uvs [0] = Vector2.zero;
		int[] triangles = new int[(amountOfRayCasts) * 3];
		for (int i = 0; i <= amountOfRayCasts; ++i) {
			
			float angle = Mathf.Deg2Rad * (minAngle + i * rayCastTotalAngle / (float)(amountOfRayCasts));
			Vector3 dir = new Vector3 (Mathf.Sin(angle), 0f, Mathf.Cos(angle));
			RaycastHit raycastHitData; 
			bool hit = Physics.Raycast (rayCastStart, dir, out raycastHitData, rayCastRange);

			Debug.DrawRay (rayCastStart, (hit ? raycastHitData.distance : rayCastRange) * dir);
			if (hit && raycastHitData.collider.tag == "Player") {
				//print ("Game Over");
			}
			vertices [i + 1] = rayCastStart + (hit ? raycastHitData.distance : rayCastRange) * dir;
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


    public void setOriginalPath(List<Tile> tilePath) {
        List<Vector3> path = new List<Vector3>(tilePath.Count);
        foreach(Tile tile in tilePath) {
			path.Add (tile.position);
        }
		originalPath = path;
    }


	public void setOriginalPath(Path path) {
		List<Vector3> newPath = new List<Vector3>(path.pathNodes.Count);
		foreach(PathNode node in path.pathNodes) {
			
			newPath.Add (levelManager.levelGrid[node.tileY][node.tileX].position);
		}
		originalPath = newPath;
	}

	public void setOriginalPath(List<Vector3> path){
		originalPath = path;
	}

	void OnTriggerEnter(Collider collider){
		
		switch (currentState) {

		case State.ONSETPATH:
		case State.RETURNINGTOPATH:
			if (collider.tag == "Shirt") {
				Destroy (collider.gameObject);
				currentState = State.DELIVERINGSHIRT;
				GameObject shirtBinTarget = FindClosestShirtBin();
				currentPath = levelManager.PathFind (transform.position, shirtBinTarget.transform.position);
				currentIndexOnPath = 0;
			}
				break;
		case State.DELIVERINGSHIRT:
			if (collider.tag == "ShirtBin") {
				currentState = State.RETURNINGTOPATH;
				Vector3 returnPoint = FindClosestPointOnOriginalPath ();
				currentPath = levelManager.PathFind (transform.position, returnPoint);
				currentIndexOnPath = 0;
			}
			break;
		default:
			break;

		}

	}

	GameObject FindClosestShirtBin(){
		GameObject[] shirtBins = GameObject.FindGameObjectsWithTag ("ShirtBin");
		if (shirtBins.Length > 0) {
			GameObject shirtBin = shirtBins [0];
			float shortestDistance = Vector3.Distance (transform.position, shirtBin.transform.position);
			for (int i = 1; i < shirtBins.Length; ++i) {
				float distance = Vector3.Distance (transform.position, shirtBins [i].transform.position);
				if (distance < shortestDistance) {
					shortestDistance = distance;
					shirtBin = shirtBins [i];
				}
			}
			return shirtBin;
		}
		return null;
	}

	Vector3 FindClosestPointOnOriginalPath(){
		if (originalPath.Count > 0) {
			Vector3 closestPoint = originalPath [0];
			float shortestDistance = Vector3.Distance (transform.position, closestPoint);
			startingIndexWhenReturningToPath = 0;
			for (int i = 1; i < originalPath.Count; ++i) {
				float distance = Vector3.Distance (transform.position, originalPath[i]);
				if (distance < shortestDistance) {
					shortestDistance = distance;
					startingIndexWhenReturningToPath = i;
					closestPoint = originalPath [i];
				}
			}
			return closestPoint;
		}
		return Vector3.zero;
	}

}
