using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// this script generates the maze

[ExecuteInEditMode]
public class MazeCreatorStatic : MonoBehaviour {

	// there are 16 possible maze objects
	// which are drawn to the slot in the inspector
	public GameObject[] cellTypes;
	// we need a prefab for creation
	public GameObject cellPrefab;
	// the maze itself is stored in an array
	GameObject[,] cells;
	
	// Initialize an array of references to the Scripts attached to the cells
	// to avoid multiple getComponent<mazeCell>().blabla() calls,
	// which is totally inefficient
	cellStatic[,] cellScripts;

	// dimension of the current maze
	public int cellsX = 25;
	public int cellsY = 15;

	// remove dead ends
	public bool removeDeadEnds;

	public bool useRandomSeed;
	public int randomSeed = 20586; 

	// since we are doing backtracking in the process of maze creation, 
	// we store the steps in an stack
	Stack<Vector2> mazeStack = new Stack<Vector2>();

	// which is the cell currently visited
	Vector2 activeCell;

	// the maze generatiokn comprises three states
	public enum GameStates {
		initGame,
		generateMaze,
		removeDeadEnd,
		mazeReady,
	};
	
	public GameStates gameState;
	
	// display the actual frames per second
	float fps;
	
	
	// Use this for initialization
	void Start () {
		// call the method to generate a maze
		makeMaze ();
	}


	public void makeMaze() {
		// check, if already a maze has been generated
		// if so, delete it
		deleteOldMaze ();
		// setthe actual game state
		gameState = GameStates.initGame;
		// initialize the structures
		cells = new GameObject[cellsX,cellsY];
		cellScripts = new cellStatic[cellsX,cellsY];
		// set the center cell to active
		activeCell = new Vector2(12,8);
		setupMaze();
		// crush the walls to the neighbors
		crushCenterWalls();
		// store the actual cell as visited on a stack
		pushVisitedCells();
		// do we use a random seed
		if (useRandomSeed)
			Random.seed = randomSeed;
		// generate the main maze
		generateMaze ();
		// shall dead ends be removed
		if (removeDeadEnds)
			removeDeads ();
		// show th emaze in the scene view
		setupMazeView ();
		// clean up elements not further needed
		cleanUp ();
	}

	public void deleteOldMaze() {
		foreach(GameObject gameObj in GameObject.FindObjectsOfType<GameObject>())
		{
			if(gameObj.name.Contains("cellStatic"))
			{
				DestroyImmediate(gameObj);
			}
		}
		cleanUp ();
	}


	public void cleanUp() {
		foreach(GameObject gameObj in GameObject.FindObjectsOfType<GameObject>())
		{
			if(gameObj.name.Contains("cell-static"))
			{
				DestroyImmediate(gameObj);
			}
		}
	}

	public void setupMazeView() {
		for (int y = 0; y < cellsY; y++) {
			for (int x = 0; x < cellsX; x++) {
				GameObject go = Instantiate(cellTypes[cellScripts[x,y].getWalls()],new Vector3(x*16,0,y*16), Quaternion.identity) as GameObject;
				go.name = "cellStatic_"+x+"_"+y;
				Vector3 rotation = go.transform.rotation.eulerAngles;
				rotation.x -= 90;
				go.transform.rotation = Quaternion.Euler(rotation);

			}
		}
	}


	//
	// void setupMaze
	//
	// this method creates the maze by instantiating 
	// the cell prefab (with all walls) 
	//

	void setupMaze(){
		for (int y = 0;y < cellsY; y++) {
			for (int x = 0; x < cellsX; x++) {
				cells[x,y] = (GameObject)Instantiate(cellPrefab,new Vector3(x*16,0,y*16), Quaternion.identity);
				cellScripts[x,y] = cells[x,y].GetComponent<cellStatic>();
				cells[x,y].name = cells[x,y].name + x + "_" +  y;
			}
		}
	}
	
	
	//
	// void crushCenterWalls
	//
	// this method starts the creation of the maze by 
	// crushing the walls of the center cell and randomly visiting the neighbors 
	//

	void crushCenterWalls(){

		// 12,7 ist cell in the middle
		// update the maze model

		cellScripts[12,7].crushNorthWall();
		cellScripts[12,6].crushSouthWall();

		cellScripts[12,7].crushSouthWall();
		cellScripts[12,8].crushNorthWall();

		cellScripts[12,7].crushWestWall();
		cellScripts[13,7].crushEastWall();

		cellScripts[12,7].crushEastWall();
		cellScripts[11,7].crushWestWall();

		// mark actual cell and surrounding cells as visited
		cellScripts[12,7].visitCell();
		cellScripts[12,6].visitCell();
		cellScripts[12,8].visitCell();
		cellScripts[13,7].visitCell();
		cellScripts[11,7].visitCell();
	}


	// 
	// void pushVisitedCells
	// 
	// the visited cells are put on a stack
	// for the purpose of backtracking
	//

	public void pushVisitedCells() {
		mazeStack.Push(new Vector2(12,7));
		mazeStack.Push(new Vector2(12,6));
		mazeStack.Push(new Vector2(12,8));
		mazeStack.Push(new Vector2(13,7));
		mazeStack.Push(new Vector2(11,7));
	}
	

	//
	// bool allCellsVisited
	//
	// if all cells of the maze are visited, 
	// the nect game phase can be triggered
	//

	public bool allCellsVisited() {
		bool visited = true;
		for (int y = 0;y < cellsY; y++) {
			for (int x = 0; x < cellsX; x++) {
				visited = visited && cellScripts[x,y].isVisited();
				if (!visited) return false;
			}
		}
		return visited;
	}
	


