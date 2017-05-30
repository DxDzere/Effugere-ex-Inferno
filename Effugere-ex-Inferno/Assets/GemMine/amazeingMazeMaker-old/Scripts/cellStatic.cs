using UnityEngine;
using System.Collections;

// this script represents the data structure for a maze cell


public class cellStatic : MonoBehaviour {
	
	// bitwise enumeration for the walls
	public int northWall = 1;
	public int eastWall  = 2;
	public int southWall = 4;
	public int westWall  = 8;
	public int cellWalls;

	// was cell aready visited
	bool visited;

	// cell's position on the grid
	Vector2 position;

	// Use this for initialization
	// avoid "Start", since MazeCreator accesses this Gameobject
	void Awake () {
		// set the predefined cellView
		// which is "complete"
		// that means, the cell is intact and has all walls
		// cell hasn't been visited
		visited =  false;
		// set all the walls
		cellWalls = northWall + southWall + westWall + eastWall;
	}

	
	// Update is called once per frame
	void Update () {
	
	}


	//
	// user methods
	//
	// to set and get cell information

	
	//
	// bool isVisited
	//
	// has the cell been visited so far?
	// (we are using a depth first maze creation algorithm)
	//

	public bool isVisited() {
		return visited;
	}


	//
	// void visitCell
	//
	// ok. we visit the cell now
	//

	public void visitCell() {
		visited = true;
	}
	
	
	//
	// void setPosition
	//
	// set the cell's position on the grid (column/row)
	//

	public void setPosition(int spalte, int zeile) {
		position = new Vector2(spalte,zeile);
	}


	//
	// Vector2 getPosition
	//
	// where is the cell located? column/row)
	//

	public Vector2 getPosition(){
		return position;
	}


	//
	// crushWalls
	//
	// if a cell has been visited, randomly a new cell is chosen
	// which has to be visited next. 
	// to visit th enewxt cell, two walls have to be crushed.
	// if the new cell is to the north,
	// the north wall of the current cell and
	// the south wall of the next cell
	// has to be crushed
	//

	public void crushNorthWall() {
		if ((cellWalls & northWall) != 0)
		cellWalls -= northWall;
	}
	
	public void crushSouthWall() {
		if ((cellWalls & southWall) != 0)
		cellWalls -= southWall;
	}
	
	public void crushEastWall() {
		if ((cellWalls & eastWall) != 0)
		cellWalls -= eastWall;
	}
	
	public void crushWestWall() {
		if ((cellWalls & westWall) != 0)
		cellWalls -= westWall;
	}


	//
	// int hasWall
	//
	// the next methods determine whether a cell 
	// has a wall to the wanted direction
	//

	public int hasWestWall()
	{
		return (cellWalls & westWall);
	}
	
	public int hasEastWall()
	{
		return (cellWalls & eastWall);
	}
	
	public int hasNorthWall()
	{
		return (cellWalls & northWall);
	}
	
	public int hasSouthWall()
	{
		return (cellWalls & southWall);
	}


	//
	// int getWalls
	//
	// get the walls a cells owns at the moment
	// used for routing in the maze
	//

	public int getWalls() {
		return cellWalls;
	}
}
