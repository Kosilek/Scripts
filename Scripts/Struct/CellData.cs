using System;
using UnityEngine;

namespace Kosilek.Data
{
    [Serializable]
    public struct CellData
    {
        public int index;
        public int monstPack;
    }

    [Serializable]
    public struct CellDataTransform
    {
        public int indexColumn;
        public Transform cellTransform;

        public CellDataTransform(int indexColumn, Transform cellTransform)
        {
            this.indexColumn = indexColumn;
            this.cellTransform = cellTransform;
        }
    }

    public struct CellDataRe
    {
        public ulong indexMonstr;
        public Sprite sprite;

        public CellDataRe(ulong indexMonstr, Sprite sprite)
        {
            this.indexMonstr = indexMonstr;
            this.sprite = sprite;
        }
    }
}