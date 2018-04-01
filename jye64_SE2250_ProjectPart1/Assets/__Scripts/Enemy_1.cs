using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Enemy_1 goes randomly left or right, reverse direction when hits the boundary

public class Enemy_1 : Enemy {

	[Header("Set in Inspector: Enemy_1")]
	public float projectileSpeed = 40;
	private float radius = 1f;
	private float camWidth;
	private float camHeight;
    public float enemyShootingInterval = 0.5f;
	public GameObject projectilePrefab;   //holding Enemy projectile  
<<<<<<< HEAD
    public Weapon[] weapons;

   


    public override void Awake(){
=======
	public Weapon[] weapons;

	private bool direction;
  
	public override void Awake(){
>>>>>>> 66ba6d3b076bd938289a81f13ddba82e3506d8b7
		camHeight = Camera.main.orthographicSize;
		camWidth = camHeight * Camera.main.aspect;
		bndCheck = GetComponent<BoundsCheck> (); 
		base.Awake ();
	}

<<<<<<< HEAD
=======
	//to do
	void EnemyFire(){
		GameObject projGO = Instantiate<GameObject>(projectilePrefab);
		projGO.transform.position = transform.position;
		Rigidbody rigidB = projGO.GetComponent<Rigidbody> ();
		rigidB.velocity = Vector3.down * projectileSpeed;
	}
>>>>>>> 66ba6d3b076bd938289a81f13ddba82e3506d8b7


    void Start(){
		if (Random.Range (0f, 1.0f) < 0.5f) { //random left or right
			direction = true;
		} else {
			direction = false;
		}
        InvokeRepeating("Enemyfire", enemyShootingInterval , fireRate);
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
<<<<<<< HEAD
    void Enemyfire()
    {
        GameObject projGo = Instantiate<GameObject>(projectilePrefab);
        projGo.transform.position = transform.position;
        Rigidbody rigidB = projGo.GetComponent<Rigidbody>();
        rigidB.velocity = Vector3.down * projectileSpeed;
    }
=======
		
>>>>>>> 66ba6d3b076bd938289a81f13ddba82e3506d8b7

}
