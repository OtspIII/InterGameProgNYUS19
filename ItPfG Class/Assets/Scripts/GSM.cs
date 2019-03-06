using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GSM : MonoBehaviour
{
    public Camera Cam;
    public TextMeshPro Text;
    public TextMeshPro ScoreTxt;
    public int Score
    {
        get { return ScoreManager.Score; }
        set { ScoreManager.Score = value; }
    }
    public int HP
    {
        get { return ScoreManager.HP; }
        set { ScoreManager.HP = value; }
    }
    public List<WorldThing> AllThings;
    public List<TileThing> AllTiles;
    public Dictionary<int, Dictionary<int, TileThing>> Tiles = new Dictionary<int, Dictionary<int, TileThing>>();

    
    void Awake()
    {
        God.GSM = this;
    }
    
    void Start()
    {
        for (int x = -GameSettings.MapSizeX /2; x <= GameSettings.MapSizeX /2; x++)
        {
            for (int y = -GameSettings.MapSizeY /2; y <= GameSettings.MapSizeY /2; y++)
            {
                God.Library.SpawnTile(x, y);
            }
        }

        List<TileThing> openTiles = new List<TileThing>();
        openTiles.AddRange(AllTiles);
        foreach (WorldThing.Types t in GameSettings.MapContents)
        {
            if (openTiles.Count == 0)
                break;
            TileThing rand = openTiles[Random.Range(0, openTiles.Count)];
            openTiles.Remove(rand);
            God.Library.SpawnThing(t, rand);
        }

        

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
        UpdateText();
    }
    
    //Makes
    public void TakeDamage(int amt)
    {
        HP -= amt;
        if (HP <= 0)
        {
            HP = 0;
            GetThings(WorldThing.Types.Player)[0].Despawn();
            God.GSM.SetText("You Died");
        }
        UpdateText();
    }

    void UpdateText()
    {
        ScoreTxt.text = "Score: " + Score + "\n" + "HP: " + HP;
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
