using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActorView : MonoBehaviour
{
    public ActorModel Model;
    
    protected SpriteRenderer Body;
    
    
    void Update()
    {

//        if (Input.GetKeyDown(KeyCode.Space))
//            transform.Dance();
    }

    //When I first spawn a world thing I need to run setup on it so that it does basic stuff like place itself
    public void Setup(ActorModel m)
    {
        Model = m;
        m.View = this;
        Body = transform.Find("Body").GetComponent<SpriteRenderer>();
        gameObject.name = m.Type.ToString();
//        if (!God.GSM.AllThings.Contains(this))
//            God.GSM.AllThings.Add(this);
        Body.sprite = God.Library.GetSprite(m.Type);
        if (m.Species != MonsterType.Types.None)
            Body.sprite = God.Library.GetMonster(m.Species).S;
        SetLocation(m.GetLocation().View);
//        transform.Rotate2D(50);
    }
    
    public void SetLocation(TileView tile)
    {
        
        transform.SetParent(tile.transform);
        transform.localPosition = Vector3.zero;
    }

    //When I'm destroyed make sure I destroy myself safely
    public void Despawn()
    {
        gameObject.SetActive(false);
//        God.GSM.AllThings.Remove(this);
    }
    
}
