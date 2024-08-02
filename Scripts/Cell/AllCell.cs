using Kosilek.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllCell : MonoBehaviour
{
    public void Restart()
    {
        CellManager.Instance.DeleteData();
        GameManager.Instance.GamePause(false);
    }

    public void RestartGame()
    {
        CellManager.Instance.RestartGame();
    }
}
