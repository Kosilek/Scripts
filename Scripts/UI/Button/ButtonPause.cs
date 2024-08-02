using Kosilek.Manager;
using Kosilek.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPause : ButtonCntr
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
        GameManager.Instance.GamePause(true);
        CanvasManager.Instance.canvasGame.GetComponent<CanvasGroup>().blocksRaycasts = false;
        CanvasManager.Instance.canvasPause.Open();
    }
}
