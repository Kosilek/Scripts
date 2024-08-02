using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Security.Cryptography;
using DredPack.Help;
using Kosilek.MonstrPack;
using Unity.VisualScripting;
using Kosilek.Manager;

namespace Kosilek.UI
{
    public class SeeMonstr : MonoBehaviour
    {
        #region RectTransform
        [Foldout("RectTransform")]
        [SerializeField]
        private List<RectTransform> mainRect;
        #endregion end RectTrnform

        #region backGroundMonster
        [Foldout("Monstr")]
        [SerializeField]
        private List<RectTransform> backGroundMonstr;
        [Foldout("Monstr")]
        [SerializeField]
        private List<CellSeeMonstr> cellSeeMonstr;
        #endregion end BackGroundMonstr

        #region Animation
        [Foldout("Animation")]
        [SerializeField]
        private float moveSpeed;
        [Foldout("Animation")]
        [SerializeField]
        private AnimationCurve animCurve;

        private bool isAciveCor = false;
        private Coroutine coroutine;
        #endregion end Animation

        #region Button
        [Foldout("Button")]
        [SerializeField]
        private Button leftButton;
        [Foldout("Button")]
        [SerializeField]
        private Button rightButton;
        #endregion endButton

        #region Const
        private const int NULL_INDEX = 0;
        private const int FIRST_INDEX = 1;
        private const int SECOND_INDEX = 2;
        private const int THIRD_INDEX = 3;
        private const int FOURTH_INDEX = 4;
        #endregion end Const

        #region Index
        [SerializeField]
        private int indexLeft = 0;
        [SerializeField]
        private int indexRight = 4;
        #endregion

        #region TopIcon
        [SerializeField] private Image image;
        [SerializeField] private CanvasGroup canvasGroup;
        #endregion

        #region Start
        private void Start()
        {
            AddListenerButton();

            cellSeeMonstr[0].image.sprite = ManagerMonstrPack.Instance.monsterCollection.monstrPack[4].iconMonstr;
            cellSeeMonstr[1].image.sprite = ManagerMonstrPack.Instance.monsterCollection.monstrPack[3].iconMonstr;
            cellSeeMonstr[2].image.sprite = ManagerMonstrPack.Instance.monsterCollection.monstrPack[2].iconMonstr;
            cellSeeMonstr[3].image.sprite = ManagerMonstrPack.Instance.monsterCollection.monstrPack[1].iconMonstr;
            cellSeeMonstr[4].image.sprite = ManagerMonstrPack.Instance.monsterCollection.monstrPack[0].iconMonstr;

            cellSeeMonstr[0].text.text = ManagerMonstrPack.Instance.monsterCollection.monstrPack[4].indexMonstr.ToString();
            cellSeeMonstr[1].text.text = ManagerMonstrPack.Instance.monsterCollection.monstrPack[3].indexMonstr.ToString();
            cellSeeMonstr[2].text.text = ManagerMonstrPack.Instance.monsterCollection.monstrPack[2].indexMonstr.ToString();
            cellSeeMonstr[3].text.text = ManagerMonstrPack.Instance.monsterCollection.monstrPack[1].indexMonstr.ToString();
            cellSeeMonstr[4].text.text = ManagerMonstrPack.Instance.monsterCollection.monstrPack[0].indexMonstr.ToString();

            image.sprite = ManagerMonstrPack.Instance.monsterCollection.monstrPack[2].iconMonstr;

        }

        private void AddListenerButton()
        {
            leftButton.onClick.AddListener(MoveLeft);
            rightButton.onClick.AddListener(MoveRight);
        }

        private void ActivonButtonMove()
        {

        }
        #endregion endStart

        #region Move
        private void MoveLeft()
        {
            if (isAciveCor)
                return;

            AudioManager2248.Instance.StartEffectClip(AudioManager2248.Instance.audioContainer.clickButon);

            InitStartMoveButton(IE());
            StartCoroutine(IEImage(3));

            IEnumerator IE()
            {
                if (indexRight > 0)
                    indexRight--;
                else
                    indexRight = ManagerMonstrPack.Instance.monsterCollection.monstrPack.Count - 1;
                if (indexLeft > 0)
                    indexLeft--;
                else
                    indexLeft = ManagerMonstrPack.Instance.monsterCollection.monstrPack.Count - 1;
                
               

                ActionCoroutineStart(THIRD_INDEX);

                SwitchImageLeft(indexLeft);

                for (int i = 1; i < backGroundMonstr.Count; i++)
                {
                    StartCorMove(i, i - 1);
                }
                yield return new WaitForSeconds(1f / moveSpeed);

                ActionCoroutineLast(NULL_INDEX, mainRect.Count - 1, true);
            }
        }

        private void MoveRight()
        {
            if (isAciveCor)
                return;

            AudioManager2248.Instance.StartEffectClip(AudioManager2248.Instance.audioContainer.clickButon);

            InitStartMoveButton(IE());
            StartCoroutine(IEImage(1));

            IEnumerator IE()
            {

                if (indexLeft < ManagerMonstrPack.Instance.monsterCollection.monstrPack.Count - 1)
                    indexLeft++;
                else
                    indexLeft = 0;

                if (indexRight < ManagerMonstrPack.Instance.monsterCollection.monstrPack.Count - 1)
                    indexRight++;
                else
                    indexRight = 0;

                ActionCoroutineStart(FIRST_INDEX);

                SwitchImageRight(indexRight);

                for (int i = 0; i < backGroundMonstr.Count - 1; i++)
                {
                    StartCorMove(i, i + 1);
                }
                yield return new WaitForSeconds(1f / moveSpeed);

                ActionCoroutineLast(backGroundMonstr.Count - 1, NULL_INDEX, false);
            }
        }

