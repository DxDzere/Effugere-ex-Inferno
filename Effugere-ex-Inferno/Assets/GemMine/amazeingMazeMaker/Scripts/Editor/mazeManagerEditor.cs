using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor (typeof(mazeManager))]
public class mazeManagerEditor : Editor {

	Texture2D logoTexture;

	SerializedProperty mazeWidth;
	SerializedProperty mazeLength;
	SerializedProperty mazeHeight;
	SerializedProperty wallWidth;
	SerializedProperty tileSize;
	SerializedProperty seed;
	SerializedProperty mazeElement;
	SerializedProperty pathElement;
	SerializedProperty floorElement;
	SerializedProperty makeEntrance;
	SerializedProperty makeExit;

	SerializedProperty cell00;
	SerializedProperty cell01;
	SerializedProperty cell02;
	SerializedProperty cell03;
	SerializedProperty cell04;
	SerializedProperty cell05;
	SerializedProperty cell06;
	SerializedProperty cell07;
	SerializedProperty cell08;
	SerializedProperty cell09;
	SerializedProperty cell10;
	SerializedProperty cell11;
	SerializedProperty cell12;
	SerializedProperty cell13;
	SerializedProperty cell14;
	SerializedProperty cell15;

	public int selectedAlgo = 0;
	public bool standardConfig = true;

	void OnEnable()	{
		logoTexture = Resources.Load ("gemmine-logo", typeof(Texture2D)) as Texture2D;
		mazeElement = serializedObject.FindProperty ("mazeElement");
		pathElement = serializedObject.FindProperty ("pathElement");
		floorElement = serializedObject.FindProperty ("floorElement");
		mazeWidth = serializedObject.FindProperty("mazeWidth");
		mazeLength = serializedObject.FindProperty("mazeLength");
		mazeHeight = serializedObject.FindProperty("mazeHeight");
		wallWidth = serializedObject.FindProperty("wallWidth");
		tileSize = serializedObject.FindProperty("tileSize");
		makeEntrance = serializedObject.FindProperty("makeEntrance");
		makeExit = serializedObject.FindProperty("makeExit");
		seed = serializedObject.FindProperty("seed");

		cell00 = serializedObject.FindProperty ("cell00");
		cell01 = serializedObject.FindProperty ("cell01");
		cell02 = serializedObject.FindProperty ("cell02");
		cell03 = serializedObject.FindProperty ("cell03");
		cell04 = serializedObject.FindProperty ("cell04");
		cell05 = serializedObject.FindProperty ("cell05");
		cell06 = serializedObject.FindProperty ("cell06");
		cell07 = serializedObject.FindProperty ("cell07");
		cell08 = serializedObject.FindProperty ("cell08");
		cell09 = serializedObject.FindProperty ("cell09");
		cell10 = serializedObject.FindProperty ("cell10");
		cell11 = serializedObject.FindProperty ("cell11");
		cell12 = serializedObject.FindProperty ("cell12");
		cell13 = serializedObject.FindProperty ("cell13");
		cell14 = serializedObject.FindProperty ("cell14");
		cell15 = serializedObject.FindProperty ("cell15");
	}


	public override void OnInspectorGUI() {
		GUILayout.Space (10);

		if (logoTexture != null) {
			Rect rect = GUILayoutUtility.GetRect (logoTexture.width, logoTexture.height);
			GUI.DrawTexture (rect, logoTexture, ScaleMode.ScaleToFit);
		}

		GUILayout.Space (10);

		GUILayout.BeginVertical ("Box");
		GUILayout.Label ("Maze Configuration", EditorStyles.boldLabel);
		serializedObject.Update();

		standardConfig = EditorGUILayout.Toggle ("Standard Config", standardConfig);
		if (standardConfig) {
			EditorGUILayout.PropertyField (mazeElement);
			EditorGUILayout.PropertyField (pathElement);
			EditorGUILayout.PropertyField (floorElement);
		} else {
			EditorGUILayout.PropertyField (cell00);
			EditorGUILayout.PropertyField (cell01);
			EditorGUILayout.PropertyField (cell02);
			EditorGUILayout.PropertyField (cell03);
			EditorGUILayout.PropertyField (cell04);
			EditorGUILayout.PropertyField (cell05);
			EditorGUILayout.PropertyField (cell06);
			EditorGUILayout.PropertyField (cell07);
			EditorGUILayout.PropertyField (cell08);
			EditorGUILayout.PropertyField (cell09);
			EditorGUILayout.PropertyField (cell10);
			EditorGUILayout.PropertyField (cell11);
			EditorGUILayout.PropertyField (cell12);
			EditorGUILayout.PropertyField (cell13);
			EditorGUILayout.PropertyField (cell14);
			EditorGUILayout.PropertyField (cell15);				
		}

			GUILayout.Space (10);
			EditorGUILayout.PropertyField (mazeWidth);
			EditorGUILayout.PropertyField (mazeLength);
			EditorGUILayout.PropertyField (mazeHeight);
			EditorGUILayout.PropertyField (wallWidth);
			EditorGUILayout.PropertyField (tileSize);
			EditorGUILayout.PropertyField (makeEntrance);
			EditorGUILayout.PropertyField (makeExit);
			EditorGUILayout.PropertyField (seed);

		GUILayout.Space (10);

		serializedObject.ApplyModifiedProperties();
		GUILayout.Space (10);

		GUILayout.Label ("Choose maze algorithm", EditorStyles.boldLabel);
		var text = new string[] { "Binary Tree", "Recursive Backtracker", "Recursive Divison", "SideWinder" };
		selectedAlgo =  GUILayout.SelectionGrid (selectedAlgo, text, 1, EditorStyles.radioButton);

		mazeManager myScript = (mazeManager) target;
		if (GUILayout.Button ("Build Maze")) {
				myScript.createMaze (selectedAlgo, standardConfig);
		}

		if (GUILayout.Button ("Solve Maze")) {
			myScript.solveMaze (standardConfig);
		}

		GUILayout.EndVertical ();
	}
	
}
#endif