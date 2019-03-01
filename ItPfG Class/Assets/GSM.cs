using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GSM : MonoBehaviour
{
    public Camera Cam;
    public TextMeshPro Text;
    public TextMeshPro ScoreTxt;
    public int Score = 0;

    void Start()
    {
        ChangeScore(0);
    }
    
    public void SetText(string txt)
    {
        Text.text = txt;
    }

    public void ChangeScore(int amt)
    {
        Score += amt;
        ScoreTxt.text = "Score: " + Score;
    }
}
