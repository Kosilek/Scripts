using Kosilek.Ad;
using Kosilek.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Kosilek.UI
{
    public class ButtonRestart : MonoBehaviour
    {
        private Button button;
        [SerializeField] private Animator anim;

        public bool isAdButton = false;

        private string startAnim
        {
            get
            {
                return "Start";
            }
        }

        private void Start()
        {
            SetValues();
            AddListener();
        }

        private void SetValues()
        {
            button = GetComponent<Button>();
        }

        private void AddListener()
        {
            if (!isAdButton)
                button.onClick.AddListener(ActionButton);
            else
                button.onClick.AddListener(ActionRestart);
        }

        private void ActionRestart()
        {
            AdManager.Instance.action = ActionButton;
            AdManager.Instance.ShowRewardedVideoButton();
        }

        private void ActionButton()
        {
            AudioManager2248.Instance.StartEffectClip(AudioManager2248.Instance.audioContainer.clickButon);
            if (BustManager.Instance.isActiveHammer)
                BustManager.Instance.hammerCount++;
            if (BustManager.Instance.isActiveExchange)
                BustManager.Instance.exchangeCount++;
            BustManager.Instance.isActiveHammer = false;
            BustManager.Instance.isActiveExchange = false;
            CellManager.Instance.RaycastIsActive = true;
            CanvasManager.Instance.canvasGame.GetComponent<CanvasGroup>().blocksRaycasts = true;
            CanvasManager.Instance.canvasPause.Close();
            CanvasManager.Instance.canvasLose.Close();
            MonstrIndexManager.Instance.stepIndexMonstrFirst = 0;
            BustManager.Instance.DeAcriveAllBuster();
            anim.SetTrigger(startAnim);
            CanvasManager.Instance.canvasLose.GetComponent<CanvasLose>().isOne = true;
            CanvasManager.Instance.canvasLose.GetComponent<CanvasLose>().resumeButton.interactable = true;
        }
    }
}
