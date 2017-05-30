using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// this script generates the maze


public class MazeCreator : MonoBehaviour {

	// there are 16 possible maze objects
	// which are drawn to the slot in the inspector
	public GameObject[] cellTypes;
	// we need a prefab for creation
	public GameObject cellPrefab;
	// put an AI prefab here, if you want
	public GameObject AI;
	// the maze itself is stored in an array
	GameObject[,] cells;
	
	// Initialize an array of references to the Scripts attached to the cells
	// to avoid multiple getComponent<mazeCell>().blabla() calls,
	// which is totally inefficient
	cell[,] cellScripts;

	// dimension of the current maze
	int cellsX = 25;
	int cellsY = 15;

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
	};
	
	public GameStates gameState;
	
	// display the actual frames per second
	float fps;
	
	
	// Use this for initialization
	void Start () {
		// setthe actual game state
		gameState = GameStates.initGame;
		// initialize the structures
		cells = new GameObject[cellsX,cellsY];
		cellScripts = new cell[cellsX,cellsY];
		// set the center cell to active
		activeCell = new Vector2(12,8);
		setupMaze();
		// crush the walls to the neighbors
		crushCenterWalls();
		// store the actual cell as visited on a stack
		pushVisitedCells();
		// Instantiate 8 AI radomly in the maze
		Instantiate(AI, new Vector3(Random.Range(0,cellsX)*16, 0.9f, Random.Range(0,cellsY)*16),Quaternion.identity);
		Instantiate(AI, new Vector3(Random.Range(0,cellsX)*16, 0.9f, Random.Range(0,cellsY)*16),Quaternion.identity);
		Instantiate(AI, new Vector3(Random.Range(0,cellsX)*16, 0.9f, Random.Range(0,cellsY)*16),Quaternion.identity);
		Instantiate(AI, new Vector3(Random.Range(0,cellsX)*16, 0.9f, Random.Range(0,cellsY)*16),Quaternion.identity);
		Instantiate(AI, new Vector3(Random.Range(0,cellsX)*16, 0.9f, Random.Range(0,cellsY)*16),Quaternion.identity);
		Instantiate(AI, new Vector3(Random.Range(0,cellsX)*16, 0.9f, Random.Range(0,cellsY)*16),Quaternion.identity);
		Instantiate(AI, new Vector3(Random.Range(0,cellsX)*16, 0.9f, Random.Range(0,cellsY)*16),Quaternion.identity);
		Instantiate(AI, new Vector3(Random.Range(0,cellsX)*16, 0.9f, Random.Range(0,cellsY)*16),Quaternion.identity);
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
				cellScripts[x,y] = cells[x,y].GetComponent<cell>();
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

		// update the maze view
		cellScripts[12,7].changeView(cellTypes[cellScripts[12,7].getWalls()]);
		cellScripts[12,8].changeView(cellTypes[cellScripts[12,8].getWalls()]);
		cellScripts[12,6].changeView(cellTypes[cellScripts[12,6].getWalls()]);
		cellScripts[13,7].changeView(cellTypes[cellScripts[13,7].getWalls()]);
		cellScripts[11,7].changeView(cellTypes[cellScripts[11,7].getWalls()]);

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
	
	

	// Update is called once per frame
	//
	// I put th emaze generation in th eUpdate method because I wanted to 
	// present it visually.
	//

	void Update () {
		
		fps = 1.0f / Time.deltaTime;
		
		switch (gameState) {
		case GameStates.initGame:
			gameState = GameStates.generateMaze;
			break;
		case GameStates.generateMaze:
			// are all cells visited?
			// then set game to next state
			if (allCellsVisited()) {
				gameState = GameStates.removeDeadEnd;
				break;
			}
			
			// get all possible moves (concerning the actual cell)
			// and store them in a string
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
					//update the maze view
					cellScripts[(int)activeCell.x, (int)activeCell.y].changeView(cellTypes[cellScripts[(int)activeCell.x, (int)activeCell.y].getWalls()]);
					cellScripts[(int)activeCell.x, (int)activeCell.y-1].changeView(cellTypes[cellScripts[(int)activeCell.x, (int)activeCell.y-1].getWalls()]);
					activeCell = new Vector2(activeCell.x,activeCell.y-1);
					cellScripts[(int)activeCell.x, (int)activeCell.y].visitCell();
					break;
				case "S":
					cellScripts[(int)activeCell.x, (int)activeCell.y].crushSouthWall();
					cellScripts[(int)activeCell.x, (int)activeCell.y+1].crushNorthWall();
					//update the maze view
					cellScripts[(int)activeCell.x, (int)activeCell.y].changeView(cellTypes[cellScripts[(int)activeCell.x, (int)activeCell.y].getWalls()]);
					cellScripts[(int)activeCell.x, (int)activeCell.y+1].changeView(cellTypes[cellScripts[(int)activeCell.x, (int)activeCell.y+1].getWalls()]);
					activeCell = new Vector2(activeCell.x,activeCell.y+1);
					cellScripts[(int)activeCell.x, (int)activeCell.y].visitCell();
					break;
				case "W":
					cellScripts[(int)activeCell.x, (int)activeCell.y].crushWestWall();
					cellScripts[(int)activeCell.x+1, (int)activeCell.y].crushEastWall();
					//update the maze view
					cellScripts[(int)activeCell.x, (int)activeCell.y].changeView(cellTypes[cellScripts[(int)activeCell.x, (int)activeCell.y].getWalls()]);
					cellScripts[(int)activeCell.x+1, (int)activeCell.y].changeView(cellTypes[cellScripts[(int)activeCell.x+1, (int)activeCell.y].getWalls()]);
					activeCell = new Vector2(activeCell.x+1,activeCell.y);
					cellScripts[(int)activeCell.x, (int)activeCell.y].visitCell();
					break;
				case "E":
					cellScripts[(int)activeCell.x, (int)activeCell.y].crushEastWall();
					cellScripts[(int)activeCell.x-1, (int)activeCell.y].crushWestWall();
					//update the maze view
					cellScripts[(int)activeCell.x, (int)activeCell.y].changeView(cellTypes[cellScripts[(int)activeCell.x, (int)activeCell.y].getWalls()]);
					cellScripts[(int)activeCell.x-1, (int)activeCell.y].changeView(cellTypes[cellScripts[(int)activeCell.x-1, (int)activeCell.y].getWalls()]);
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
			
			break;
		case GameStates.removeDeadEnd:

			break;
		}
	}


	//
	// void OnGUI
	//
	// This method shows GUI elements. 
	// in this case this is the FPS display

	void OnGUI(){
		GUI.Label(new Rect(10,10,200,100),"FPS: " + fps);
	}
}
