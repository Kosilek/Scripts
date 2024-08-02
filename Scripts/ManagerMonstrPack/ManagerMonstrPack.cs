using DredPack;
using Kosilek.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kosilek.MonstrPack
{
    public class ManagerMonstrPack : SimpleSingleton<ManagerMonstrPack>
    {
        public List<GameObject> prefabeMonstrPack;
     //   [HideInInspector]
        public MonsterCollection monsterCollection;

        #region Start
        private void Awake()
        {
            InitializeMonstrPack();

            for (int i = 0; i < monsterCollection.monstrPack.Count; i++)
            {
                monsterCollection.monstrPack[i].numberMonstr = i;
            }
        }

        private void InitializeMonstrPack()
        {
            var monstrPack = Instantiate(prefabeMonstrPack[GameManager.Instance.indexMonstrPack]);
            monstrPack.name = "MonstrCollection";
            monstrPack.transform.SetParent(gameObject.transform);
            monsterCollection = monstrPack.GetComponent<MonsterCollection>();
        }
        #endregion
    }
}
