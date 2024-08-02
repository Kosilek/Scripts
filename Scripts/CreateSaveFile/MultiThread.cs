using System.Threading.Tasks;
using System;
using DredPack;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Kosilek.SaveAndLoad
{
    public class MultiThread : MonoBehaviour
    {
        public Task loadAllDataTask;

        /// <summary>
        /// 0 - settings Volume
        /// 1 - cell Data
        /// 2 - other Data
        /// </summary>
        private readonly Dictionary<SaveTaskType, Task> _saveTasks = new Dictionary<SaveTaskType, Task>
        {
            [SaveTaskType.SettingsVolume] = null,
            [SaveTaskType.CellData] = null,
            [SaveTaskType.OtherData] = null,
            [SaveTaskType.NameData] = null
        };

        public void SaveData(Action<bool, string, string> action, SaveTaskType taskType, bool isNewData, string path, string fileData)
        {
            if (_saveTasks.TryGetValue(taskType, out var task))
            {
                task = Task.Run(() =>
                {
                    action?.Invoke(isNewData, path, fileData);
                    _saveTasks[taskType] = null;
                });
                _saveTasks[taskType] = task;
            }
        }

        public void LoadData(Action action)
        {
            loadAllDataTask = Task.Run(() =>
            {
                action?.Invoke();
                loadAllDataTask = null;
            });
        }
    }
}