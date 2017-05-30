using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class recursiveDivisionMaze : BaseGenerator {


	//
	// public void generateMaze(int width, int height) 
	//
	//
	//

	public override void generateMaze() {

		base.generateMaze ();

		// start up the generation
		addInnerWalls (true, 1, mWidth - 2, 1, mHeight- 2);
		// draw the outer wall
		addOuterWalls ();
		// add an entrance
		if (makeEntrance)
			addEntrance();
		// add an exit
		if (makeExit)
			addExit ();
	}
				


	//
	// public void addInnerWalls(bool isHoriz, int minX, int maxX, int minY, int maxY) 
	//
	// heart of the maze creation
	// recursive function
	//

	public void addInnerWalls(bool isHoriz, int minX, int maxX, int minY, int maxY) {
		// are we horizontal?
		if (isHoriz) {
			// recursion anchor
			if (maxX - minX < 2)
				return;

			// determine a random divider
			int y  = (int)Mathf.Floor(Random.Range(minY, maxY+1)/2)*2;
			// add a horizontal wall
			addHWall (minX, maxX, y);
			// do the recursion for the upper and lower dungeon
			addInnerWalls (!isHoriz, minX, maxX, minY, y - 1);
			addInnerWalls (!isHoriz, minX, maxX, y + 1, maxY);
		} 
		// we are vertical
		else {
			// recursion anchor
			if (maxY - minY < 2)
				return;

			// determine a random divider
			int x  = (int)Mathf.Floor(Random.Range(minX, maxX+1)/2)*2;
			// ad a vertical wall
			addVWall (minY, maxY, x);
			// do the recursion for the left and right dungeon
			addInnerWalls (!isHoriz, minX, x - 1, minY, maxY);
			addInnerWalls (!isHoriz, x + 1, maxX, minY, maxY);
		}
	}




	//
	// public void addHWall(int minX, int maxX, int y)
	//
	// draw a horizontal dividing wall from left to right
	//

	public void addHWall(int minX, int maxX, int y) {
		// set a random position for the hole in the wall which connects the two dungeon parts
		int hole = (int)Mathf.Floor(Random.Range(minX, maxX+1)/2)*2+1;

		// draw the wall and omit the hole
		for (int x = minX; x <= maxX; x++) {
			if (x != hole)
				mazeGrid [x, y] = 1;
			else
				mazeGrid [x, y] = 0;
		}
	}



	//
	// public void addVWall(int minY, int maxY, int x
	//
	// draw a vertical dividing wall from top to down
	//

	public void addVWall(int minY, int maxY, int x) {
		// set a random position for the hole in the wall which connects the two dungeon parts
		int hole = (int)Mathf.Floor(Random.Range(minY, maxY+1)/2)*2+1;

		// draw the wall and omit the hole
		for (int y = minY; y <= maxY; y++) {
			if (y != hole)
				mazeGrid [x,y] = 1;
			else
				mazeGrid [x, y] = 0;
		}
	}
}
