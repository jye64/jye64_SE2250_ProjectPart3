using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour {

	static public Hero S;

	[Header("Set in Inspector")]
	public float speed = 30;
	public float rollMult = -45;
	public float pitchMult = 30;

	[Header("Set Dynamically")]
	public float shieldLevel = 1;

	void Awake(){
		if (S == null) {
			S = this;
		} else {
			Debug.LogError ("Hero.Awake() - Attempted to assign second Hero.S!");
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float xAxis = Input.GetAxis ("Horizontal");
		float yAsix = Input.GetAxis ("Vertical");

		Vector3 pos = transform.position;
		pos.x += xAxis * speed * Time.deltaTime;
		pos.y += yAsix * speed * Time.deltaTime;
		transform.position = pos;

		transform.rotation = Quaternion.Euler (yAsix * pitchMult, xAxis * rollMult, 0);
		
	}
}
