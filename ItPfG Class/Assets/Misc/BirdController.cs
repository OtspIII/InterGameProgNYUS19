using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    public SpriteRenderer Body;
    public SpriteRenderer Head;
    public Vector3 Target;
    public float spin = 0;
    
    void Start()
    {
        Target = new Vector3(Random.Range(-8f,8f),Random.Range(-4f,1f),0);
    }

    void Update()
    {
        Vector3 pos = transform.position;
        
//        pos += new Vector3(0.05f,0,0);

        if (Vector3.Distance(pos, Target) < 0.1f)
        {
            Target = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 1f), 0);
            spin = -360;
        }

        if (spin != 0)
        {
            spin = Mathf.MoveTowards(spin, 0, 5);
            Body.transform.localRotation = Quaternion.Euler( new Vector3(0,0,spin));
        }
        else
        {
            pos = Vector3.MoveTowards(pos, Target, 0.05f);
            Body.transform.localPosition = new Vector3(0, Mathf.Sin(Time.time * 8f)*0.75f,0);
        }

        transform.position = pos;
    }
}
