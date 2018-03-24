using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	[Header("Set in Inspector: Enemy")]
	public float speed = 10f;
	public float fireRate = 0.3f;
	public float health = 10;
	public int score = 100;    
	public float showDamageDuration = 0.1f;
	public float powerUpDropChance = 1f;

	[Header("Set Dynamically: Enemy")]
	public Color[] originalColors;
	public Material[] materials;
	public bool showingDamage = false;
	public float damageDoneTime;  //time to stop showing damage
	public bool notifiedOdDestruction = false;

	protected BoundsCheck bndCheck;

	public virtual void Awake(){
		bndCheck = GetComponent<BoundsCheck> ();

		//Get materials and colors for this GameObject and its children
		materials = Utils.GetAllMaterials(gameObject);
		originalColors = new Color[materials.Length];
		for (int i = 0; i < materials.Length; i++) {
			originalColors [i] = materials [i].color;
		}
	}

	public Vector3 pos{
		get{ 
			return (this.transform.position);
		}
		set{ 
			this.transform.position = value;
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Move ();

		if (showingDamage && Time.time > damageDoneTime) {
			UnShowDamage ();
		}
		 //check to make sure it's gone off the bottom of the screen
		if (bndCheck != null && bndCheck.offDown) {
			if (pos.y < bndCheck.camHeight - bndCheck.radius) {
				Destroy (gameObject);
			}
		}
	}

	public virtual void Move(){
		Vector3 tempPos = pos;
		tempPos.y -= speed*Time.deltaTime;
		pos = tempPos;
	}
		

	void OnCollisionEnter(Collision coll){
		GameObject otherGO = coll.gameObject;
		switch (otherGO.tag) {

		case "ProjectileHero":
			Projectile p = otherGO.GetComponent<Projectile> ();
			if (!bndCheck.isOnScreen) {
				Destroy (otherGO);
				break;
			}
			health -= Main.GetWeaponDefinition (p.type).damageOnHit;
			if (health <= 0) {
				if (!notifiedOdDestruction){
					Main.S.ShipDestroyed (this);
				}
				notifiedOdDestruction = true;
				Destroy (this.gameObject);
				Main.S.setScoreText (score);    //when enemy get destroyed, add score to scoreCounter
			}
			ShowDamage ();   
			Destroy (otherGO);
			break;

		default:
			print ("Enemy hit by non-ProjectileHero:" + otherGO.name);
			break;

		}
		
	}

	void ShowDamage(){
		foreach (Material m in materials) {
			m.color = Color.red;
		}
		showingDamage = true;
		damageDoneTime = Time.time + showDamageDuration;
	}

	void UnShowDamage(){
		for (int i = 0; i < materials.Length; i++) {
			materials [i].color = originalColors [i];
		}
		showingDamage = false;
	}

}
