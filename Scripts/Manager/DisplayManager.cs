using DredPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayManager : SimpleSingleton<DisplayManager>
{
    public float x;
    public float y;

    private void Awake()
    {
        x = Screen.width;
        y = Screen.height;
    }
}
