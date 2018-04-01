using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

	private Slider healthBar;
	private float currentHP = 10;

	// Use this for initialization
	void Start () {
		
	}

	void Awake(){
		healthBar = GetComponent<Slider> ();
	}
	
	// Update is called once per frame
	void Update () {
		healthBar.value = currentHP;
	}

	public void changeHP(float hp){
		currentHP = hp;
	}
}
