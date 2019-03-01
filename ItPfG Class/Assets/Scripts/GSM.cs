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
    public List<WorldThing> AllThings;//This doesn't work yet
    public List<TileThing> AllTiles;
    public Dictionary<int, Dictionary<int, TileThing>> Tiles = new Dictionary<int, Dictionary<int, TileThing>>(); 

    void Start()
    {
        ChangeScore(0);
    }
    
    //I can update the big screen covering text with this
    public void SetText(string txt)
    {
        Text.text = txt;
    }

    //This makes my points go up or down and also updates the text in the top left corner
    public void ChangeScore(int amt)
    {
        Score += amt;
        ScoreTxt.text = "Score: " + Score;
    }

    //I can get a list of all the TYPE of a thing to exist.
    //If I don't specify a type it just gives me all world things that exist period
    //This doesn't work yet, since I don't have a way to populate AllThings
    public List<WorldThing> GetThings(WorldThing.Types type = WorldThing.Types.None)
    {
        List<WorldThing> r = new List<WorldThing>();
        foreach(WorldThing wt in AllThings)
            if (type == WorldThing.Types.None || wt.Type == type)
                r.Add(wt);
        return r;
    }

    //Feed me coordinates and I'll tell you the tile that lives there
    public TileThing GetTile(int x, int y)
    {
        if (!Tiles.ContainsKey(x) || !Tiles[x].ContainsKey(y))
            return null;
        return Tiles[x][y];
    }
}
