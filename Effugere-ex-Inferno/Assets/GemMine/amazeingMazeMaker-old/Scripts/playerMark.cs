using UnityEngine;
using System.Collections;

public class playerMark : MonoBehaviour {
		
	public GameObject mark;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void LateUpdate() {
		Quaternion rotation = mark.transform.rotation;
		rotation = Quaternion.Euler(new Vector3(90,this.transform.rotation.y,0));
		mark.transform.rotation = rotation;
	}
}
