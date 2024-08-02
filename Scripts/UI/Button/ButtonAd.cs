using Kosilek.Ad;
using Kosilek.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kosilek.UI;

public class ButtonAd : ButtonCntr
{
    public int countMoney = 15;

    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        if (!GameManager.Instance.isGame)
            return;

        if (GameManager.Instance.isGamePause)
            return;
        if (CanvasTrainingLevel.isActiveTutorital)
            return;
        AdManager.Instance.action = ActionMoney;
        AdManager.Instance.ShowRewardedVideoButton();
    }

    private void ActionMoney()
    {
        MoneyManager.Instance.SetMoney(countMoney);
    }
}
