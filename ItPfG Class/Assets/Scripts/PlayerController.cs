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


	void Start()
	{
		PlayerController.Singleton = this;
		//Find our rigidbody and audio right at the start
		RB = GetComponent<Rigidbody2D>();
		AS = GetComponent<AudioSource>();
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

		//Jump, but only if you're touching the ground
		if (Input.GetKeyDown(KeyCode.UpArrow) )//&& OnGround()
		{
//			RB.AddForce(new Vector2(0,10),ForceMode2D.Impulse);
//			transform.position += new Vector3(0,0.2f,0);
			vel.y = JumpPower;
			//AS.PlayOneShot(Hop);
			Particles.Emit(10);
		}

		float move = Mathf.Min(0.5f,vel.magnitude / 8f);
		Body.transform.localScale = new Vector3(1 - move, 1 + move,1);

		//Okay, we've modified our velocity enough--plug it back into the rigidbody
		RB.velocity = vel;
	}
	
	public bool OnGround()
	{
		return Touching.Count > 0;
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