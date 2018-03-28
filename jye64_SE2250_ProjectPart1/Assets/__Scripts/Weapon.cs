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
	simple,      //simply creating bullets, all moving upwards
	blaster,     //a simple blaster, three bullets, one upward, one to left 30 ,one to right 30
	spread,      //two shots simultaneously
	phaser,      //[NT] shots that move in waves
	missile,     //[NT] homing missiles
	laser, //[NT] damage over time
    nuke,
	shield,//raise shieldLevel

}

/// <summary>
/// The WeapoonDefinition class allows you to set the properties
/// of a specific weapon in the Inspector. The Main class has 
/// an array of WeaponDefinitions that makes this possible.
/// </summary>

[System.Serializable]
public class WeaponDefinition{
	public WeaponType    type = WeaponType.none;
	public string        letter;                              //letter to show on the PowerUp
	public Color         color = Color.white;                 //Color of Collar & PowerUp
	public GameObject    projectileprefab;                    //prefab for projectiles
	public Color         projectileColor = Color.white;
	public float         damageOnHit = 0;
	public float         continuousDamage = 0;                // damage per second(Laser)
	public float         delayBetweenShots = 0;
	public float         velocity = 20;                       //speed of projectiles
  
} 


public class Weapon : MonoBehaviour {

	static public Transform PROJECTILE_ANCHOR;

	[Header("Set Dynamically")]  [SerializeField]
	private WeaponType _type = WeaponType.none;
	public WeaponDefinition def;
	public GameObject collar;
	public float lastShotTime;
	private Renderer collarRend;

	[Header("Set in Inspector: Explosion")]
	public float radius = 5.0f;
	public float power = 10.0f;
    public GameObject explosions;

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
			rootGO.GetComponent<Hero>().fireDelegate += Fire;     //add Fire to fireDelegate
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

		switch (type) {

		case WeaponType.simple:      //bullets move upwards
			p = MakeProjectile ();
			p.rigid.velocity = vel;
			break;
			 
		case WeaponType.blaster:      //three bullets, one straight up, the other two 30 degrees to left&right
			p = MakeProjectile();
			p.rigid.velocity = vel;
			p  = MakeProjectile();
			p.transform.rotation = Quaternion.AngleAxis(30, Vector3.back);
			p.rigid.velocity = p.transform.rotation*vel;
			p = MakeProjectile();
			p.transform.rotation = Quaternion.AngleAxis(-30, Vector3.back);
			p.rigid.velocity = p.transform.rotation*vel;
			break;
		
		case WeaponType.laser:

			break;

		case WeaponType.missile:

			break;
       case WeaponType.nuke:
                GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
                for (int i = 0; i < gameObjects.Length; i++)
                {
                    Enemy x;
                    Enemy_1 y;
                    x = new Enemy();
                    y = new Enemy_1();
                    if (gameObjects[i].name == "Enemy_0(Clone)")
                    { 
                        Main.S.setScoreText(x.score);
                    }
                    if (gameObjects[i].name == "Enemy_1(Clone)")
                    {
                        Main.S.setScoreText(y.score);
                    }
                    Instantiate(explosions, gameObjects[i].transform.position, gameObjects[i].transform.rotation);
                    Destroy(gameObjects[i]);
                }
                break;
        } // end switch

	}

	public Projectile MakeProjectile(){
		GameObject go = Instantiate<GameObject> (def.projectileprefab);
		if (transform.parent.gameObject.tag == "Hero") {
			go.tag = "ProjectileHero";
			go.layer = LayerMask.NameToLayer ("ProjectileHero");
		} else {
			go.tag = "ProjectileEnemy";
			go.layer = LayerMask.NameToLayer("ProjectileEnemy");
		}
		go.transform.position = collar.transform.position;
		go.transform.SetParent(PROJECTILE_ANCHOR, true);
		Projectile p = go.GetComponent<Projectile>();
		p.type = type;
		lastShotTime =Time.time;
		return(p);
	}
		
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Alpha1)){
			type = WeaponType.simple;
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			type = WeaponType.blaster;
		}
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            type = WeaponType.missile;
        }
         if (Input.GetKeyDown(KeyCode.E))
        {
             type = WeaponType.nuke;
        }
    }

//	public GameObject FindClosestEnemy(){
//		GameObject[] gos;
//		gos = GameObject.FindGameObjectsWithTag("Enemy");
//		GameObject closest = null;
//		float distance2 = Mathf.Infinity;
//		float distance = 15;
//		Vector3 position = transform.position;
//		foreach (GameObject go in gos) {
//			Vector3 diff = go.transform.position - position;
//			float curDistance = diff.sqrMagnitude;
//			if (curDistance < distance) {
//				closest = go;
//				distance = curDistance;
//				//target = go;
//
//			}
//		}
//
//		return closest;
//	}
//

}

