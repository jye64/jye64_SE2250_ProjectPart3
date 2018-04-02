using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour {

	private Slider energyBar;
	public float currentHP;

	// Use this for initialization
	void Start () {

	}

	void Awake(){
		energyBar = GetComponent<Slider> ();
		currentHP = 0;
	}

	// Update is called once per frame
	void Update () {
		energyBar.value = currentHP;
	}

	public void changeHP(float hp){
		currentHP += hp;
	}
		
}
