using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GSM : MonoBehaviour
{
    public Camera Cam;
    public TextMeshPro Text;
    public TextMeshPro ScoreTxt;

//    public List<ActorView> AllThings;
//    public List<TileView> AllTiles;
//    public Dictionary<int, Dictionary<int, TileView>> Tiles = new Dictionary<int, Dictionary<int, TileView>>();


    void Awake()
    {
        God.GSM = this;
        ModelManager.Init();//This will only actually run code the first time called
    }

    void Start()
    {
        ModelManager.BuildWorld();
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
