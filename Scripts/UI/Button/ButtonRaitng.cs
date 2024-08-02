using Kosilek.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kosilek.UI;

public class ButtonRaitng : ButtonCntr
{
    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        if (!GameManager.Instance.isGame)
            return;
        if (GameManager.Instance.isGamePause)
            return;
        if (CanvasTrainingLevel.isActiveTutorital)
            return;
    }
}
