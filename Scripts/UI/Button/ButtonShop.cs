using Kosilek.Manager;
using Kosilek.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonShop : ButtonCntr
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
        CanvasManager.Instance.canvasShop.GetComponent<CanvasShop>().AddListener(false);
        CanvasManager.Instance.canvasShop.Open();
        CanvasManager.Instance.canvasShop.GetComponent<CanvasShop>().backGame.SetActive(true);
    }
}
