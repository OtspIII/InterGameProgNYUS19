using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public Vector2 MapSize;
    public List<WorldThing.Types> MapContents;
    [SerializeField]private int n;

    void Awake()
    {
        GameSettings.MapSizeX = (int)MapSize.x;
        GameSettings.MapSizeY = (int)MapSize.y;
        GameSettings.MapContents = MapContents;
    }
}
