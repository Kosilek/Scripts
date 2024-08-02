using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kosilek.Ad
{
    public class IntervalAdvertising : MonoBehaviour
    {
        #region const
        private float DELAY = 150f;
        #endregion end const

        private void Start()
        {
            StartCoroutine(IEIntervalAd());
        }

        IEnumerator IEIntervalAd()
        {
            var a = 0;
            var time = 0f;

            while (a < 1)
            {
                time += 1f;
              //  Debug.Log("TIme = " + time);
                if (time > DELAY)
                {
                    AdManager.Instance.ShowInterstitial();
                    time = 0f;
                }

                if (AdManager.Instance.isNoAd)
                    a = 10;

                yield return new WaitForSeconds(1f);
            }
        }
    }
}
