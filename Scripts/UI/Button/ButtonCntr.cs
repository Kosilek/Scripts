using Kosilek.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCntr : MonoBehaviour
{
    protected virtual void OnMouseDown()
    {
        if (!GameManager.Instance.isGame)
            return;

        if (GameManager.Instance.isGamePause)
            return;


        AudioManager2248.Instance.StartEffectClip(AudioManager2248.Instance.audioContainer.clickButon);
    }
}