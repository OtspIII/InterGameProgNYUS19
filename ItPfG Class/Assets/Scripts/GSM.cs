using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GSM : MonoBehaviour
{
    public Camera Cam;
    public TextMeshPro Text;
    public TextMeshPro ScoreTxt;


    void Awake()
    {
        God.GSM = this;
        Debug.Log(Application.persistentDataPath);
    }

    void Start()
    {
        ModelManager.BuildModel();
        ModelManager.BuildView();
        UpdateText();
    }

    
    
    //I can update the big screen covering text with this
    public void SetText(string txt)
    {
        Text.text = txt;
    }

    
    public void UpdateText()
    {
        ScoreTxt.text = "Score: " + ModelManager.Score + "\n" + "HP: " + ModelManager.HP;
    }

    
}
