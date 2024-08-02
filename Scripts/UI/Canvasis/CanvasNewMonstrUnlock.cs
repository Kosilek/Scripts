using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DredPack.Help;
using TMPro;
using UnityEngine.UI;
using Kosilek.Manager;
using Kosilek.Ad;

namespace Kosilek.UI
{
    public class CanvasNewMonstrUnlock : MonoBehaviour
    {
        [Foldout("SwitchRoulette")]
        [SerializeField] private List<Transform> switchPosition;
        [Foldout("SwitchRoulette")]
        [SerializeField] private Transform switchTransform;
        private int startIndexSwitch;
        [HideInInspector] public int countPrize;

        #region Animation
        [Foldout("Animation")]
        [SerializeField] private float moveSpeed;
        private float saveMoveSpeed;
        [Foldout("Animation")]
        [SerializeField] private AnimationCurve animCurve;       
        private bool isActive = false;      
        private bool isStop = false;
        private int counter;
        #endregion end Animation

        #region Button
        [Foldout("Button")]
        [SerializeField] private Button adButton;
        [Foldout("Button")]
        [SerializeField] private Button exitButton;
        #endregion

        [SerializeField] private TextMeshProUGUI countPrizeText;

        #region Const
        private const int INDEX_PRIZE_NULL = 2;
        private const int INDEX_PRIZE_FIRST = 3;
        private const int INDEX_PRIZE_SECEND = 4;
        private const int INDEX_PRIZE_THIRD = 5;
        private const int INDEX_PRIZE_FOURTH = 4;
        private const int INDEX_PRIZE_FIFTH = 3;
        private const int INDEX_PRIZE_SIXTH = 2;
        #endregion

        #region Start 
        private void Start()
        {
            SetValues();
            AddListenerButton();
        }

        private void SetValues()
        {
            saveMoveSpeed = moveSpeed;
            InteractableButton(false);
        }

        private void AddListenerButton()
        {
            adButton.onClick.AddListener(ActionAdButton);
            exitButton.onClick.AddListener(ActionExitButton);
        }

        private void ActionAdButton()
        {
            AdManager.Instance.action = ActionAd;
            AdManager.Instance.ShowRewardedVideoButton();
        }

        private void ActionAd()
        {
            GameManager.Instance.GamePause(false, CanvasManager.Instance.canvasNewMonstrUnlock);
            CellManager.Instance.RaycastIsActive = false;
            var value = startIndexSwitch + counter;
            counter = -1;
            while (value > switchPosition.Count - 1)
            {
                if (value > switchPosition.Count - 1)
                    value -= switchPosition.Count - 1;
            }
            value = UpdateUITextPrize(value * 2);
            CanvasManager.Instance.canvasPrize.InitializeUI(value * CanvasManager.Instance.canvasPrize.counter * 2);
        }

        private void ActionExitButton()
        {
            GameManager.Instance.GamePause(false, CanvasManager.Instance.canvasNewMonstrUnlock);
            CellManager.Instance.RaycastIsActive = false;
            var value = startIndexSwitch + counter;
            counter = -1;
            while (value > switchPosition.Count - 1)
            {
                if (value > switchPosition.Count - 1)
                    value -= switchPosition.Count - 1;
            }
            value = UpdateUITextPrize(value);
            CanvasManager.Instance.canvasPrize.InitializeUI(value * CanvasManager.Instance.canvasPrize.counter);
        }
        #endregion end Start

        private void InteractableButton(bool isActive)
        {
           // adButton.interactable = isActive;
           // exitButton.interactable = isActive;
        }

        #region InitializeSwitchRoulette
        public void InitializeSwitchRoulette()
        {
            if (isActive)
                return;
            InteractableButton(false);
            moveSpeed = saveMoveSpeed;
            isActive = true;
            startIndexSwitch = Random.Range(0, switchPosition.Count);
            counter = Random.Range(7, 14);
            switchTransform.position = switchPosition[startIndexSwitch].position;
            UpdateUITextPrize();
            isStop = false;
            MoveSwitcher();

        }

        private void MoveSwitcher()
        {
            StartCoroutine(IE());

            IEnumerator IE()
            {
                while (!isStop)
                {
                    if (startIndexSwitch >= switchPosition.Count - 1)
                    {
                        switchTransform.position = switchPosition[0].position;
                        startIndexSwitch = 0;
                    }
                    yield return StartCoroutine(Lerper.LerpVector3IE(switchTransform.position, switchPosition[startIndexSwitch + 1].position,
                        moveSpeed, animCurve, _ => switchTransform.position = _));
                    startIndexSwitch++;
                    UpdateUITextPrize();
                    counter--;

                    if (counter <= 0)
                        isStop = true;
                }

                MoveSwitcherStop();
            }
        }

        private void MoveSwitcherStop()
        {
            StartCoroutine(IE());

            IEnumerator IE()
            {
                while (isStop)
                {
                    if (startIndexSwitch >= switchPosition.Count - 1)
                    {
                        switchTransform.position = switchPosition[0].position;
                        startIndexSwitch = 0;
                    }
                    yield return StartCoroutine(Lerper.LerpVector3IE(switchTransform.position, switchPosition[startIndexSwitch + 1].position,
                        moveSpeed, animCurve, _ => switchTransform.position = _));
                    startIndexSwitch++;
                    UpdateUITextPrize();
                    moveSpeed -= 0.5f;

                    if (moveSpeed <= 0)
                    {
                        isStop = false;
                        isActive = false;
                        InteractableButton(true);
                    }
                }
            }
        }

        private void UpdateUITextPrize()
        {
            switch(startIndexSwitch)
            {
                case 0:
                    ReceivingAPrize(INDEX_PRIZE_NULL);
                    break;
                case 1:
                    ReceivingAPrize(INDEX_PRIZE_FIRST);
                    break;
                case 2:
                    ReceivingAPrize(INDEX_PRIZE_SECEND);
                    break;
                case 3:
                    ReceivingAPrize(INDEX_PRIZE_THIRD);
                    break;
                case 4:
                    ReceivingAPrize(INDEX_PRIZE_FOURTH);
                    break;
                case 5:
                    ReceivingAPrize(INDEX_PRIZE_FIFTH);
                    break;
                case 6:
                    ReceivingAPrize(INDEX_PRIZE_SIXTH);
                    break;
            }
        }

        private int UpdateUITextPrize(int index)
        {
            switch (index)
            {
                case 0:
                    return INDEX_PRIZE_NULL;
                case 1:
                    return INDEX_PRIZE_FIRST;
                case 2:
                    return INDEX_PRIZE_SECEND;
                case 3:
                    return INDEX_PRIZE_THIRD;
                case 4:
                    return INDEX_PRIZE_FOURTH;
                case 5:
                    return INDEX_PRIZE_FIFTH;
                case 6:
                    return INDEX_PRIZE_SIXTH;
                default: Debug.LogError("Error: index is not value"); return -1;
            }
        }

        private void ReceivingAPrize(int index)
        {
            countPrizeText.text = "+" + index.ToString();
            countPrize = index;
        }

        private void UpdateUICanvasPrize()
        {

        }
        #endregion end InitializeSwitchRoulette
    }
}
