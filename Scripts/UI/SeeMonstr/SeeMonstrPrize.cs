using DredPack.Help;
using Kosilek.Manager;
using Kosilek.MonstrPack;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

namespace Kosilek.UI
{
    public class SeeMonstrPrize : MonoBehaviour
    {
        public Animator anim;

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
        [Foldout("Monstr")]
        [SerializeField]
        private List<CanvasGroup> canvasGroupMonstr;
        #endregion end BackGroundMonstr

        #region Animation
        [Foldout("Animation")]
        [SerializeField]
        private float moveSpeed;
        [Foldout("Animation")]
        [SerializeField]
        private AnimationCurve animCurve;
        private bool isAciveCor = false;
        #endregion end Animation

        #region Index
        [SerializeField]
        private int indexLeft = 0;
        [SerializeField]
        private int indexRight = 4;
        #endregion

        #region Const
        private const int NULL_INDEX = 0;
        private const int FIRST_INDEX = 1;
        private const int SECOND_INDEX = 2;
        private const int THIRD_INDEX = 3;
        private const int FOURTH_INDEX = 4;

        private const string INTERMEDIATE = "Intermediate";
        private const string STOP = "Intermediate";
        #endregion end Const

        #region Start
        public void Initialize()
        {
            if (MonstrIndexManager.Instance.oldNumberMonstOpen + 2 < ManagerMonstrPack.Instance.monsterCollection.monstrPack.Count)
                cellSeeMonstr[4].image.sprite = ManagerMonstrPack.Instance.monsterCollection.monstrPack[MonstrIndexManager.Instance.oldNumberMonstOpen + 2].iconMonstr;
            else
                canvasGroupMonstr[4].alpha = 0f;
            if (MonstrIndexManager.Instance.oldNumberMonstOpen + 1 < ManagerMonstrPack.Instance.monsterCollection.monstrPack.Count)
                cellSeeMonstr[3].image.sprite = ManagerMonstrPack.Instance.monsterCollection.monstrPack[MonstrIndexManager.Instance.oldNumberMonstOpen + 1].iconMonstr;
            else
                canvasGroupMonstr[3].alpha = 0f;
            cellSeeMonstr[2].image.sprite = ManagerMonstrPack.Instance.monsterCollection.monstrPack[MonstrIndexManager.Instance.oldNumberMonstOpen].iconMonstr;
            if (MonstrIndexManager.Instance.oldNumberMonstOpen - 1 > 0)
                cellSeeMonstr[1].image.sprite = ManagerMonstrPack.Instance.monsterCollection.monstrPack[MonstrIndexManager.Instance.oldNumberMonstOpen - 1].iconMonstr;
            else
                canvasGroupMonstr[1].alpha = 0f;
            if (MonstrIndexManager.Instance.oldNumberMonstOpen - 2 > 0)
                cellSeeMonstr[0].image.sprite = ManagerMonstrPack.Instance.monsterCollection.monstrPack[MonstrIndexManager.Instance.oldNumberMonstOpen - 2].iconMonstr;
            else
                canvasGroupMonstr[0].alpha = 0f;

            if (MonstrIndexManager.Instance.oldNumberMonstOpen + 2 < ManagerMonstrPack.Instance.monsterCollection.monstrPack.Count)
                // cellSeeMonstr[4].text.text = ManagerMonstrPack.Instance.monsterCollection.monstrPack[MonstrIndexManager.Instance.oldNumberMonstOpen + 2].indexMonstr.ToString();
                cellSeeMonstr[4].text.text = DigitConverter.digitConverter((ulong)ManagerMonstrPack.Instance.monsterCollection.monstrPack[MonstrIndexManager.Instance.oldNumberMonstOpen + 2].indexMonstr);

            else
                canvasGroupMonstr[4].alpha = 0f;
            if (MonstrIndexManager.Instance.oldNumberMonstOpen + 1 < ManagerMonstrPack.Instance.monsterCollection.monstrPack.Count)
                cellSeeMonstr[3].text.text = DigitConverter.digitConverter((ulong)ManagerMonstrPack.Instance.monsterCollection.monstrPack[MonstrIndexManager.Instance.oldNumberMonstOpen + 1].indexMonstr);
            else
                canvasGroupMonstr[3].alpha = 0f;
            cellSeeMonstr[2].text.text = DigitConverter.digitConverter((ulong)ManagerMonstrPack.Instance.monsterCollection.monstrPack[MonstrIndexManager.Instance.oldNumberMonstOpen].indexMonstr);
            if (MonstrIndexManager.Instance.oldNumberMonstOpen - 1 > 0)
                cellSeeMonstr[1].text.text = DigitConverter.digitConverter((ulong)ManagerMonstrPack.Instance.monsterCollection.monstrPack[MonstrIndexManager.Instance.oldNumberMonstOpen -1].indexMonstr);
            else
                canvasGroupMonstr[1].alpha = 0f;
            if (MonstrIndexManager.Instance.oldNumberMonstOpen - 2 > 0)
                cellSeeMonstr[0].text.text = DigitConverter.digitConverter((ulong)ManagerMonstrPack.Instance.monsterCollection.monstrPack[MonstrIndexManager.Instance.oldNumberMonstOpen - 2].indexMonstr);
            else
                canvasGroupMonstr[0].alpha = 0f;


            indexLeft = MonstrIndexManager.Instance.oldNumberMonstOpen - 2;
            indexRight = MonstrIndexManager.Instance.oldNumberMonstOpen + 2;
        }
        #endregion

