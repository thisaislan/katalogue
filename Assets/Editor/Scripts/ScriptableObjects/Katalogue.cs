using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

using M = Thisaislan.Katalogue.Editor.Metas.Metadata;

namespace Thisaislan.Katalogue.Editor.ScriptableObjects
{
    [
        CreateAssetMenu(
                fileName = M.KatalogueDataFileName,
                menuName = M.KatalogueAssetMenuDataName,
                order = M.KatalogueAssetMenuDataOrder
            )
    ]
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
        internal List<KatalogueData> katalogueDatas = new();

        public virtual void Init()
        {
            for (int index = 0; index < katalogueDatas.Count; index++)
            {
                if (katalogueDatas[index].IsPrefabNull())
                {
                    katalogueDatas.RemoveAt(index);
                    index--;
                }
            }

            katalogueDatas = katalogueDatas.OrderBy(data => data.prefab.name).ToList();

            KatalalogueEditor.PersistData(this);
        }

        public bool AddNewKatalogueData(GameObject prefab, string description)
        {
            if (CanAddNewPrefab(prefab))
            {
                try
                {
                    katalogueDatas.Insert(GetIndexOfNewData(prefab.name), GetNewDataToAdd(prefab, description));
                    KatalalogueEditor.PersistData(this);
                    NotifyKatalogueEditor();
                }
                catch
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        
        public bool RemoveKatalogueData(KatalogueData katalogueData)
        {
            if (katalogueData != null)
            {
                try
                {
                    katalogueDatas.Remove(katalogueData);
                    KatalalogueEditor.PersistData(this);
                    NotifyKatalogueEditor();
                }
                catch {
                    return false;
                }
                return true;
            }
            return false;
        }

        protected void NotifyKatalogueEditor() =>
            KatalalogueEditor.NotifyDataChanged(this);
        
        private KatalogueData GetNewDataToAdd(GameObject prefabToAdd, string descriptionToAdd) =>
            new() {
                prefab = prefabToAdd,
                description = descriptionToAdd
            };

        private bool CanAddNewPrefab(GameObject prefabToAdd) =>
            katalogueDatas.FindIndex(data => data.prefab.Equals(prefabToAdd)) == -1;
        
        private int GetIndexOfNewData(string prefabName) =>
            ~katalogueDatas.Select(data => data.prefab.name).ToList().BinarySearch(prefabName);

    }
}