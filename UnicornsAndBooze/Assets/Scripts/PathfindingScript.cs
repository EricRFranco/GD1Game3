using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum State{
	ONSETPATH,
	DELIVERINGSHIRT,
	RETURNINGTOPATH,
	GOINGTODANCEFLOOR,
	DANCING,
	PICKINGUPSHIRT,
	PLACINGSHIRT,
	GOINGTOSHIRT,
}

public class PathfindingScript : MonoBehaviour {
	
	LevelManagerScript levelManager;

	List<Vector3> originalPath;
    List<Vector3> currentPath;

	public float closeEnoughToPointDistance;
	public float closeEnoughToBoomBoxDistance;
	public float maxAcceleration;
	public float maxVelocity;

	public float slowRadius;
	public float timeToTarget;

	public float avoidingOtherAgentsDistance;
	public float repulsionConstant;

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

	SceneChanger sceneChanger;


	GameObject boombox;
	GameObject targetShirt;

	GameObject[] otherPudgys;
    Animator anim;

	float animationStartTime;

    public AudioSource audio;
    public AudioClip shirtup;
    public AudioClip shirtdown;

	// Use this for initialization
	void Awake () {
		sceneChanger = GameObject.Find("SceneManager").GetComponent<SceneChanger>();
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManagerScript>();
		rbody = GetComponent<Rigidbody> ();
		visionCone = new GameObject("vision", typeof(MeshFilter), typeof(MeshRenderer));
		visionCone.transform.position = Vector3.zero;
		visionCone.GetComponent<MeshRenderer> ().material = visionMaterial;
		currentState = State.ONSETPATH;
		
        anim = this.gameObject.GetComponent<Animator>();
		anim.Play ("Walking");
		anim.SetBool("walking", true);
        anim.SetBool("walking", false);
		audio = GetComponent<AudioSource>();
	}


	void Start() {
        boombox = GameObject.FindGameObjectWithTag("Boombox");
        otherPudgys = GameObject.FindGameObjectsWithTag ("Enemy");

	}
	
	// Update is called once per frame
	void Update () {
		if (currentPath == null) {
			currentIndexOnPath = 0;
			currentPath = originalPath;
          
		}

		if (!(currentState == State.DANCING || currentState == State.PICKINGUPSHIRT || currentState == State.PLACINGSHIRT)) {
			if (boombox.GetComponent<BoomBox> ().IsPlaying && currentState != State.GOINGTODANCEFLOOR && currentState != State.DANCING) {
                anim.SetBool("walking", true);
				currentState = State.GOINGTODANCEFLOOR;
				Vector3 targetPoint = boombox.transform.parent.position;
				targetPoint.y = transform.position.y;
				currentPath = levelManager.PathFind (transform.position, targetPoint);
				currentIndexOnPath = 0;
				Destroy (visionCone);
			}
            
			if ((Vector3.Distance (transform.position, currentPath [currentPath.Count - 1]) > closeEnoughToPointDistance && currentState != State.GOINGTODANCEFLOOR)
				|| (Vector3.Distance (transform.position, currentPath [currentPath.Count - 1]) > closeEnoughToBoomBoxDistance && currentState == State.GOINGTODANCEFLOOR)) {
				if (Vector3.Distance (transform.position, currentPath [currentIndexOnPath]) <= closeEnoughToPointDistance) {
					currentIndexOnPath++;

				}
				Vector3 acceleration = FollowPath (transform.position, currentPath, currentIndexOnPath, rbody.velocity) + AvoidCollisions();
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
				case(State.GOINGTODANCEFLOOR):
					currentState = State.DANCING;
                    anim.SetBool("dancing", true);
					break;
				case(State.GOINGTOSHIRT):
					break;
				default:
					rbody.velocity = Vector2.zero;
					break;
				}
			}
			if (currentState != State.GOINGTODANCEFLOOR && currentState != State.DANCING) {
				
				UpdateVisionCone ();
			}
			anim.speed = rbody.velocity.magnitude / maxVelocity;
		} else {
			rbody.velocity = Vector2.zero;
			if (currentState == State.PICKINGUPSHIRT || currentState == State.PLACINGSHIRT) {
				UpdateVisionCone ();
			}
			if (currentState == State.PICKINGUPSHIRT && animationStartTime + 1.208f < Time.time) {
				currentState = State.DELIVERINGSHIRT;
			}
			if (currentState == State.PLACINGSHIRT && animationStartTime + 0.292f < Time.time) {
				currentState = State.RETURNINGTOPATH;
			}
				
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
				
				sceneChanger.GameOver ();
				//print ("hit");
			} else if (hit && raycastHitData.collider.tag == "Shirt" && currentState != State.GOINGTOSHIRT && currentState != State.DELIVERINGSHIRT && currentState != State.PICKINGUPSHIRT
				&& currentState != State.PLACINGSHIRT) {
				currentState = State.GOINGTOSHIRT;
				currentIndexOnPath = 0;
				Vector3 target = raycastHitData.collider.transform.position;
				currentPath = levelManager.PathFind (transform.position, new Vector3 (target.x, transform.position.y, target.z));

				//targetShirt = raycastHitData.collider.gameObject;
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


	Vector3 AvoidCollisions(){
		Vector3 seperation = Vector3.zero;
		int numClose = 0;

		foreach (GameObject pudgy in otherPudgys) {
			float distance = Vector3.Distance (transform.position, pudgy.transform.position);
			if (pudgy != gameObject && distance < avoidingOtherAgentsDistance) {
				float strength = Mathf.Max (repulsionConstant / distance, maxAcceleration);

				seperation += strength * (transform.position - pudgy.transform.position).normalized;
				numClose++;
			}
		}

		if (numClose > 0) {
			seperation *= 1 / (float)numClose; 
		}
		return seperation;
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

		case State.GOINGTOSHIRT:
		case State.ONSETPATH:
		case State.RETURNINGTOPATH:
			if (collider.tag == "Shirt") {
                audio.clip = shirtup;
                audio.Play();
				Destroy (collider.transform.parent.gameObject);
				currentState = State.PICKINGUPSHIRT;
				animationStartTime = Time.time;
                anim.SetBool("dropping", false);
                anim.SetBool("carrying", true);
				collider.gameObject.layer = 2;
				GameObject shirtBinTarget = FindClosestShirtBin();
				currentPath = levelManager.PathFind (transform.position, new Vector3(shirtBinTarget.transform.position.x, transform.position.y, shirtBinTarget.transform.position.z));
				currentIndexOnPath = 0;
			

			}
				break;
		case State.DELIVERINGSHIRT:
			if (collider.tag == "ShirtBin") {
                audio.clip = shirtdown;
                audio.Play();
                anim.SetBool("dropping", true);
                anim.SetBool("carrying", false);
				currentState = State.PLACINGSHIRT;
				animationStartTime = Time.time;
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