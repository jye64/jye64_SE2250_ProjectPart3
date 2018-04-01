﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsCheck : MonoBehaviour {

	/// <summary>
	/// Keep a GameObject on screen.
	/// Note tha tthis ONLY works for an orthographic Main Camera at [0,0,0].
	/// </summary>

	[Header("Set in Inspector")]
	public float radius = 1f;
	public bool keepOnScreen = true;

    [Header("Set Dynamically")]
    public bool isOnScreen = true;
	public float camWidth;
	public float camHeight;

	[HideInInspector]
	public bool offRight, offLeft, offUp, offDown;

	void Awake(){
		camHeight = Camera.main.orthographicSize;     
		camWidth  = camHeight * Camera.main.aspect;
	}
		

	void LateUpdate(){
		Vector3 pos = transform.position;
		isOnScreen = true;
		offRight = offLeft = offUp = offDown = false;

		if (pos.x > camWidth - radius) {
			pos.x = camWidth - radius;
			offRight = true;
		}

		if (pos.x < -camWidth + radius) {
			pos.x = -camWidth + radius;
			offLeft = true;
		}

		if (pos.y > camHeight - radius) {
			pos.y = camHeight - radius;
			offUp = true;
		}

		if (pos.y < -camHeight + radius) {
			pos.y = -camHeight + radius;
			offDown = true;
		}

		isOnScreen = !(offRight || offLeft || offUp || offDown);
		if (keepOnScreen && !isOnScreen) {
			transform.position = pos;
			isOnScreen = true;
			offRight = offLeft = offUp = offDown = false;
		}
			
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//draw the bounds in the scene pane using OnDrawGizmos()
	void OnDrawGizmos(){
		if (!Application.isPlaying)
			return;
		Vector3 boundsize = new Vector3 (camWidth * 2, camHeight * 2, 0.1f);
		Gizmos.DrawWireCube (Vector3.zero, boundsize);
	}
}
