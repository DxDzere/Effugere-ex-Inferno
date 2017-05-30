using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGenerator {

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

	protected Vector2 mazeEntrance;
	protected Vector2 mazeExit;

	protected bool makeEntrance = false;
	protected bool makeExit = false;

	//
	// public BaseGenerator()
	//
	// standard constructor
	//

	public BaseGenerator() {
	}


	public Vector2 getEntrance() {
		return mazeEntrance;
	}


	public Vector2 getExit() {
		return mazeExit;
	}

	//
	// public virtual void setParameters(...)
	//
	// The configured parameters in the editor
	// are set
	//

	public virtual void setParameters(int _mazeWidth = 	10, int _mazeLength = 10, 
		float  _mazeHeight = 1f, int _seed = 12345, float _wallWidth = 1f, 
				float _tileSize = 1f, bool _makeEntrance = true, bool _makeExit = true) {	
		mazeWidth = _mazeWidth;
		mazeHeight = _mazeHeight;
		mazeLength = _mazeLength;
		seed = _seed;
		wallWidth = _wallWidth;
		tileSize = _tileSize;
		makeEntrance = _makeEntrance;
		makeExit = _makeExit;
	}



	//
	// public virtual int[,] createMaze() 
	//
	// create a maze and return the grid 
	// to the main manager class
	//

	public virtual int[,] createMaze() {
		mWidth = mazeWidth * 2 + 1;
		mHeight = mazeLength * 2 + 1;
		// generate a maze with given width and height
		generateMaze ();
		return mazeGrid;
	}



	//
	// public virtual void generateMaze() 
	//
	// do the work
	//

	public virtual void generateMaze() {
				
		// generate the same dungeon over and over again
		Random.InitState(seed);

		// width and height are the maze tiles
		// each wall has its own tile, so we have to create 2*n+1 tiles

		// initialize the grid
		mazeGrid = new int[mWidth, mHeight];

		// reset the grid
		for (int y = 0; y < mHeight; y++) {
				for (int x = 0; x < mWidth; x++) {
						mazeGrid [x, y] = 0;
				}
		}	
	}



	//
	// public void addOuterWalls() 
	//
	// draw the bounding box
	//

	public void addOuterWalls() {
		for (int y = 0; y < mHeight; y++) {
			mazeGrid [0, y] = 1;
			mazeGrid [mWidth - 1,y] = 1;
		}

		for (int x = 1; x < mWidth - 1; x++) {
			mazeGrid [x, 0] = 1;
			mazeGrid [x,mHeight - 1] = 1;
		}
	}



	//
	// public void addEntrance() 
	//
	// add an entrance to the maze
	//

	public virtual void addEntrance() {
		int entrance = (int)Mathf.Floor(Random.Range(1, mWidth/4)/2)*2+1;
		mazeGrid [entrance,0] = 0;
		mazeEntrance = new Vector2 (entrance, 1);
	}



	//
	// public void addExit() 
	//
	// add an exit to the maze
	//

	public virtual void addExit() {
		int exit = (int)Mathf.Floor(Random.Range(mWidth/4*3, mWidth-1)/2)*2+1;
		mazeGrid [exit, mHeight-1] = 0;
		mazeExit = new Vector2 (exit, mHeight - 2);
	}



	//
	// public virtual void addInnerWalls() 
	//
	// each generator has its own method 
	// to set inner walls
	//

	public virtual void addInnerWalls() {
	}
}
