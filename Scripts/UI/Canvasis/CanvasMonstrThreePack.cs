using Kosilek.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kosilek.UI
{
    public class CanvasMonstrThreePack : MonoBehaviour
    {
        public Button backButton;

        #region ThreeButton
        [SerializeField]
        private Button threeButton1;
        #endregion end ThreeButton

        #region Start
        private void Start()
        {
            AddListener();
        }

        private void AddListener()
        {
            backButton.onClick.AddListener(ActionButton);
            threeButton1.onClick.AddListener(ActionThreeButton);
        }

        private void ActionButton()
        {
            Debug.Log("Click Action Button");
            CanvasManager.Instance.actionWindow.OpenWindow(false, CanvasManager.Instance.canvasMonstrThreePack, "canvasMonstrThreePack");
            AudioManager2248.Instance.StartEffectClip(AudioManager2248.Instance.audioContainer.clickButon);
        }

        #region ActionThreeButton
        private void ActionThreeButton()
        {
            Debug.Log("Click ActionThreeButton");
            CanvasManager.Instance.actionWindow.OpenWindow(true, CanvasManager.Instance.canvasMonstrSee, "canvasMonstrSee");
            AudioManager2248.Instance.StartEffectClip(AudioManager2248.Instance.audioContainer.clickButon);
        }
        #endregion end ActionThreeButton
        #endregion end Start
    }
}
