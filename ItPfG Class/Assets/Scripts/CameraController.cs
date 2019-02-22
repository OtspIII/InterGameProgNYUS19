using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraController : MonoBehaviour
{

	public TextMeshPro Text;
	private float Timer;
	
	public Vector2 Bounds;
	
	void Start () {
	}

	
	void Update ()
	{	
		Vector3 pos = transform.position;
		
		//If the player moves too far to the left or right away from me I follow them
		if (transform.position.x > PlayerController.Singleton.transform.position.x + 2)
			pos.x = PlayerController.Singleton.transform.position.x + 2;
		if (transform.position.x < PlayerController.Singleton.transform.position.x - 2)
			pos.x = PlayerController.Singleton.transform.position.x - 2;
		
		//If the player approaches the edge of the map I don't move past the edge
		if (pos.x < Bounds.x)
			pos.x = Bounds.x;
		if (pos.x > Bounds.y)
			pos.x = Bounds.y;

		transform.position = pos;
	}
}

