using DredPack;
using DredPack.UI;
using Kosilek.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Kosilek.Manager
{
    public class GameManager : SimpleSingleton<GameManager>
    {
        public int indexMonstrPack;

        public bool isGame = false;

        public bool isGamePause = false;

        public void GamePause(bool isPause, Window window)
        {
            Action action = isPause ? window.Open : window.Close;
            action.Invoke();
            CellManager.Instance.RaycastIsActive = !isPause;
            isGamePause = isPause;
        }

        public void GamePause(bool isPause)
        {
            CellManager.Instance.RaycastIsActive = !isPause;
            isGamePause = isPause;
        }
    }
}