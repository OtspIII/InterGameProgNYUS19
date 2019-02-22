using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour {

	public void BumpIntoMe(PlayerController pc)
	{
		gameObject.SetActive(false);
	}
}
