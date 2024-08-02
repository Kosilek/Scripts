using DredPack.UI;
using Kosilek.Ad;
using Kosilek.Manager;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kosilek.UI
{
    public class CanvasLose : MonoBehaviour
    {
        public Animator anim;

        public bool isOne = true;

        #region Button
        [Foldout("Buuton")]
        public Button exitButton;
        [Foldout("Buuton")]
        public Button resumeButton;
        #endregion endButton

        #region Start
        private void Start()
        {
            AddListenerButton();
        }

        private void AddListenerButton()
        {
            exitButton.onClick.AddListener(ActionExitButton);
            resumeButton.onClick.AddListener(ActionResumeButton);
        }

        private void ActionExitButton()
        {
            CanvasManager.Instance.canvasMenuScr.EventEndGameFirst();
            if (BustManager.Instance.isActiveHammer)
                BustManager.Instance.hammerCount++;
            if (BustManager.Instance.isActiveExchange)
                BustManager.Instance.exchangeCount++;
           // AudioManager2248.Instance.StartMainClip(true);
            BustManager.Instance.isActiveHammer = false;
            BustManager.Instance.isActiveExchange = false;
            CellManager.Instance.RaycastIsActive = false;
            BustManager.Instance.DeAcriveAllBuster();
            CanvasManager.Instance.canvasLose.Close();
            GameManager.Instance.GamePause(false);
            CanvasManager.Instance.canvasGame.GetComponent<CanvasGroup>().blocksRaycasts = true;
            CanvasManager.Instance.canvasMenu.GetComponent<CanvasMenu>().ActionEnd();
        }

        private void ActionResumeButton()
        {
           if (isOne)
            {
                EndAnim();
                AdManager.Instance.action = ActionResume;
                AdManager.Instance.ShowRewardedVideoButton();
                isOne = false;
                resumeButton.interactable = false;
            }
           
        }

        private void ActionResume()
        {
            EndAnim();
            AudioManager2248.Instance.StartEffectClip(AudioManager2248.Instance.audioContainer.clickButon);
            if (BustManager.Instance.isActiveHammer)
                BustManager.Instance.hammerCount++;
            if (BustManager.Instance.isActiveExchange)
                BustManager.Instance.exchangeCount++;
            BustManager.Instance.isActiveHammer = false;
            BustManager.Instance.isActiveExchange = false;
            resumeButton.interactable = false;
            CellManager.Instance.RaycastIsActive = true;
            CanvasManager.Instance.canvasGame.GetComponent<CanvasGroup>().blocksRaycasts = true;
            CanvasManager.Instance.canvasLose.Close();
            BustManager.Instance.DeAcriveAllBuster();
            CellManager.Instance.ResumeGame();
            GetComponent<Window>().Close();
        }
        #endregion endStart

        #region Animator
        public void StartAnim()
        {
            anim.SetTrigger("Start");
        }

        public void EndAnim()
        {
            anim.SetTrigger("End");
        }
        #endregion end Animation
    }
}