using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGController : MonoBehaviour
{

    public SpriteRenderer SR;
    public List<Sprite> BGs;
    public float Timer = 0;
    
    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0)
        {
            Timer = Random.Range(5f, 10f);
            SR.sprite = BGs[Random.Range(0, BGs.Count)];
        }
    }
}