	public void removeDeads()
	{
		// Remove walls on center fields
		if (cellScripts[12, 6].cellWalls == 11)
		{
			cellScripts[12, 6].crushNorthWall();
			cellScripts[12, 5].crushSouthWall();
		}
		if (cellScripts[11, 7].cellWalls == 7)
		{
			cellScripts[11, 7].crushEastWall();
			cellScripts[10, 7].crushWestWall();
		}
		if (cellScripts[13, 7].cellWalls == 13)
		{
			cellScripts[13, 7].crushWestWall();
			cellScripts[14, 7].crushEastWall();
		}
		if (cellScripts[12, 8].cellWalls == 14)
		{
			cellScripts[12, 8].crushSouthWall();
			cellScripts[12, 9].crushNorthWall();
		}

		// Process all cells except border cells
		// Remove only wall in direction of the dead end
		for (int spalten = 0; spalten < cellsX; spalten++)
		{
			for (int zeilen = 0; zeilen < cellsY; zeilen++)
			{
				// Wall to the right
				if (cellScripts[spalten, zeilen].cellWalls == 7 && spalten > 0)
				{
					cellScripts[spalten, zeilen].crushEastWall();
					cellScripts[spalten - 1, zeilen].crushWestWall();
				}
				
				// Wall to the up
				if (cellScripts[spalten, zeilen].cellWalls == 11 && zeilen > 0)
				{
					cellScripts[spalten, zeilen].crushNorthWall();
					cellScripts[spalten, zeilen - 1].crushSouthWall();

				}
				
				// Wall to the left
				if (cellScripts[spalten, zeilen].cellWalls == 13 && spalten < cellsX)
				{
						cellScripts[spalten, zeilen].crushWestWall();
						cellScripts[spalten + 1, zeilen].crushEastWall();
				}
							
				// Wall to the down
				if (cellScripts[spalten, zeilen].cellWalls == 14 && zeilen < cellsY)
				{
					cellScripts[spalten, zeilen].crushSouthWall();
					cellScripts[spalten, zeilen + 1].crushNorthWall();
				}

			}
		}

	}



	void generateMaze() {
		gameState = GameStates.generateMaze;

		if (allCellsVisited ()) {
			gameState = GameStates.removeDeadEnd;
		}

		while (gameState == GameStates.generateMaze) {
			string possibleMoves = "";
			if (activeCell.x > 0 && cellScripts[(int)activeCell.x - 1, (int)activeCell.y].getWalls() == 15)
				possibleMoves+="E";
			if (activeCell.x < cellsX-1 && cellScripts[(int)activeCell.x + 1, (int)activeCell.y].getWalls() == 15)
				possibleMoves+="W";
			if (activeCell.y < cellsY-1 && cellScripts[(int)activeCell.x, (int)activeCell.y + 1].getWalls() == 15)
				possibleMoves+="S";
			if (activeCell.y > 0  && cellScripts[(int)activeCell.x, (int)activeCell.y - 1].getWalls() == 15)
				possibleMoves+="N";
			
			// are any moves possible?
			// this means: if the string is notempty
			if (possibleMoves!="") {
				// push actual cell to stack
				mazeStack.Push(activeCell);
				cellScripts[(int)activeCell.x, (int)activeCell.y].visitCell();
				// randomly chose a direction 
				// (and a cell the maze move to)
				string chosen = possibleMoves[Random.Range(0,possibleMoves.Length)].ToString();
				switch (chosen){
				case "N":
					cellScripts[(int)activeCell.x, (int)activeCell.y].crushNorthWall();
					cellScripts[(int)activeCell.x, (int)activeCell.y-1].crushSouthWall();
					activeCell = new Vector2(activeCell.x,activeCell.y-1);
					cellScripts[(int)activeCell.x, (int)activeCell.y].visitCell();
					break;
				case "S":
					cellScripts[(int)activeCell.x, (int)activeCell.y].crushSouthWall();
					cellScripts[(int)activeCell.x, (int)activeCell.y+1].crushNorthWall();
					activeCell = new Vector2(activeCell.x,activeCell.y+1);
					cellScripts[(int)activeCell.x, (int)activeCell.y].visitCell();
					break;
				case "W":
					cellScripts[(int)activeCell.x, (int)activeCell.y].crushWestWall();
					cellScripts[(int)activeCell.x+1, (int)activeCell.y].crushEastWall();
					activeCell = new Vector2(activeCell.x+1,activeCell.y);
					cellScripts[(int)activeCell.x, (int)activeCell.y].visitCell();
					break;
				case "E":
					cellScripts[(int)activeCell.x, (int)activeCell.y].crushEastWall();
					cellScripts[(int)activeCell.x-1, (int)activeCell.y].crushWestWall();
					activeCell = new Vector2(activeCell.x-1,activeCell.y);
					cellScripts[(int)activeCell.x, (int)activeCell.y].visitCell();
					break;
				}
			}
			// no possible moves
			else {
				if (mazeStack.Count > 0)
					activeCell = mazeStack.Pop();
				else 
					gameState = GameStates.removeDeadEnd;
			}
		}
	}


	// Update is called once per frame
	//
	// I put th emaze generation in th eUpdate method because I wanted to 
	// present it visually.
	//

	void Update () {

	}


	//
	// void OnGUI
	//
	// This method shows GUI elements. 
	// in this case this is the FPS display

	void OnGUI(){

	}
}
