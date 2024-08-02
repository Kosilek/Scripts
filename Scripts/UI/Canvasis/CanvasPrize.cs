using DredPack.UI;
using Kosilek.Manager;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kosilek.UI
{
    public class CanvasPrize : MonoBehaviour
    {
        private int countPrize;
        [HideInInspector] public int counter = 0; 
        #region Animator
        [SerializeField] private Animator anim;
        #endregion end Animator

        #region ScriptsCompnent
        private Window window;
        #endregion end ScriptsCompnent

        #region TextmeshPro
        [SerializeField] private TextMeshProUGUI prizeText;
        #endregion end TextmeshPro

        #region Button
        [SerializeField] private Button buutonClaim;
        #endregion end Button

        #region Const
        private const string ROTATION = "Rotation";
        private const string STOP = "Stop";
        #endregion end Const

        #region Start
        private void Start()
        {
            SetValues();
            AddListener();
        }

        private void SetValues()
        {
            window = GetComponent<Window>();   
        }

        private void AddListener()
        {
            buutonClaim.onClick.AddListener(ActionClaim);
        }

        private void ActionClaim()
        {
            window.Close();
            MoneyManager.Instance.SetMoney(countPrize);
            CellManager.Instance.RaycastIsActive = true;
            StartCoroutine(IE());

            IEnumerator IE()
            {
                yield return new WaitForSeconds(2f);
                StopRotation();
            }
        }
        #endregion end Start

        #region Action Animator
        public void StartRotation() => StartAnimation(ROTATION);

        public void StopRotation() => StartAnimation(STOP);

        private void StartAnimation(string nameAnimation)
        {
            anim.SetTrigger(nameAnimation);
        }
        #endregion end Action Animator

        #region InitializeUI
        public void InitializeUI(int count)
        {
            AudioManager2248.Instance.StartEffectClip(AudioManager2248.Instance.audioContainer.prizeCanvas);
            countPrize = count;
            StartRotation();
            window.Open();
            prizeText.text = "X" + countPrize.ToString();
        }
        #endregion end InitializeUI
    }
}