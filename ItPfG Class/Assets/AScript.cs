using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AScript : MonoBehaviour
{
    [Header("Fun with attributes")]
    [Range(0,1)]public float Percent;
    [Space(50)]
    [SerializeField] private string AWord;

    void Start()
    { 
        DontDestroyOnLoad(gameObject);
    }
    
    
    [ContextMenu("Say Hi")]
    public void Hello()
    {
        Debug.Log("Hey!");
    }

}
