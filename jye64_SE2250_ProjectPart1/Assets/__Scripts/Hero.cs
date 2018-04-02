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
	public Weapon[] weapons;           
	public GameObject heroExplosion;

	[Header("Set Dynamically")]
	[SerializeField]
	private float _shieldLevel = 1;

	private GameObject lastTriggerGo = null;

	public delegate void WeaponFireDelegate ();

	public WeaponFireDelegate fireDelegate;

	void Start(){
		if (S == null) {
			S = this;
		} else {
			Debug.LogError ("Hero.Awake() - Attempted to assign second Hero.S!");
		}
		ClearWeapons ();
		weapons [0].SetType (WeaponType.simple);
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
		} else if (go.tag == "PowerUp"){
			AbsorbPowerUp (go);          
		} else if (go.tag == "ProjectileEnemy"){
			shieldLevel--;
            Destroy(go);
		} else {
			print ("Triggered by non-Enemy: " + go.name);
		}
	}

	public void AbsorbPowerUp(GameObject go){      
		PowerUp pu = go.GetComponent<PowerUp> ();
		Debug.Log (pu.type);     
		switch (pu.type) {

		case WeaponType.shield:
			shieldLevel++;
			break;
        case WeaponType.nuke:
                int BombCount = Main.S.getBombCount();
                BombCount += 1;
            Main.S.setBombCountText(BombCount);
            pu.SetType(WeaponType.simple);
            SwitchWeapons(pu.type);
            break;
        default:
			if (pu.type == weapons [0].type) {      // if it's the same weapon type
				Weapon w = GetEmptyWeaponSlot ();
				if (w != null) {
					w.SetType (pu.type);
				}	
			} else{//different weapon type, switch to same level
               SwitchWeapons (pu.type);
			}
			break;
		  
		} // end switch

		pu.AbsorbedBy (this.gameObject);
	}

	public float shieldLevel{
		get{ 
			return (_shieldLevel);
		}
		set{ 
			_shieldLevel = Mathf.Min (value, 4);
			if (value < 0) {
				Destroy (this.gameObject);
				Instantiate (heroExplosion, transform.position, transform.rotation);
				Main.S.DelayedRestart (gameRestartDelay);
				Main.S.setHighScoreText ();   
			}
		}
	}

	Weapon GetEmptyWeaponSlot(){
		for (int i = 0; i < weapons.Length; i++) {
			if (weapons [i].type == WeaponType.none) {
				return (weapons [i]);
			}
		}
		return (null);
	}

	void SwitchWeapons(WeaponType type){
		foreach (Weapon w in weapons) {
			if (w.type == WeaponType.none){
				//for unfilled weapon slots, do nothing
			}else{
				w.SetType (type);
			}
		}
	}

	void ClearWeapons(){
		foreach(Weapon w in weapons){
			w.SetType (WeaponType.none);
		}
	}
		

}
