using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MazeCreatorStatic))]
public class MazeCreatorEditor : Editor {

	public override void OnInspectorGUI()
	{
		//DrawDefaultInspector ();

		MazeCreatorStatic myScript = (MazeCreatorStatic)target;
		myScript.cellsX = EditorGUILayout.IntSlider ("CellsX", myScript.cellsX, 5, 100);
		myScript.cellsY = EditorGUILayout.IntSlider ("CellsY", myScript.cellsY, 5, 100);
		
		myScript.removeDeadEnds = EditorGUILayout.Toggle ("Remove dead ends", myScript.removeDeadEnds);

		myScript.useRandomSeed = EditorGUILayout.Toggle ("Use random seed", myScript.useRandomSeed);

		if (myScript.useRandomSeed) {
			myScript.randomSeed = EditorGUILayout.IntSlider ("Random seed", myScript.randomSeed, 42, 400000);
		}

		if (GUILayout.Button ("Generate Maze")) {
			myScript.makeMaze();
		}
		if (GUILayout.Button ("Delete Maze")) {
			myScript.deleteOldMaze();
		}
	}
}