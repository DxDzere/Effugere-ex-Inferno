using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class recursiveBacktracker : BaseGenerator {
		
	//
	// public void generateMaze(int width, int height) 
	//
	//
	//

	public override void generateMaze() {

		base.generateMaze ();

		// start up the generation
		addInnerWalls ();
		// draw the outer wall
		addOuterWalls ();
		// do the binaryTree 
		doMaze();
		// add an entrance
		if (makeEntrance)
			addEntrance();
		// add an exit
		if (makeExit)
			addExit ();

	}



	public void addInnerWalls() {
		for (int y = 1; y < mHeight - 1; y++) {
			for (int x = 1; x < mWidth - 1; x++) {
				if (x % 2 == 1 && y % 2 == 1)
					// 2 means unvisited cell
					mazeGrid [x, y] = 2;
				else
					// 1 means wall
					mazeGrid [x, y] = 1;
			}
		}
	}

	

	public void doMaze() {
		Stack<Vector2> mazeStack = new Stack<Vector2>();
		bool allCellsVisited = false;
		string possibleMoves;
		Vector2 activeCell = new Vector2(1,1);

		while (!allCellsVisited) {
			possibleMoves = "";
			if (activeCell.x <= mWidth - 3 && mazeGrid[(int)activeCell.x + 2,(int)activeCell.y] == 2)
					possibleMoves += "E";
			if (activeCell.x >= 3 && mazeGrid[(int)activeCell.x - 2,(int)activeCell.y] == 2)
					possibleMoves += "W";
			if (activeCell.y <= mHeight - 3 && mazeGrid[(int)activeCell.x, (int)activeCell.y + 2] == 2)
					possibleMoves += "N";
			if (activeCell.y >= 3 && mazeGrid[(int)activeCell.x, (int)activeCell.y- 2] == 2)
					possibleMoves += "S";
			
			// are any move possible
			// this means: the string is not empty
			if (possibleMoves != "") {
					mazeStack.Push (activeCell);
					mazeGrid [(int)activeCell.x, (int)activeCell.y] = 0;
					// randomly chose a direction
					string chosenDir = possibleMoves [Random.Range (0, possibleMoves.Length)].ToString ();
					switch (chosenDir) {
					case "N":
						mazeGrid [(int)activeCell.x, (int)activeCell.y + 1] = 0;
						activeCell = new Vector2 (activeCell.x, activeCell.y + 2);
						mazeGrid [(int)activeCell.x, (int)activeCell.y] = 0;
						break;
					case "S":
						mazeGrid [(int)activeCell.x, (int)activeCell.y - 1] = 0;
						activeCell = new Vector2 (activeCell.x, activeCell.y - 2);
						mazeGrid [(int)activeCell.x, (int)activeCell.y] = 0;
						break;
					case "W":
						mazeGrid [(int)activeCell.x - 1, (int)activeCell.y] = 0;
						activeCell = new Vector2 (activeCell.x - 2, activeCell.y);
						mazeGrid [(int)activeCell.x, (int)activeCell.y] = 0;
						break;
					case "E":
						mazeGrid [(int)activeCell.x + 1, (int)activeCell.y] = 0;
						activeCell = new Vector2 (activeCell.x + 2, activeCell.y);
						mazeGrid [(int)activeCell.x, (int)activeCell.y] = 0;
						break;
					}
			} else {
				if (mazeStack.Count > 0)
					activeCell = mazeStack.Pop ();
				else
					allCellsVisited = true;
			}
		}
	}
				


}
