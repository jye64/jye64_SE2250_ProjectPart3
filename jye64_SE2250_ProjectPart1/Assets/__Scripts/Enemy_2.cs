using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Sine Wave Movement

public class Enemy_2 : Enemy {

	[Header("Set in Inspector")]
	public float waveFrequency =1;
	public float waveWidth = 2;
	public float waveRotY = 45;

	private float x0;
	private float birthTime;

	void Start(){
		x0 = pos.x;
		birthTime=Time.time;
	}

	public override void Move(){
		Vector3 tempPos = pos;
		float age = Time.time -birthTime;
		float theta = Mathf.PI*2*age/waveFrequency;
		float sin = Mathf.Sin(theta);
		tempPos.x = x0 + waveWidth * sin;
		pos = tempPos;

		Vector3 rot = new Vector3(0,sin*waveRotY,0);
		this.transform.rotation = Quaternion.Euler(rot);
		base.Move();
	}
}
