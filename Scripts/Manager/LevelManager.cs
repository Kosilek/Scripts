using DredPack;
using System.Collections;
using DredPack.Help;
using UnityEngine;
using Kosilek.UI;
using System.Collections.Generic;
using Kosilek.MonstrPack;
using NaughtyAttributes;

namespace Kosilek.Manager
{
    public class LevelManager : SimpleSingleton<LevelManager>
    {
        public GameObject level;

        public List<MonstrPack.MonstrPack> monstrPack;

        #region Animation
        [Foldout("Animation")]
        [SerializeField] private float moveSpeed;
        [Foldout("Animation")]
        [SerializeField] private AnimationCurve animCurve;
        private bool isActie = false;
        #endregion end Animation

        #region Instance
        public void InitializingLevel()
        {
            if (isActie)
                return;

            var BackGround = CanvasManager.Instance.canvasGame.transform.GetChild(0);
            var AllCell = CanvasManager.Instance.canvasGame.transform.GetChild(2);
            BackGround.transform.localScale = Vector3.zero;
            AllCell.transform.localScale = Vector3.zero;
            CanvasManager.Instance.canvasGame.Open();
            StartCoroutine(IE());

            IEnumerator IE()
            {
                isActie = true;
                StartCoroutine(Lerper.LerpVector3IE(BackGround.transform.localScale, Vector3.one, moveSpeed, animCurve, _ => BackGround.transform.localScale = _));
                yield return StartCoroutine(Lerper.LerpVector3IE(AllCell.transform.localScale, Vector3.one, moveSpeed, animCurve, _ => AllCell.transform.localScale = _));
                CellManager.Instance.RaycastIsActive = true;
                isActie = false;
            }
        }

        public void InitializingEndLevel()
        {
            if (isActie)
                return;

            var BackGround = CanvasManager.Instance.canvasGame.transform.GetChild(0);
            var AllCell = CanvasManager.Instance.canvasGame.transform.GetChild(2);
            CellManager.Instance.RaycastIsActive = false;
            StartCoroutine (IE());

            IEnumerator IE()
            {
                isActie = true;
                StartCoroutine(Lerper.LerpVector3IE(BackGround.transform.localScale, new Vector3(0.3f, 0.3f, 0.3f), moveSpeed, animCurve, _ => BackGround.transform.localScale = _));
                yield return StartCoroutine(Lerper.LerpVector3IE(AllCell.transform.localScale, new Vector3(0.3f, 0.3f, 0.3f), moveSpeed, animCurve, _ => AllCell.transform.localScale = _));
                isActie = false;
                CanvasManager.Instance.canvasGame.Close();
            }
        }
        #endregion end Instance
    }
}