using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using Kosilek.Manager;

namespace Kosilek.UI
{
    public class CanvasShop : MonoBehaviour
    {
        public Button backButton;
        public GameObject backGame;

        #region Start
        private void Start()
        {
            AddListener(true);
        }

        public void AddListener(bool isMenu)
        {
            backButton.onClick.RemoveAllListeners();
            UnityAction action = isMenu ? ActionButtonMenu : ActionButtongame;
            backButton.onClick.AddListener(action);
        }

        private void ActionButtonMenu()
        {
            CanvasManager.Instance.actionWindow.OpenWindow(false, CanvasManager.Instance.canvasShop, "canvasShop");
            AudioManager2248.Instance.StartEffectClip(AudioManager2248.Instance.audioContainer.clickButon);
        }

        private void ActionButtongame()
        {
            AudioManager2248.Instance.StartEffectClip(AudioManager2248.Instance.audioContainer.clickButon);
            GameManager.Instance.GamePause(false);
            CanvasManager.Instance.canvasShop.Close();
            CanvasManager.Instance.canvasGame.GetComponent<CanvasGroup>().blocksRaycasts = true;
            backGame.SetActive(false);
        }
        #endregion end Start
    }
}