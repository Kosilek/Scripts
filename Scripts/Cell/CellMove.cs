using DredPack.Help;
using Kosilek.Data;
using Kosilek.Manager;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellMove : MonoBehaviour
{
    #region AnimationMove
    [Foldout("AnimMove")]
    [SerializeField] private float moveSpeed;
    [Foldout("AnimMove")]
    [SerializeField] private AnimationCurve animCurve;
    #endregion

    #region Move
    public void StartMoveOneStep(int index, List<CellDataTransform> cellDataTransform)
    {
        StartCoroutine(IECellMove(cellDataTransform[index].cellTransform));
    }

    public void StartMoveManyStep(List<int> index, List<CellDataTransform> cellDataTransform)
    {
        StartCoroutine(IECellManyMove());

        IEnumerator IECellManyMove()
        {
            yield return new WaitForSeconds(CellManager.DELAY_DESTROY);

            for (int i = 0; i < index.Count; i++)
            {
                yield return StartCoroutine(IECellMove(cellDataTransform[index[i]].cellTransform));
            }
        }
    }

    private IEnumerator IECellMove(Transform newTransform)
    {
        yield return StartCoroutine(Lerper.LerpVector3IE(transform.position, newTransform.position, moveSpeed, animCurve, _ => transform.position = _));
    }
    #endregion
}
