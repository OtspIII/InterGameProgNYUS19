using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterController : MonoBehaviour
{

	public Rigidbody2D RB;
	
	
	public void BumpIntoMe(PlayerController pc)
	{
		SceneManager.LoadScene("YouLose");
	}
}
