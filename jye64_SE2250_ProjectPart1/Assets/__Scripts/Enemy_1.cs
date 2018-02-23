using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Enemy_1 extends the Enemy class

public class Enemy_1 : Enemy {

	[HideInInspector]
	public bool direction;

	public float radius = 1f;
	public float camWidth;
	public float camHeight;

	void Awake(){
		camHeight = Camera.main.orthographicSize;
		camWidth = camHeight * Camera.main.aspect;
		bndCheck = GetComponent<BoundsCheck> ();
	}

	void Start(){
		if (Random.Range (0f, 1.0f) < 0.5f) { //random left or right
			direction = true;
		} else {
			direction = false;
		}
	}
		
	public override void Move(){
		
		if (direction == true) {
			transform.Translate(new Vector3(-1*Time.deltaTime*10, 0, 0)); // left
			if (pos.x < -camWidth + radius){
				direction = false; //change to right
			}
		} else {
			transform.Translate(new Vector3(1*Time.deltaTime*10, 0, 0)); // right
			if (pos.x > camWidth -radius){
				direction = true; //change to left
			}
		}
			
		base.Move ();
	}


}
