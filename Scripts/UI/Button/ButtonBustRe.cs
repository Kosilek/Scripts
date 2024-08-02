using Kosilek.Manager;
using UnityEngine;
using System;
using Kosilek.Data;

namespace Kosilek.UI
{
    public class ButtonBustRe : ButtonCntr
    {
        #region Touch
        private Touch touch;
        #endregion

        private bool isFirst = false;

        private CellGame cellFirst;
        private CellGame cellSecond;

        protected override void OnMouseDown()
        {
            base.OnMouseDown();
            if (!GameManager.Instance.isGame)
                return;
            if (GameManager.Instance.isGamePause)
                return;
            if (BustManager.Instance.isHummer)
                return;
            if (!CanvasTrainingLevel.isActiveSecondBust)
                return;

            Action action = BustManager.Instance.isExchange ? InAction : Action;
            action.Invoke();


        }

        private void Action()
        {
            BustManager.Instance.isExchange = true;
            BustManager.Instance.ActiveBusterExchange();
        }

        private void InAction()
        {
            BustManager.Instance.DeActiveBustButton(BustManager.Instance.exchangeCount, false, BustManager.Instance.imageExchange, BustManager.Instance.isActiveExchange);
        }

        private void Update()
        {
            if (!BustManager.Instance.isActiveExchange)
                return;
            if (!GameManager.Instance.isGame)
                return;
            if (GameManager.Instance.isGamePause)
                return;

#if UNITY_EDITOR
            MoveWindow();
#else
            MoveAndriod();
#endif
        }

        private void MoveWindow()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = CellManager.Instance.GetPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.TryGetComponent(out CellGame data))
                    {
                        if (cellFirst == null)
                        {
                            if (data.isReady)
                            cellFirst = data;
                            StartCoroutine(cellFirst.IEStartEffectUpScale());
                        }
                        else if (cellSecond == null)
                        {
                            if (cellFirst.index != data.index && CheckIndex(data))
                            {
                                if (data.isReady)
                                    cellSecond = data;
                                StartCoroutine(cellSecond.IEStartEffectUpScale());
                            }
                        }
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (cellSecond == null)
                    return;

                AudioManager2248.Instance.StartEffectClip(AudioManager2248.Instance.audioContainer.echangesCell);

                var saveCellFirst = CellManager.Instance.cellData[cellFirst.index];
                var saveCellSecond = CellManager.Instance.cellData[cellSecond.index];

                var index = CellManager.Instance.cellData[cellFirst.index].indexMonstr;
                var sprite = CellManager.Instance.cellData[cellFirst.index].image.sprite;
                var text = CellManager.Instance.cellData[cellFirst.index].textIndexMonstr.text;

                CellManager.Instance.cellData[cellFirst.index].StartEffestRotation(CellManager.Instance.cellData[cellSecond.index].indexMonstr,
                    CellManager.Instance.cellData[cellSecond.index].image.sprite, CellManager.Instance.cellData[cellSecond.index].textIndexMonstr.text,
                    CellManager.Instance.cellData[cellFirst.index].IEStartEffectLoyScale());
                CellManager.Instance.cellData[cellSecond.index].StartEffestRotation(index, sprite, text, CellManager.Instance.cellData[cellSecond.index].IEStartEffectLoyScale());

                cellFirst = null;
                cellSecond = null;

                if (!isFirst)
                {
                    isFirst = true;
                    CanvasTrainingLevel.StateStage5 = true;
                    for (int i = 0; i < CellManager.Instance.cellData.Count; i++)
                    {
                        CellManager.Instance.cellData[i].isReady = true;
                    }
                }

                BustManager.Instance.DeActiveBustButton(BustManager.Instance.exchangeCount, false, BustManager.Instance.imageExchange, false);
            }
        }

        private bool CheckIndex(CellGame checkItemData)
        {
            if (checkItemData.column != 0 && checkItemData.column != 4)
            {
                var result = Mathf.Abs(cellFirst.index - checkItemData.index);
                if (result == 4 || result == 5 || result == 6 || result == 1)
                {
                    return true;
                }
                else return false;
            }
            else
            {
                var result = cellFirst.index - checkItemData.index;
                if (checkItemData.column == 0)
                {
                    if (Mathf.Abs(result) == 5 || result == 1 || result == -4 || result == 6)
                    {
                        return true;
                    }
                    else return false;
                }
                else if (checkItemData.column == 4)
                {
                    if (Mathf.Abs(result) == 5 || result == -1 || result == -6 || result == 4)
                    {
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
        }

        private void MoveAndriod()
        {
            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
                {
                    Ray ray = CellManager.Instance.GetPointToRay(touch.position);
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        if (hit.collider.TryGetComponent(out CellGame data))
                        {
                            if (cellFirst == null)
                            {
                                cellFirst = data;
                                StartCoroutine(cellFirst.IEStartEffectUpScale());
                            }
                            else if (cellSecond == null)
                            {
                                if (cellFirst.index != data.index && CheckIndex(data))
                                {
                                    cellSecond = data;
                                    StartCoroutine(cellSecond.IEStartEffectUpScale());
                                }
                            }
                        }
                    }
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    if (cellSecond == null)
                        return;

                    AudioManager2248.Instance.StartEffectClip(AudioManager2248.Instance.audioContainer.echangesCell);

                    var saveCellFirst = CellManager.Instance.cellData[cellFirst.index];
                    var saveCellSecond = CellManager.Instance.cellData[cellSecond.index];

                    var index = CellManager.Instance.cellData[cellFirst.index].indexMonstr;
                    var sprite = CellManager.Instance.cellData[cellFirst.index].image.sprite;
                    var text = CellManager.Instance.cellData[cellFirst.index].textIndexMonstr.text;

                    CellManager.Instance.cellData[cellFirst.index].StartEffestRotation(CellManager.Instance.cellData[cellSecond.index].indexMonstr,
                        CellManager.Instance.cellData[cellSecond.index].image.sprite, CellManager.Instance.cellData[cellSecond.index].textIndexMonstr.text,
                        CellManager.Instance.cellData[cellFirst.index].IEStartEffectLoyScale());
                    CellManager.Instance.cellData[cellSecond.index].StartEffestRotation(index, sprite, text, CellManager.Instance.cellData[cellSecond.index].IEStartEffectLoyScale());

                    cellFirst = null;
                    cellSecond = null;

                    BustManager.Instance.DeActiveBustButton(BustManager.Instance.exchangeCount, false, BustManager.Instance.imageExchange, false);
                }
            }
        }

        public void ResetData()
        {
            Action action = cellFirst == null ? null : RestCellFirst;
            action?.Invoke();
            action = cellSecond == null ? null : RestCellSecond;
            action?.Invoke();

        }

        private void RestCellFirst()
        {
            StartCoroutine(cellFirst.IEStartEffectLoyScale());
            cellFirst = null;
        }

        private void RestCellSecond()
        {
            StartCoroutine(cellSecond.IEStartEffectLoyScale());
            cellSecond = null;
        }
    }
}