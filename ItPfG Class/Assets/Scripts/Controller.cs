using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    private Queue<GameAction> Acts = new Queue<GameAction>();
    
    void Awake()
    {
        God.C = this;
        StartCoroutine(MainLoop());
    }
    
    public IEnumerator MainLoop()
    {
        while (true)
        {
            ReadInputs();
            yield return StartCoroutine(ResolveActions());
        }
    }
    
    void ReadInputs()
    {
        if (IM.Pressed(Inputs.Left))
        {
            MsgAll(new EventMsg(Inputs.Left));
        }
        else if (IM.Pressed(Inputs.Right))
        {
            MsgAll(new EventMsg(Inputs.Right));
        }
        else if (IM.Pressed(Inputs.Up))
        {
            MsgAll(new EventMsg(Inputs.Up));
        }
        else if (IM.Pressed(Inputs.Down))
        {
            MsgAll(new EventMsg(Inputs.Down));
        }
        
        if (Input.GetKeyUp(KeyCode.S))
        {
            ModelManager.SaveGame();
        }
        if (Input.GetKeyUp(KeyCode.L))
        {
            ModelManager.PendingSave = ModelManager.LoadGame();
            if (ModelManager.PendingSave != null)
                SceneManager.LoadScene(0);
            else
                Debug.Log("NO FILE TO LOAD");
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            ModelManager.DeleteSave();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            StartCoroutine(ModelManager.CaptureScreenshot("X"));
        }
    }

    public void MsgAll(EventMsg msg)
    {
        foreach(ActorModel am in ModelManager.GetActors())
            am.TakeMsg(msg);
    }

    public void AddAction(GameAction a)
    {
        Acts.Enqueue(a);
    }

    public IEnumerator ResolveActions()
    {
        while (Acts.Count > 0)
        {
            yield return StartCoroutine(Acts.Dequeue().Run());
        }
    }
}
