using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Sine Wave Movement, shoot three bullets after 3 seconds

public class Enemy_2 : Enemy {

	[Header("Set in Inspector: Enemy_2")]
	public float projectileSpeed = 40;
	public float waveFrequency =1;
	public float waveWidth = 2;
	public float waveRotY = 45;
	public GameObject projectilePrefab;

	private float x0;
	private float birthTime;

	void Start(){
		x0 = pos.x;
		birthTime=Time.time;
		Invoke ("EnemyFire", 3f);
	}

	void EnemyFire(){
		GameObject[] gameOB = new GameObject[3];
		for (int i = 0; i <= 2; i++) {
			gameOB [i] = Instantiate<GameObject> (projectilePrefab);
			gameOB [i].transform.position = transform.position;
		}
		Rigidbody rigidB1 = gameOB [0].GetComponent<Rigidbody> ();
		Rigidbody rigidB2 = gameOB [1].GetComponent<Rigidbody> ();
		Rigidbody rigidB3 = gameOB [2].GetComponent<Rigidbody> ();

		rigidB1.velocity = Vector3.down * projectileSpeed;
		rigidB2.velocity = new Vector3 (-0.5f, -1, 0) * projectileSpeed/2;
		rigidB3.velocity = new Vector3 (0.5f, -1, 0) * projectileSpeed/2;
		
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
