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
    public GameObject projectilePrefab;   //holding Enemy projectile 

    private bool direction;    

    public override void Awake()
    {
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;
        bndCheck = GetComponent<BoundsCheck>();
        base.Awake();
        Invoke("EnemyFire", 2f);
    }

    //can implement shoot towards hero
    void EnemyFire()
    {
        GameObject go = Instantiate<GameObject>(projectilePrefab);
        go.transform.position = this.gameObject.transform.position;
        go.tag = "ProjectileEnemy";
        go.layer = LayerMask.NameToLayer("ProjectileEnemy");

        Rigidbody rigidB = go.GetComponent<Rigidbody>();
        rigidB.velocity = Vector3.down * projectileSpeed;
        Invoke("EnemyFire", 1.5f);
    }

    void Start()
    {
        if (Random.Range(0f, 1.0f) < 0.5f)
        { //random left or right
            direction = true;
        }
        else
        {
            direction = false;
        }
    }

    public override void Move()
    {
        if (direction == true) {
            transform.Translate(new Vector3(-1 * Time.deltaTime * 10, 0, 0)); // left
            if (pos.x < -camWidth + radius + 3)
            {
                direction = false; //when hits the scene boundary, change direction
            }
        } else {
            transform.Translate(new Vector3(1 * Time.deltaTime * 10, 0, 0)); // right
            if (pos.x > camWidth - radius - 3)
            {
                direction = true; //when hits the scene boundary, change direction
            }
        }
        base.Move();
    }


}
