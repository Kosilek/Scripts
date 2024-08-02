using Kosilek.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kosilek.Data
{
    [Serializable]
    public class CellManagerData
    {
        [HideInInspector] public List<CellDataTransform> cellDataTransform;
        public List<Transform> cellTransform;
        [HideInInspector] public int stepColumn;
        public List<CellGame> column;
        [HideInInspector] public List<int> indexColumn;
        [HideInInspector] public int counter;
        [HideInInspector] public List<int> counterList;
        [HideInInspector] public List<int> positionList;
        [HideInInspector] public bool isStartFor;
    }
}