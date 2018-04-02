using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Enemy_3 goes straight down, shoot towards player, then goes up

public class Enemy_3 : Enemy {

    [Header("Set in Inspector: Enemy_3")]
    public float projectileSpeed = 40;
    public float rotateSpeed = 10f;
    public GameObject projectileEnemy;

    private float timeCounter = 0f;

	void Start(){
		Invoke ("EnemyFire", 2f);
	}
		
	void EnemyFire()
    {
        GameObject gameOB = Instantiate<GameObject>(projectileEnemy);
        gameOB.transform.position = this.gameObject.transform.position;
        gameOB.tag = "ProjectileEnemy";
        gameOB.layer = LayerMask.NameToLayer("ProjectileEnemy");

		GameObject hero = GameObject.Find ("_Hero");
		if(hero!= null){
			Vector3 vel = hero.transform.position -this.transform.position;
			Rigidbody rb = gameOB.GetComponent<Rigidbody>();
			rb.velocity = vel;
		}
		Invoke ("EnemyFire", 2.5f);
    }


    public override void Move()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * rotateSpeed);
        Vector3 tempPos = pos;
        if (timeCounter <= 3f)
        {
            tempPos.y -= speed * Time.deltaTime;
            pos = tempPos;
        }
        else
        {
            tempPos.y += 2*speed * Time.deltaTime;
            pos = tempPos;
        }
        timeCounter += Time.deltaTime;

    }


}


