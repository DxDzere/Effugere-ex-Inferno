using UnityEngine;
using System.Collections;

// each AI has a red "Cap"
// which blinks in the minimap view

public class blink : MonoBehaviour {

	// the blinking is done via a timer
	// and usage of Math.sin
	float elapsedTime;
	float speed = 8f;

	Color choiceColor;


	// Use this for initialization
	void Start () {
		choiceColor = GetComponent<Renderer>().material.color;
	}


	// Update is called once per frame
	//
	// This method produces the magic and displays a blinking rectangle
	//
	void Update () {
		elapsedTime += Time.deltaTime*speed;
		elapsedTime = elapsedTime % (Mathf.PI*2f);
		float alpha = Mathf.Sin(elapsedTime)*0.5f + 0.5f;
		choiceColor.a = alpha;
		GetComponent<Renderer>().material.color = choiceColor; 
	}
}
