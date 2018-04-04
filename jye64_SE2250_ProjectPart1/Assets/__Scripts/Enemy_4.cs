using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_4 : Enemy
{

	[Header("Set in Inspector: Enemy_4")]
	public float projectileSpeed = 40;
	public GameObject projectilePrefab;
	private float timer;
	public float horizontalSpeed = 10f;

	private bool direction;
	private float camWidth;
	private float camHeight;
	private float radius = 2f;

	//boss weapon types
	private string[] weaponDefinition = {
		"blaster", "track"
	};

	public override void Awake()
	{
		camHeight = Camera.main.orthographicSize;
		camWidth = camHeight * Camera.main.aspect;
		bndCheck = GetComponent<BoundsCheck>();
		base.Awake();
		timer = 0;
		Invoke("EnemyFire", 2f);
	}

	// Use this for initialization
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

	void EnemyFire()
	{
		int choice = Random.Range(0, weaponDefinition.Length);
		switch (choice)
		{
		case 0:
			Blaster();
			break;

		case 1:
			Track();
			break;

		}

		Invoke("EnemyFire", 2.5f);
	}

	public override void Move()
	{
		if (direction == true)
		{
			transform.Translate(new Vector3(-1 * Time.deltaTime * horizontalSpeed, 0, 0)); // left
			if (pos.x < -camWidth + radius + 3.5)
			{
				direction = false; //when hits the scene boundary, change direction
			}
		}
		else
		{
			transform.Translate(new Vector3(1 * Time.deltaTime * horizontalSpeed, 0, 0)); // right
			if (pos.x > camWidth - radius - 3.5)
			{
				direction = true; //when hits the scene boundary, change direction
			}
		}
		if (timer < 1.5f)
		{
			base.Move();
		}
		timer += Time.deltaTime;
	}

	void Blaster()
	{
		GameObject[] goes = new GameObject[10];
		for (int i = 0; i < 10; i++)
		{
			goes[i] = Instantiate<GameObject>(projectilePrefab);
		}
		for (int i = 0; i < 10; i++)
		{
			if (i <= 4)
			{
				goes[i].transform.position += this.transform.position + new Vector3(2, 0, 0) * i;
			}
			else
			{
				goes[i].transform.position += this.transform.position + new Vector3(-2, 0, 0) * i;
			}
			goes[i].tag = "ProjectileEnemy";
			goes[i].layer = LayerMask.NameToLayer("ProjectileEnemy");
			Rigidbody rigidB = goes[i].GetComponent<Rigidbody>();
			rigidB.velocity = Vector3.down * projectileSpeed;
		}
	}

	void Track()
	{
		GameObject[] goes = new GameObject[10];
		for (int i = 0; i < 10; i++)
		{
			goes[i] = Instantiate(projectilePrefab);
		}
		for (int i = 0; i < 10; i++)
		{
			if (i < 10)
			{
				goes[i].transform.position = this.gameObject.transform.position + new Vector3(4, 2, 0) * i;
			}
			else
			{
				goes[i].transform.position = this.gameObject.transform.position + new Vector3(4, 2, 0) * i;
			}
			goes[i].tag = "ProjectileEnemy";
			goes[i].layer = LayerMask.NameToLayer("ProjectileEnemy");
			GameObject hero = GameObject.Find("_Hero");
			if (hero != null)
			{
				Vector3 vel = hero.transform.position - this.transform.position;
				Rigidbody rb = goes[i].GetComponent<Rigidbody>();
				rb.velocity = vel;
			}
		}

	}
}