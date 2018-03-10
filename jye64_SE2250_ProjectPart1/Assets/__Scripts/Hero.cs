using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour {

	static public Hero S;

	[Header("Set in Inspector")]
	public float speed = 30;
	public float rollMult = -45;
	public float pitchMult = 30;
	public float gameRestartDelay = 2f;
	public GameObject projectilePrefab;
	public float projectileSpeed = 40;

	[Header("Set Dynamically")]
	[SerializeField]
	private float _shieldLevel = 1;

	private GameObject lastTriggerGo = null;


	public delegate void WeaponFireDelegate ();

	public WeaponFireDelegate fireDelegate;

	void Awake(){
		if (S == null) {
			S = this;
		} else {
			Debug.LogError ("Hero.Awake() - Attempted to assign second Hero.S!");
		}
		fireDelegate += TempFire;
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

		if (Input.GetAxis("Jump") == 1 && fireDelegate !=null){
			fireDelegate();
		}
	}


	void TempFire(){
		GameObject[] gameOB = new GameObject[3];
		for (int i = 0; i <= 2; i++) {
			gameOB [i] = Instantiate<GameObject> (projectilePrefab);
			gameOB [i].transform.position = transform.position;
		}
		Rigidbody rigidB1 = gameOB [0].GetComponent<Rigidbody> ();
		Rigidbody rigidB2 = gameOB [1].GetComponent<Rigidbody> ();
		Rigidbody rigidB3 = gameOB [2].GetComponent<Rigidbody> ();

//		rigidB1.velocity = Vector3.up * projectileSpeed;
//		rigidB2.velocity = new Vector3 (0.5f, 1, 0) * projectileSpeed;
//		rigidB3.velocity = new Vector3 (-0.5f, 1, 0) * projectileSpeed;

		Projectile proj1 = gameOB [0].GetComponent<Projectile> ();
		Projectile proj2 = gameOB [1].GetComponent<Projectile> ();
		Projectile proj3 = gameOB [2].GetComponent<Projectile> ();

		proj1.type = WeaponType.blaster;
		proj2.type = WeaponType.blaster;
		proj3.type = WeaponType.blaster;

		float tSpeed = Main.GetWeaponDefinition (proj1.type).velocity;

		rigidB1.velocity = Vector3.up * tSpeed;
		rigidB2.velocity = new Vector3 (0.5f, 1, 0) * tSpeed;
		rigidB3.velocity = new Vector3 (-0.5f, 1, 0) * tSpeed;


	}

	void OnTriggerEnter(Collider other){
		Transform rootT = other.gameObject.transform.root;
		GameObject go = rootT.gameObject;

		if (go == lastTriggerGo) {
			return;
		}
		lastTriggerGo = go;

		if (go.tag == "Enemy") {
			shieldLevel--;
			Destroy (go);
		} else {
			print ("Triggered by non-Enemy: " + go.name);
		}
	}

	public float shieldLevel{
		get{ 
			return (_shieldLevel);
		}
		set{ 
			_shieldLevel = Mathf.Min (value, 4);
			if (value < 0) {
				Destroy (this.gameObject);
				Main.S.DelayedRestart (gameRestartDelay);
			}
		}
	}

}
