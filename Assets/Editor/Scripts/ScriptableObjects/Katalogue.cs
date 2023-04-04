using System;
using UnityEngine;
using System.Collections.Generic;

using M = Thisaislan.Katalogue.Editor.Metas.Metadata;

namespace Thisaislan.Katalogue.Editor.ScriptableObjects
{
    [CreateAssetMenu(fileName = M.DataFileName, menuName = M.AssetMenuDataName, order = M.AssetMenuDataOrder)]
    public class Katalogue : ScriptableObject
    {
        [Serializable]
        public class KatalogueData
        {
            
            [SerializeField]
            public GameObject prefab;

            [SerializeField]
            public string description;

            public bool IsPrefabNull() =>
                prefab == null;

        }

        [SerializeField]
        internal List<KatalogueData> katalogueDatas = new List<KatalogueData>();

        public void AddNewKatalogueData(KatalogueData katalogueData, int indexAt)
        {
            katalogueDatas.Insert(indexAt, katalogueData);
            NotifyEditorWindow();
        }
        
        public void RemoveKatalogueData(KatalogueData katalogueData)
        {
            katalogueDatas.Remove(katalogueData);
            NotifyEditorWindow();
        }
        
        private void NotifyEditorWindow() =>
            KatalalogueEditor.NotifyDataChanged(this);

    }
}