using Kosilek.Manager;
using Kosilek.UI;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kosilek.UI
{
    public class CanvasPause : MonoBehaviour
    {
        #region Button
        [Foldout("Buuton")]
        public Button resumeButton;
        [Foldout("Buuton")]
        public Button exitButton;
        #endregion endButton

        #region Start
        private void Start()
        {
            AddListenerButton();
        }

        private void AddListenerButton()
        {
            resumeButton.onClick.AddListener(ActionResumeButton);
            exitButton.onClick.AddListener(ActionExitButton);
        }

        private void ActionResumeButton()
        {
            GameManager.Instance.GamePause(false);
            CanvasManager.Instance.canvasGame.GetComponent<CanvasGroup>().blocksRaycasts = true;
            CanvasManager.Instance.canvasPause.Close();
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
            CanvasManager.Instance.canvasPause.Close();
            GameManager.Instance.GamePause(false);
            CanvasManager.Instance.canvasGame.GetComponent<CanvasGroup>().blocksRaycasts = true;
            CanvasManager.Instance.canvasMenu.GetComponent<CanvasMenu>().ActionEnd();

        }
        #endregion endStart
    }
}