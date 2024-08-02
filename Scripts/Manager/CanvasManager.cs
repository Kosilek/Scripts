using DredPack;
using DredPack.UI;
using Kosilek.Manager;
using System;
using System.Collections;
using UnityEngine;

namespace Kosilek.UI
{
    public class CanvasManager : SimpleSingleton<CanvasManager>
    {
        public ActionWindow actionWindow = new ActionWindow();

        public Window canvasMenu;
        public Window canvasSettings;
        public Window canvasProfile;
        public Window canvasGame;
        public Window canvasShop;
        public Window canvasMonstrThreePack;
        public Window canvasMonstrSee;
        public Window canvasPause;
        public Window canvasLose;
        public Window canvasNewMonstrUnlock;
        public Window canvasTutoritalLevel;

        public CanvasGame canvasGameScr;
        public CanvasNewMonstrUnlock canvasNewMonstrUnlockScr;
        public SeeMonstrPrize seeMonstrPrize;
        public BestScore bestScore;
        public CanvasPrize canvasPrize;
        public CanvasMenu canvasMenuScr;
        public CanvasTrainingLevel tutoritalLevel;

        internal void DebugLogError(string errorInfo)
        {
            Debug.LogError(errorInfo);
        }

        internal void DebugLog(string debugInfo)
        {
            Debug.Log(debugInfo);
        }

        public void LoseGame()
        {
            CellManager.Instance.RaycastIsActive = false;
            canvasGame.GetComponent<CanvasGroup>().blocksRaycasts = false;
            canvasLose.Open();
            canvasLose.GetComponent<CanvasLose>().StartAnim();
        }
    }

    public class ActionWindow
    {
        public void OpenWindow(bool isOpen, Window window, string windowName)
        {
            try
            {
                ActionOpen(isOpen, window.Close, window.Open);
            }
            catch
            {
                CanvasManager.Instance.DebugLogError($"Error: It is impossible to perform an action with the window, {windowName}");
            }
        }

        private Action ReturnMenuAction(bool isOpen)
        {
            Action menuAction = isOpen ? CanvasManager.Instance.canvasMenu.Close : CanvasManager.Instance.canvasMenu.Open;
            return menuAction;
        }

        private void ActionOpen(bool isOpen, Action callTrue, Action callFalse)
        {
            Action action = !isOpen ? callTrue : callFalse;
            Action menuAction = ReturnMenuAction(isOpen);

            action();
            menuAction();
        }
    }
}