        private void InitStartMoveButton(IEnumerator IE)
        {
            isAciveCor = true;
            StartCoroutine(IE);
        }

        private IEnumerator IEImage(int i)
        {
            var move = moveSpeed * 2;
            yield return StartCoroutine(CustomIenumerator.IEImageAlphaCor(canvasGroup.alpha, 0f, move, animCurve, _ => canvasGroup.alpha = _));
            image.sprite = cellSeeMonstr[i].image.sprite;
            yield return StartCoroutine(CustomIenumerator.IEImageAlphaCor(canvasGroup.alpha, 1f, move, animCurve, _ => canvasGroup.alpha = _));
        }

        private void StartCorMove(int index, int rectIndex)
        {
            StartCoroutine(Lerper.LerpVector3IE(backGroundMonstr[index].position, mainRect[rectIndex].position, moveSpeed, animCurve, _ => backGroundMonstr[index].position = _));
            StartCoroutine(Lerper.LerpVector3IE(backGroundMonstr[index].localScale, mainRect[rectIndex].localScale, moveSpeed, animCurve, _ => backGroundMonstr[index].localScale = _));
        }

        private void ActionCoroutineStart(int indexUpper)
        {
            backGroundMonstr[indexUpper].SetAsLastSibling();
        }

        private void ActionCoroutineMiddle()
        {

        }

        private void ActionCoroutineLast(int firstIndex, int secondIndex, bool isAdd)
        {
            backGroundMonstr[firstIndex].transform.position = mainRect[secondIndex].position;
            var saveBackGround = backGroundMonstr[firstIndex];
            backGroundMonstr.RemoveAt(firstIndex);

            var saveCellSeeMonstr = cellSeeMonstr[firstIndex];
            cellSeeMonstr[firstIndex] = cellSeeMonstr[secondIndex];
            cellSeeMonstr.RemoveAt(firstIndex);

            Action<int, RectTransform> actionRect = isAdd ? AddList : InsertList;
            actionRect.Invoke(secondIndex, saveBackGround);

            Action<int, CellSeeMonstr> actionCell = isAdd ? AddList : InsertList;
            actionCell.Invoke(secondIndex, saveCellSeeMonstr);

            isAciveCor = false;
        }

        private void AddList(int index, RectTransform rect)
        {
            backGroundMonstr.Add(rect);
        }

        private void InsertList(int index, RectTransform rect)
        {
            backGroundMonstr.Insert(index, rect);
        }

        private void AddList(int index, CellSeeMonstr cellSeeMonstr)
        {
            this.cellSeeMonstr.Add(cellSeeMonstr);
        }

        private void InsertList(int index, CellSeeMonstr cellSeeMonstr)
        {
            this.cellSeeMonstr.Insert(index, cellSeeMonstr);
        }

        private void SwitchImageRight(int index)
        {
       //     if (indexLeft < ManagerMonstrPack.Instance.monsterCollection.monstrPack.Count)
     //       {
                cellSeeMonstr[4].image.sprite = ManagerMonstrPack.Instance.monsterCollection.monstrPack[index].iconMonstr;
            cellSeeMonstr[4].text.text = DigitConverter.digitConverter((ulong)ManagerMonstrPack.Instance.monsterCollection.monstrPack[index].indexMonstr);
            // cellSeeMonstr[4].text.text = ManagerMonstrPack.Instance.monsterCollection.monstrPack[index].indexMonstr.ToString();
            //       }
            /*   elseSwitchImageRight
               {
                   var newIndex = index - ManagerMonstrPack.Instance.monsterCollection.monstrPack.Count + 2;
                   cellSeeMonstr[4].image.sprite = ManagerMonstrPack.Instance.monsterCollection.monstrPack[newIndex].iconMonstr;
                   cellSeeMonstr[4].text.text = ManagerMonstrPack.Instance.monsterCollection.monstrPack[newIndex].indexMonstr.ToString();
               }*/
        }

        private void SwitchImageLeft(int index)
        {
    //        if (index - 2 > -1)
     //       {
                cellSeeMonstr[0].image.sprite = ManagerMonstrPack.Instance.monsterCollection.monstrPack[index].iconMonstr;
            cellSeeMonstr[0].text.text = DigitConverter.digitConverter((ulong)ManagerMonstrPack.Instance.monsterCollection.monstrPack[index].indexMonstr);
            //cellSeeMonstr[0].text.text = ManagerMonstrPack.Instance.monsterCollection.monstrPack[index].indexMonstr.ToString();
            //       }
            /*       else
                   {
                       var newIndex = ManagerMonstrPack.Instance.monsterCollection.monstrPack.Count - 2 + index;
                       cellSeeMonstr[0].image.sprite = ManagerMonstrPack.Instance.monsterCollection.monstrPack[newIndex].iconMonstr;
                       cellSeeMonstr[0].text.text = ManagerMonstrPack.Instance.monsterCollection.monstrPack[newIndex].indexMonstr.ToString();
                   }*/
        }
        #endregion endMove
    }
}