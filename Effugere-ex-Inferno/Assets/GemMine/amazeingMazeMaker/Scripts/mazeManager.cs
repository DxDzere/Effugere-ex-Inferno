using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mazeManager : MonoBehaviour {

	// maze dimensions
	public int mazeWidth = 10;
	public int mazeLength = 10;
	public float mazeHeight = 1f;

	protected int mWidth;
	protected int mHeight;

	// random seed
	public int seed = 12345;

	// thickness of wall
	public float wallWidth = 1.0f;
	// tilesize
	public float tileSize = 1.0f;

	// data model
	protected int[,] mazeGrid;

	// prefab, may be changed
	public GameObject mazeElement;
	public GameObject pathElement;
	public GameObject floorElement;

	public bool makeEntrance = false;
	public bool makeExit = false;

	List<Vector2> solutionPath = new List<Vector2> ();

	BaseGenerator generator;
	Vector2 mazeEntrance;
	Vector2 mazeExit;

	public GameObject cell00;
	public GameObject cell01;
	public GameObject cell02;
	public GameObject cell03;
	public GameObject cell04;
	public GameObject cell05;
	public GameObject cell06;
	public GameObject cell07;
	public GameObject cell08;
	public GameObject cell09;
	public GameObject cell10;
	public GameObject cell11;
	public GameObject cell12;
	public GameObject cell13;
	public GameObject cell14;
	public GameObject cell15;


	//
	// public void createMaze()
	// 
	// take the values from the editor and create the maze
	//

	public void createMaze(int algorithm, bool standardConfig) {
		// calculate the final maze size by taking walls into account
		mWidth = mazeWidth * 2 + 1;
		mHeight = mazeLength * 2 + 1;
		// set the generation algorithm
		switch (algorithm) {
			case 0: 
				generator = new binaryTree ();
				break;
			case 1: 
				generator = new recursiveBacktracker();
				break;
			case 2: 
				generator = new recursiveDivisionMaze();
				break;
			case 3: 
				generator = new sideWinder ();
				break;
			default:
				generator = new binaryTree ();
				break;
		}

		generator.setParameters (mazeWidth, mazeLength, mazeHeight, seed, wallWidth, tileSize, makeEntrance, makeExit);
		mazeGrid = generator.createMaze ();
		// show the maze
		deleteFloor();
		if (standardConfig) {
			addFloor ();
			displayMazeCubes ();
		} else {
			displayMazePrefabs ();
		}
		// remove old solution, if existent
		removeSolution ();
	}



	//
	// void removeSolution()
	//
	// Clean up the solution path
	//

	void removeSolution() {
		solutionPath.Clear();
		GameObject[] path = GameObject.FindGameObjectsWithTag ("solutionPath");
		foreach (GameObject go in path) {
			DestroyImmediate (go);
		}
	}



	//
	// public void solveMaze() 
	//
	// this solves the maze and draw the
	// solution path inside the maze
	//

		public void solveMaze(bool standardConfig) {
		if (generator != null) {
			removeSolution ();
			mazeEntrance = generator.getEntrance ();
			mazeExit = generator.getExit ();
			solveMazeIterative((int)mazeEntrance.x, (int)mazeEntrance.y);
			if (standardConfig)
				drawSolutionCubes ();
			else 
				drawSolutionPrefabs ();
		}
	}



	//
	// public void displayMaze() 
	//
	// show the maze on the screen
	//

	public void displayMazeCubes() {
		// clean up old maze
		GameObject[] mazeElements = GameObject.FindGameObjectsWithTag ("mazeElement");
		foreach (GameObject me in mazeElements) {
				DestroyImmediate (me);
		}

		// set the maze tile dimension
		Vector3 wallThickness = new Vector3 (tileSize, mazeHeight, tileSize);

		// iterate through maze datamodel
		for (int y = 0; y < mHeight; y++) {
			for (int x = 0; x < mWidth; x++) {
				// if we have wall
				if (mazeGrid [x, y] == 1) {
					GameObject me = Instantiate (mazeElement, new Vector3 (x, 0, y), Quaternion.identity) as GameObject;
					if (x % 2 == 0)
						wallThickness.x = wallWidth;
					else
						wallThickness.x = tileSize;

					if (y % 2 == 0)
						wallThickness.z = wallWidth;
					else
						wallThickness.z = tileSize;

					me.transform.localScale = wallThickness;

					me.transform.position = new Vector3 (x * (tileSize + wallWidth) / 2f, 0, y* (tileSize + wallWidth)/2f);
					me.name = "mazeElement-"+x+"-"+y;
				}
			}
		}
	}


	public void displayMazePrefabs() {
		// clean up old maze
		GameObject[] mazeElements = GameObject.FindGameObjectsWithTag ("mazeElement");
		foreach (GameObject me in mazeElements) {
				DestroyImmediate (me);
		}

		// 1 = N
		// 2 = E
		// 4 = S
		// 8 = W
		GameObject go = null;
		int walls = 0;
		int ypos = 0;
		int xpos = 0;
		for (int y = 1; y < mHeight - 1; y+=2) {
			ypos++;
			xpos = 0;
			for (int x = 1; x < mWidth - 1; x+=2) {
				xpos++;
				walls = 0;
				if (mazeGrid [x, y + 1] == 1)
						walls += 1;
				if (mazeGrid [x + 1, y] == 1)
						walls += 2;
				if (mazeGrid [x, y - 1] == 1)
						walls += 4;
				if (mazeGrid [x - 1, y] == 1)
						walls += 8;
				if (walls == 0)
						go = Instantiate (cell00);
				if (walls == 1)
						go = Instantiate (cell01);
				if (walls == 2)
						go = Instantiate (cell02);
				if (walls == 3)
						go = Instantiate (cell03);
				if (walls == 4)
						go = Instantiate (cell04);
				if (walls == 5)
						go = Instantiate (cell05);
				if (walls == 6)
						go = Instantiate (cell06);
				if (walls == 7)
						go = Instantiate (cell07);
				if (walls == 8)
						go = Instantiate (cell08);
				if (walls == 9)
						go = Instantiate (cell09);
				if (walls == 10)
						go = Instantiate (cell10);
				if (walls == 11)
						go = Instantiate (cell11);
				if (walls == 12)
						go = Instantiate (cell12);
				if (walls == 13)
						go = Instantiate (cell13);
				if (walls == 14)
						go = Instantiate (cell14);
				if (walls == 15)
						go = Instantiate (cell15);

				go.transform.localScale = new Vector3 (0.05f*tileSize, 0.05f*mazeHeight, 0.05f*tileSize);
				go.transform.position = new Vector3 (xpos * 0.9f*tileSize, 0.2f*mazeHeight, ypos * 0.9f*tileSize); 
			}
		}
	}


	public void deleteFloor() {
		GameObject[] floors = GameObject.FindGameObjectsWithTag ("floor");
		foreach (GameObject go in floors) {
			DestroyImmediate (go);
		}
	}

	//
	// public void addFloor() 
	//
	// draw a floor benath the maze
	//

	public void addFloor() {
		GameObject floor = Instantiate(floorElement);
		float floorWidth = mazeWidth * tileSize + (mazeWidth + 1) * wallWidth;
		float floorLength = mazeLength * tileSize + (mazeLength + 1) * wallWidth;

		floor.transform.localScale = new Vector3 (floorWidth,0.01f,floorLength);
		floor.transform.localPosition = new Vector3(floorWidth/2f-wallWidth/2f, -mazeHeight/2f+0.01f, floorLength/2f-wallWidth/2f);
		floor.name = "floor";
		floor.tag = "floor";
	}


		public bool solveMazeIterative(int x, int y) {
				pathPoint pos;
				bool ok = false;
				Stack<pathPoint> store = new Stack<pathPoint> ();

				for (int y1 = 1; y1 < mHeight - 1; y1++) {
						for (int x1 = 1; x1 < mWidth - 1; x1++) {
								if (x1 % 2 == 1 && y1 % 2 == 1)
										// 2 means unvisited cell
										mazeGrid [x1, y1] = 2;
						}
				}

				pos = new pathPoint (new Vector2 (x, y), null);
				store.Push (pos);
				mazeGrid[x,y] = 0;

				while (!ok && store.Count != 0) {
						pos = store.Pop ();
						x = (int)pos.position.x;
						y = (int)pos.position.y;
						if (pos.position == mazeExit) {
								ok = true;
						}
						else {
								if (x <= mWidth - 3 && mazeGrid [x + 1, y] == 0 && mazeGrid[x+2,y] == 2) {
										store.Push(new pathPoint(new Vector2(x+2,y), pos));
										mazeGrid[x+2,y] = 0;
								}
								if (y <= mHeight - 3 && mazeGrid [x, y + 1] == 0 && mazeGrid[x,y+2] == 2) {
										store.Push(new pathPoint(new Vector2(x,y+2), pos));
										mazeGrid[x,y+2] = 0;
								}
								if (x >=3 && mazeGrid [x - 1, y] == 0 && mazeGrid[x-2,y] == 2) {
										store.Push(new pathPoint(new Vector2(x-2,y), pos));
										mazeGrid[x-2,y] = 0;
								}
								if (y >= 3 && mazeGrid [x, y - 1] == 0 && mazeGrid[x,y-2] == 2) {
										store.Push(new pathPoint(new Vector2(x,y-2), pos));
										mazeGrid[x,y-2] = 0;
								}
						}
				}

				if (ok) {
						do {
								solutionPath.Add (pos.position);
								pos = pos.prevPosition;
						} while (pos != null);
				}

				return ok;
		}


		public bool solveMaze(int x, int y, int dir) {
				bool ok = false;
				for (int i = 0; i < 4 && !ok; i++) {
						if (i != dir) {
								switch (i) {
								// 0 = north
								case 0:
										if (y <= mHeight - 3 && mazeGrid [x, y + 1] == 0) {
												ok = solveMaze (x, y + 2, 2);
										}
										break;
										// 1 = east
								case 1:	
										if (x <= mWidth -3 && mazeGrid [x + 1, y] == 0)
												ok = solveMaze (x + 2, y, 3);	
										break;
										// 2 = down
								case 2:
										if (y >= 3 && mazeGrid [x, y - 1] == 0)
												ok = solveMaze (x, y - 2, 0);
										break;
										// 3 = west
								case 3:
										if (x >= 3 && mazeGrid [x - 1, y] == 0)
												ok = solveMaze (x - 2, y, 1);
										break;
								}
						}
				}
				if (x == mazeExit.x && y == mazeExit.y) {
						ok = true;
				}

				if (ok) {
						solutionPath.Add(new Vector2(x,y));
				}
				return ok;
		}


	public void drawSolutionCubes() {
		float height = GameObject.FindGameObjectWithTag ("floor").transform.localPosition.y;
		GameObject[] path = GameObject.FindGameObjectsWithTag ("solutionPath");
		foreach (GameObject go in path) {
			DestroyImmediate (go);
		}

		for (int i = 0; i < solutionPath.Count-1;i++) {
			Vector2 startPos = solutionPath [i];
			Vector2 endPos = solutionPath [i + 1];

			GameObject pe = Instantiate (pathElement);
			Vector2 delta = (endPos - startPos)/2f;
			if (delta.x != 0) {
				pe.transform.localScale = new Vector3 (tileSize + wallWidth, 0.5f, 0.1f*tileSize);
				pe.transform.localPosition = new Vector3((startPos.x + delta.x)* (tileSize + wallWidth)/2f, height, startPos.y * (tileSize + wallWidth) / 2f);
			} 
			else {
				pe.transform.localScale = new Vector3 (0.1f*tileSize, 0.5f, tileSize + wallWidth);
				pe.transform.localPosition = new Vector3(startPos.x* (tileSize + wallWidth)/2f, height, (startPos.y + delta.y) * (tileSize + wallWidth) / 2f);
			}
		}
	}
			

		public void drawSolutionPrefabs() {
				float height = 0.2f*mazeHeight/2f;
				GameObject[] path = GameObject.FindGameObjectsWithTag ("solutionPath");
				foreach (GameObject go in path) {
						DestroyImmediate (go);
				}

				for (int i = 0; i < solutionPath.Count-1;i++) {
						Vector2 startPos = solutionPath [i];
						Vector2 endPos = solutionPath [i + 1];

						GameObject pe = Instantiate (pathElement);
						Vector2 delta = (endPos - startPos)/2f;
						if (delta.x != 0) {
								pe.transform.localScale = new Vector3 (0.9f*tileSize, 0.5f, 0.1f*tileSize);
								pe.transform.localPosition = new Vector3((startPos.x + delta.x)* 0.9f*tileSize/2f, height, startPos.y * 0.9f *tileSize/ 2f);
						} 
						else {
								pe.transform.localScale = new Vector3 (0.1f*tileSize, 0.5f, 0.9f*tileSize);
								pe.transform.localPosition = new Vector3(startPos.x* 0.9f*tileSize/2f, height, (startPos.y + delta.y) * 0.9f*tileSize / 2f);
						}
				}
		}


	class pathPoint {
		public Vector2 position;
		public pathPoint prevPosition;

		public pathPoint(Vector2 _position, pathPoint _prevPosition){
			position = _position;
			prevPosition = _prevPosition;
		}
	};

}
