using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveInDebugPanel : MonoBehaviour
{
    public int maxTouch = 20;
    public int countTouch = 0;
    public GameObject inDebugPanel;
    private Coroutine cor;

    public void ActionButton()
    {
        if (cor == null)
            cor = StartCoroutine(IE());

        countTouch++;

        if (countTouch == maxTouch)
            inDebugPanel.SetActive(true);
    }

    private IEnumerator IE()
    {
        yield return new WaitForSeconds(10f);
        countTouch = 0;
    }
}
