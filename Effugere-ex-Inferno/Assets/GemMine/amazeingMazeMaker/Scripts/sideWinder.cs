using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sideWinder : BaseGenerator {


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



	//
	// public void addInnerWalls() 
	//
	// Sidewinder crushes down walls
	// so starting point is a fully "walled" maze
	//

	public void addInnerWalls() {
		for (int y = 1; y < mHeight - 1; y++) {
			for (int x = 1; x < mWidth - 1; x++) {
				if (x % 2 == 1 && y % 2 == 1)
					mazeGrid [x, y] = 0;
				else
					mazeGrid [x, y] = 1;
			}
		}
	}



	//
	// public void doMaze() 
	//
	// this method generates the maze
	//

	public void doMaze() {
		int run_start;		
		for (int y = 1; y < mHeight - 1; y++) {
			run_start = 1;
			for (int x = 1; x < mWidth - 1; x++) {
				if (x % 2 == 1 && y % 2 == 1) { 
					if (y < mHeight-3 && (x + 1 == mWidth - 1 || Random.Range (0, 3) == 0)) {
						int cell = (run_start / 2 + 1) + Random.Range (0, (x / 2 + 1) - (run_start / 2 + 1) + 1);
						mazeGrid [cell*2-1, y + 1] = 0;
						run_start = x + 2;
					} else if (x + 2 < mWidth - 1) {
						mazeGrid [x + 1, y] = 0;
					}
				}
			}
		}
	}
				
}
