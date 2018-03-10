using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is an enum of the various possible weapon types.
/// It also includes a "shield" type to allow a shield power-up
/// Items marked [NT] are Not Implemented in the IGDPD book.
/// </summary>


public enum WeaponType{
	none,        //default
	blaster,     //a simple blaster
	spread,      //two shots simultaneously
	phaser,      //[NT] shots that move in waves
	missile,     //[NT] homing missiles
	laser,       //[NT] damage over time
	shield       //raise shieldLevel
}

/// <summary>
/// The WeapoonDefinition class allows you to set the properties
/// of a specific weapon in the Inspector. The Main class has 
/// an array of WeaponDefinitions that makes this possible.
/// </summary>

[System.Serializable]
public class WeaponDefinition{
	public WeaponType    type = WeaponType.none;
	public string        letter;
	public Color         color = Color.white;
	public GameObject    projectileprefab;
	public Color         projectileColor = Color.white;
	public float         damegeOnHit = 0;
	public float         continuousDamege = 0;
	public float         delayBetweenShots = 0;
	public float         velocity = 20;
}


public class Weapon : MonoBehaviour {

	static public Transform PROJECTILE_ANCHOR;

	[Header("Set Dynamically")]  [SerializeField]
	private WeaponType _type = WeaponType.none;
	public WeaponDefinition def;
	public GameObject collar;
	public float lastShotTime;
	private Renderer collarRend;


	// Use this for initialization
	void Start () {
		collar = transform.Find ("Collar").gameObject;
		collarRend = collar.GetComponent<Renderer> ();

		SetType (_type);

		if (PROJECTILE_ANCHOR == null) {
			GameObject go = new GameObject ("_ProjectileAnchor");
			PROJECTILE_ANCHOR = go.transform;
		}

		GameObject rootGO = transform.root.gameObject;
		if (rootGO.GetComponent<Hero> () != null) {
			rootGO.GetComponent<Hero>().fireDelegate += Fire;
		}
	}

	public WeaponType type{
		get{ return (_type); }
		set{ SetType (value); }
	}

	public void SetType(WeaponType wt){
		_type = wt;
		if (type == WeaponType.none) {
			this.gameObject.SetActive (false);
			return;
		} else {
			this.gameObject.SetActive (true);
		}
		def = Main.GetWeaponDefinition (_type);
		collarRend.material.color = def.color;
		lastShotTime = 0;
	}

	public void Fire(){
		if (!gameObject.activeInHierarchy)
			return;
		if (Time.time - lastShotTime < def.delayBetweenShots) {
			return;
		}
		Projectile p;
		Vector3 vel = Vector3.up * def.velocity;
		if (transform.up.y < 0) {
			vel.y = -vel.y;
		}
//
//		switch (type) {
//
//		case WeaponType.blaster;
//			p = MakeProjectile();
//			p.rigid.velocity = vel;
//			break;
//
//		case WeaponType.spread;
//			p = MakeProjectiel();
//			p.rigid.velocity = vel;
//
//
//		
//		}


	}

//	public Projectile MakeProjectiel(){
//		
//	}
		
	
	// Update is called once per frame
	void Update () {
		
	}
}
