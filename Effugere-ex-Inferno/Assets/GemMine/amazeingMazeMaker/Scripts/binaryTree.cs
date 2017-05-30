using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class binaryTree : BaseGenerator {


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




	public override void addInnerWalls() {
		for (int y = 1; y < mHeight - 1; y++) {
			for (int x = 1; x < mWidth - 1; x++) {
				if (x % 2 == 1 && y % 2 == 1)
					mazeGrid [x, y] = 0;
				else
					mazeGrid [x, y] = 1;
			}
		}
	}




	public void doMaze() {
		List<string> directions = new List<string>();
		for (int y = 1; y < mHeight - 1; y++) {
			for (int x = 1; x < mWidth - 1; x++) {
				if (x % 2 == 1 && y % 2 == 1) {
					directions.Clear();
					if (x < mWidth-3)
						directions.Add("E");
					if (y < mHeight -3)
						directions.Add("N");

					// true only for the upper right tile
					if (directions.Count == 0)
						continue;
					
					string dir = directions [Random.Range (0, directions.Count)];
					if (dir == "E") {
						mazeGrid [x + 1, y] = 0;
					}
					if (dir == "N")
						mazeGrid [x, y + 1] = 0;
				}
			}
		}
	}


}
