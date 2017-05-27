using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimControler_Player : MonoBehaviour {
	private Animator anim;
	private float walking;
    private float turning;
    private float backward;

    public int turnSpeed;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		walking = 0.0f;
        turning = 0.0f;
        backward = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		walking = Input.GetAxis ("Vertical");
		anim.SetFloat ("walking", walking);
        turning = Input.GetAxis("Horizontal");
        transform.Rotate(new Vector3(0.0f, turnSpeed*turning*Time.deltaTime));
        backward = Input.GetAxis("Vertical");
        anim.SetFloat("backward", backward);
	}

}
