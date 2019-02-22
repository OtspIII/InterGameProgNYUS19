using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{

	public Rigidbody2D RB;
	public ParticleSystem Particles;
	public float Speed;
	public float JumpPower;
	public List<GameObject> Touching;
	public static PlayerController Singleton;

	public AudioSource AS;
	public AudioClip Hop;
	public SpriteRenderer SR;
	public Transform Body;
	public Collider2D Collider;

	private float JumpTime;
	private float CoyoteTime;


	void Start()
	{
		PlayerController.Singleton = this;
		//Find our rigidbody and audio right at the start
		RB = GetComponent<Rigidbody2D>();
		AS = GetComponent<AudioSource>();
		Collider = GetComponent<Collider2D>();
	}


	void Update()
	{
		Inputs();
	}

	void Inputs()
	{
		//Pull out our old velocity so we can modify it
		Vector3 vel = RB.velocity;
		float xDesire;
		//If we're hitting keys we move in that direction
		if (Input.GetKey(KeyCode.RightArrow))
		{
//			vel.x = Speed;
			xDesire = Speed;
			if (vel.x < 0)
				vel.x = 0;
		}
		else if (Input.GetKey(KeyCode.LeftArrow))
		{
//			vel.x = -Speed;
			xDesire = -Speed;
			if (vel.x > 0)
				vel.x = 0;
		}
		else //If we're not hitting keys, come to stop
		{
//			vel.x = 0;
			xDesire = 0;
		}

		vel.x = Mathf.Lerp(vel.x, xDesire, 0.3f);

		if (OnGround())
		{
			JumpTime = 0;
			CoyoteTime = 0;
		}
		else
		{
			CoyoteTime += Time.deltaTime;
			JumpTime += Time.deltaTime;
			if (!Input.GetKey(KeyCode.UpArrow))
				JumpTime = 999;
		}

		//Jump, but only if you're touching the ground
		if (Input.GetKey(KeyCode.UpArrow) && (OnGround() || JumpTime < 0.3f || CoyoteTime < 0.15f))
		{
//			RB.AddForce(new Vector2(0,10),ForceMode2D.Impulse);
//			transform.position += new Vector3(0,0.2f,0);
			vel.y = JumpPower;
			CoyoteTime = 999;
			//AS.PlayOneShot(Hop);
//			Particles.Emit(10);
		}

		float move = Mathf.Min(0.5f,vel.magnitude / 8f);
		Body.transform.localScale = new Vector3(1 - move, 1 + move,1);

		//Okay, we've modified our velocity enough--plug it back into the rigidbody
		RB.velocity = vel;
	}
	
	public bool OnGround()
	{
		Vector2 bottomLeft = (Vector2)(Collider.bounds.center) + new Vector2(-Collider.bounds.extents.x + 0.01f,
			                     -Collider.bounds.extents.y);
		Vector2 right = (Vector2)(Collider.bounds.center) + new Vector2(Collider.bounds.extents.x - 0.01f,
			                -Collider.bounds.extents.y - 0.01f);
		Collider2D r = Physics2D.OverlapArea(bottomLeft, right, LayerMask.GetMask("Floor"));
		return r;

	}


	void OnCollisionEnter2D(Collision2D other)
	{
		//Keeps track of if we're touching any floor objects
		if (other.gameObject.CompareTag("Floor"))
		{
			if (!Touching.Contains(other.gameObject))
				Touching.Add(other.gameObject);
		}

		//Did we bump into an exit?
		ExitController exit = other.gameObject.GetComponent<ExitController>();
		if (exit != null)
		{
			exit.BumpIntoMe(this);
		}

		//Did we bump into a monster?
		MonsterController monster = other.gameObject.GetComponent<MonsterController>();
		if (monster != null)
		{
			monster.BumpIntoMe(this);
		}

		//Did we bump into a coin?
		CoinController coin = other.gameObject.GetComponent<CoinController>();
		if (coin != null)
		{
			coin.BumpIntoMe(this);
		}
	}


	void OnCollisionExit2D(Collision2D other)
	{
		//Tracks when we stop touching floors
		if (other.gameObject.CompareTag("Floor"))
		{
			Touching.Remove(other.gameObject);
		}
	}

}