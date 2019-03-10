using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AScript : MonoBehaviour
{
    [Header("Fun with attributes")]
    [Range(0,1)]public float Percent;
    [Space(50)]
    [SerializeField] private string AWord;

    public Coroutine ColorFade;

    void Start()
    { 
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (ColorFade != null)
                StopCoroutine(ColorFade);
            ColorFade = StartCoroutine(FadeToColor(new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f))));
        }

    }

    IEnumerator FadeToColor(Color c)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        while (sr.color != c)
        {
            sr.color = Color.Lerp(sr.color, c, 0.1f);
            yield return null;
        }
    }


    [ContextMenu("Say Hi")]
    public void Hello()
    {
        Debug.Log("Hey!");
    }

}
