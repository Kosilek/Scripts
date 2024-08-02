using Kosilek.Data;
using Kosilek.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Kosilek.UI;

public class ButtonBustHummer : ButtonCntr
{
    public CellGame cellGame;

    public bool isAction = false;

    private bool isFirst = false;

    #region Touch
    private Touch touch;
    #endregion

    //Delete
    public List<CellManagerData> cellData = new();

    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        if (!GameManager.Instance.isGame)
            return;
        if (GameManager.Instance.isGamePause)
            return;
        if (BustManager.Instance.isExchange)
            return;
        if (!CanvasTrainingLevel.isActiveFIrstBust)
            return;

        Action action = BustManager.Instance.isHummer ? InAction : Action;
        action.Invoke();

    }

    private void Action()
    {
        if (isAction)
            return;
        BustManager.Instance.isHummer = true;
        BustManager.Instance.ActiveBusterHammer();
    }

    private void InAction()
    {
        BustManager.Instance.DeActiveBustButton(BustManager.Instance.hammerCount, true, BustManager.Instance.imageHummer, BustManager.Instance.isActiveHammer);
    }

    private void Update()
    {
        if (!BustManager.Instance.isActiveHammer)
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
        if (isAction)
            return;

        if (Input.GetMouseButton(0))
        {
            Ray ray = CellManager.Instance.GetPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out CellGame data))
                {
                    if (data.isReady)
                        cellGame = data;
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (cellGame == null)
                return;
            AudioManager2248.Instance.StartEffectClip(AudioManager2248.Instance.audioContainer.destoyHummer);

            CellManager.Instance.indexCell.Add(cellGame.index);
            CellManager.Instance.columnCell.Add(cellGame.column);
            BustManager.Instance.isActiveHammer = false;
            isAction = true;
            cellGame.DestroyObject(cellGame.index, cellGame.column, cellGame.indexColumn);
            cellGame = null;

            StartCoroutine(IE());

            CellManager.Instance.RemoveCellList();
            CellManager.Instance.MoveAllCell();
            CellManager.Instance.SetListIndex();
            CellManager.Instance.SortCell();
            CellManager.Instance.SaveCell();
            CellManager.Instance.DataReset();


            IEnumerator IE()
            {
                var delay = CellManager.DELAY_DESTROY;
                yield return new WaitForSeconds(delay);
                BustManager.Instance.DeActiveBustButton(BustManager.Instance.hammerCount, true, BustManager.Instance.imageHummer, false);
                isAction = false;
                if (!isFirst)
                {
                    isFirst = true;
                    CanvasTrainingLevel.StateStage5 = true;
                    for (int i = 0; i < CellManager.Instance.cellData.Count; i++)
                    {
                        CellManager.Instance.cellData[i].isReady = true;
                    }
                }
              
            }
        }
    }

    private void MoveAndriod()
    {
        if (isAction)
            return;

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
                        cellGame = data;
                    }
                }
            }
            if (touch.phase == TouchPhase.Ended)
            {
                if (cellGame == null)
                    return;

                AudioManager2248.Instance.StartEffectClip(AudioManager2248.Instance.audioContainer.destoyHummer);

                CellManager.Instance.indexCell.Add(cellGame.index);
                CellManager.Instance.columnCell.Add(cellGame.column);

                BustManager.Instance.isActiveHammer = false;
                isAction = true;
                cellGame.DestroyObject(cellGame.index, cellGame.column, cellGame.indexColumn);
                cellGame = null;

                StartCoroutine(IE());

                CellManager.Instance.RemoveCellList();
                CellManager.Instance.MoveAllCell();
                CellManager.Instance.SetListIndex();
                CellManager.Instance.SortCell();
                CellManager.Instance.SaveCell();
                CellManager.Instance.DataReset();


                IEnumerator IE()
                {
                    var delay = CellManager.DELAY_DESTROY;
                    yield return new WaitForSeconds(delay);
                    BustManager.Instance.DeActiveBustButton(BustManager.Instance.hammerCount, true, BustManager.Instance.imageHummer, false);
                    isAction = false;
                }
            }
        }
    }

    private IEnumerator IEEndMove()
    {

        yield return new WaitForSeconds(CellManager.DELAY_DESTROY);
        CellManager.Instance.RemoveCellList();
        CellManager.Instance.MoveAllCell();
        CellManager.Instance.SetListIndex();
        CellManager.Instance.SortCell();
        CellManager.Instance.SaveCell();
        CellManager.Instance.DataReset();
    }
}