        #region Animation
        public void Move()
        {
            if (isAciveCor)
                return;

            var counter = 0;
            for (int i = MonstrIndexManager.Instance.oldNumberMonstOpen;
i < MonstrIndexManager.Instance.maxNumberMonstOpen; i++)
            {
                moveSpeed *= 1.5f;
                counter++;
            }
            CanvasManager.Instance.canvasPrize.counter = counter;
            StartCoroutine(IEStart());

            IEnumerator IEStart()
            {
                for (int i = MonstrIndexManager.Instance.oldNumberMonstOpen;
                i < MonstrIndexManager.Instance.maxNumberMonstOpen; i++)
                {
                    isAciveCor = true;

                    if (i == MonstrIndexManager.Instance.maxNumberMonstOpen - 1)
                        StartAnimation();

                    yield return StartCoroutine(IE());
                    moveSpeed /= 1.5f;
                }

                MonstrIndexManager.Instance.oldNumberMonstOpen = MonstrIndexManager.Instance.maxNumberMonstOpen;
            }

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

                ActionCoroutineStart(THIRD_INDEX);

                SwitchImageRight(indexRight);

                for (int i = 1; i < backGroundMonstr.Count; i++)
                {
                    StartCorMove(i, i - 1);
                    canvasGroupMonstr[i - 1].alpha = 1f;
                }
                yield return new WaitForSeconds(1f / moveSpeed);

                ActionCoroutineLast(NULL_INDEX, mainRect.Count - 1, true);
            }
        }

        private void ActionCoroutineStart(int indexUpper)
        {
            backGroundMonstr[indexUpper].SetAsLastSibling();
        }

        private void SwitchImageRight(int index)
        {
            cellSeeMonstr[0].image.sprite = ManagerMonstrPack.Instance.monsterCollection.monstrPack[index].iconMonstr;
            cellSeeMonstr[0].text.text = DigitConverter.digitConverter((ulong)ManagerMonstrPack.Instance.monsterCollection.monstrPack[index].indexMonstr);
            // cellSeeMonstr[0].text.text = ManagerMonstrPack.Instance.monsterCollection.monstrPack[index].indexMonstr.ToString();
        }

        private void StartCorMove(int index, int rectIndex)
        {
            StartCoroutine(Lerper.LerpVector3IE(backGroundMonstr[index].position, mainRect[rectIndex].position, moveSpeed, animCurve, _ => backGroundMonstr[index].position = _));
            StartCoroutine(Lerper.LerpVector3IE(backGroundMonstr[index].localScale, mainRect[rectIndex].localScale, moveSpeed, animCurve, _ => backGroundMonstr[index].localScale = _));
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
        #endregion

        #region Animator
        public void StartAnimation()
        {
            anim.SetTrigger(INTERMEDIATE);
        }

        public void StopAnimation()
        {
            anim.SetTrigger(STOP);
        }
        #endregion end Animator
    }
}
