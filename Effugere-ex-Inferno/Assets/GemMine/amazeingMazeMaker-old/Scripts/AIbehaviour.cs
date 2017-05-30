using UnityEngine;
using System.Collections;

// this script realizes the AI behaviour
//
// each AI patrols the maze until it reaches a junction. Here it decides
// which way to go (randomly)

public class AIbehaviour : MonoBehaviour {

	// the ray is used to check for junctions
	// four rays are casted in each direction
	// if the ray hits nothing, this is potentially 
	// a route to go

	Ray ray;
	RaycastHit hit;
	int RayCount = 9;
	// since the maze size is integer,
	// we check each int for a junction
	public float deltaMove = 1f;

	// size of a cell.
	// has to be customized individually
	public float mazeCellSize = 16f;
	// speed of the AI
	public float moveSpeed = 0.01f;
	// reziprocal speed
	float moveSpeedRezi;

	// the directions the AI can potentially go
	bool left, right, forward, backward;
	float dstRight, dstLeft, dstForward, dstBackward;

	// which direction are heading to at the moment
	Vector3 direction;

	// possible moves
	string possibleMoves;
	// a the chosen move from the possible directions
	string chosenDirection;

	// not needed at the moment. 
	// I set a orc-Model here. But since I am not allowed to sell
	// it, this slot stays free.
	// public Transform orc;
	// the model has to turn around a corner. If it's not Robocop,
	// the turn should be smooth
	float smooth = 3;



	// Use this for initialization
	void Start () {
		moveSpeedRezi = 1f / moveSpeed;
	}



	// Update is called once per frame
	void Update () {
		// if AI is reaching a new tile, 
		// it has to compute which way to go
		if (transform.position.x % mazeCellSize == 0 && transform.position.z % mazeCellSize == 0) {
			evaluateDirection();
		}

		// some calculation to move to the new direction
		Vector3 position = transform.position;
		position += direction;
		position *= moveSpeedRezi;
		position = new Vector3 (Mathf.Round (position.x), Mathf.Round (position.y), Mathf.Round (position.z));
		position /= moveSpeedRezi;
		transform.position = position;     
		// and slowly turn the GameObject around the corner
		//orc.transform.rotation = Quaternion.Lerp(orc.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * smooth);
	}


	//
	// void evaluateDirection
	//
	// This method chooses a random direction for the AI to go
	//

	void evaluateDirection() {
		string possibleMoves = buildDirections ();

		// we have run into an dead end, so turn around
		if (possibleMoves.Length == 1) {
			chosenDirection = possibleMoves;
		}

		else {
			if (chosenDirection != "") {
				// are we moving north?
				// remove south from the list - we don't want to turn 180°
				if (chosenDirection == "N") {
					possibleMoves = possibleMoves.Replace("S","");
				}
				// same for south
				if (chosenDirection == "S") {
					possibleMoves = possibleMoves.Replace("N","");
				}
				// same for east
				if (chosenDirection == "E") {
					possibleMoves = possibleMoves.Replace("W","");
				}
				// same for west
				if (chosenDirection == "W") {
					possibleMoves = possibleMoves.Replace("E","");
				}
			}
		}

		// can we move (we should in any case)
		if (possibleMoves.Length >= 1)
			chosenDirection = possibleMoves.Substring (Random.Range (0, possibleMoves.Length), 1);
		else
			chosenDirection = "";

		// set the vector for the new direction
		if (chosenDirection == "E")
			direction = new Vector3 (moveSpeed, 0, 0);
		if (chosenDirection == "W")
			direction = new Vector3 (-moveSpeed, 0, 0);
		if (chosenDirection == "N")
			direction = new Vector3 (0, 0, moveSpeed);
		if (chosenDirection == "S")
			direction = new Vector3 (0, 0, -moveSpeed);
	}


	//
	// string buildDirections
	//
	// This method evaluates which directions are open
	//

	public string buildDirections() {
		// no directions
		string directions="";

		// we can go everywhere
		// at least we think so...
		left = true;		
		right = true;
		forward = true;
		backward = true;

		// Ok, now cast some rays to check
		// if we can go right
		for (int i = 0; i<RayCount; i ++) {
			float x = transform.position.x+transform.localScale.x/2;
			float y = transform.position.y;
			float z = transform.position.z-4*transform.localScale.z/10f+i*transform.localScale.z/10f;
			ray = new Ray(new Vector3(x,y,z), Vector3.right);
			if (Physics.Raycast(ray,out hit,Mathf.Abs(deltaMove))) {
				//no, right is not possible
				dstRight = Vector3.Distance (ray.origin, hit.point);
				right = right && false;
			}
		}
		
		// Ok, now cast some rays to check
		// if we can go left
		for (int i = 0; i<RayCount; i ++) {
			float x = transform.position.x-transform.localScale.x/2;
			float y = transform.position.y;
			float z = transform.position.z-4*transform.localScale.z/10f+i*transform.localScale.z/10f;
			ray = new Ray(new Vector3(x,y,z), Vector3.left);
			if (Physics.Raycast(ray,out hit,Mathf.Abs(deltaMove))) {
				//no, left is not possible
				dstLeft = Vector3.Distance (ray.origin, hit.point);
				left = false;
			}
		}
		
		// Ok, now cast some rays to check
		// if we can go ahead
		for (int i = 0; i<RayCount; i ++) {
			float x = transform.position.x-4*transform.localScale.x/10f+i*transform.localScale.x/10f;
			float y = transform.position.y;
			float z = transform.position.z+transform.localScale.z/2;
			ray = new Ray(new Vector3(x,y,z), Vector3.forward);
			if (Physics.Raycast(ray,out hit,Mathf.Abs(deltaMove))) {
				//no, ahead is not possible
				dstForward = Vector3.Distance (ray.origin, hit.point);
				forward = forward && false;
			}
			
		}

		// Ok, now cast some rays to check
		// if we can go back
		for (int i = 0; i<RayCount; i ++) {
			float x = transform.position.x-4*transform.localScale.x/10f+i*transform.localScale.x/10f;
			float y = transform.position.y;
			float z = transform.position.z-transform.localScale.z/2;
			ray = new Ray(new Vector3(x,y,z), Vector3.back);
			//Debug.DrawRay(ray.origin,ray.direction);
			if (Physics.Raycast(ray,out hit,Mathf.Abs(deltaMove))) {
				//no, backwards is not possible
				dstBackward = Vector3.Distance (ray.origin, hit.point);
				backward = false;
			}
		}

		// construct a string for further examination
		if (right)
			directions += "E";
		if (left)
			directions += "W";
		if (forward)
			directions += "N";
		if (backward)
			directions += "S";

		return directions;
	}
}
