using Kosilek.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kosilek.UI
{
    public class CanvasMonstrSee : MonoBehaviour
    {
        public Button backButton;

        #region Start
        private void Start()
        {
            AddListener();
        }

        private void AddListener()
        {
            backButton.onClick.AddListener(ActionButton);
        }

        private void ActionButton()
        {
            CanvasManager.Instance.actionWindow.OpenWindow(false, CanvasManager.Instance.canvasMonstrSee, "canvasMonstrSee");
            AudioManager2248.Instance.StartEffectClip(AudioManager2248.Instance.audioContainer.clickButon);
        }
        #endregion end Start
    }
}
