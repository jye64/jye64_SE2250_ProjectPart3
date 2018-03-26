using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy_1 : Enemy {

	[HideInInspector]
	public bool direction;

	[Header("Set in Inspector: Enemy_1")]
	public float projectileSpeed = 40;
	private float radius = 1f;
	private float camWidth;
	private float camHeight;
	public float enemyShootingInterval = 2.0f;
	public GameObject projectilePrefab;   //holding Enemy projectile  
	public Weapon[] weapons;  
  
	public override void Awake(){
		camHeight = Camera.main.orthographicSize;
		camWidth = camHeight * Camera.main.aspect;
		bndCheck = GetComponent<BoundsCheck> (); 
		base.Awake ();
	}

//	void EnemyFire(){
//		GameObject projGO = Instantiate<GameObject>(projectilePrefab);
//		projGO.transform.position = transform.position;
//		Rigidbody rigidB = projGO.GetComponent<Rigidbody> ();
//		Vector3 heroPosition = Hero.S.transform.position;
//		rigidB.velocity = heroPosition * projectileSpeed;
//	}

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
			if (pos.x < -camWidth + radius+3){
				direction = false; //when hits the scene boundary, change direction
			}
		} else {
			transform.Translate(new Vector3(1*Time.deltaTime*10, 0, 0)); // right
			if (pos.x > camWidth -radius-3){
				direction = true; //when hits the scene boundary, change direction
			}
		}
			
		base.Move ();
	}

}
