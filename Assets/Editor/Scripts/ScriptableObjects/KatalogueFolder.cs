using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

using M = Thisaislan.Katalogue.Editor.Metas.Metadata;

namespace Thisaislan.Katalogue.Editor.ScriptableObjects
{
    [
        CreateAssetMenu(
            fileName = M.KatalogueFolderDataFileName,
            menuName = M.KatalogueFodlerAssetMenuDataName,
            order = M.KatalogueFolderAssetMenuDataOrder
        )
    ]
    public class KatalogueFolder : Katalogue
    {
        
        [SerializeField]
        public string folderPath;
        
        public override void Init()
        {
            if (!string.IsNullOrWhiteSpace(folderPath))
            {
                ResetKatalogueDataList(GetPrefasbGuids());
                KatalalogueEditor.PersistData(this);
            }
        }

        private void ResetKatalogueDataList(string[] guids) =>
            katalogueDatas = GetKatalogueDatas(GetPrefabs(guids));

        private List<KatalogueData> GetKatalogueDatas(List<GameObject> prefabsOnFolder) =>
            (from prefab in prefabsOnFolder
                join katalogueData in katalogueDatas on prefab equals katalogueData.prefab into groupJoin
                from joinKatalogueData in groupJoin.DefaultIfEmpty()
                orderby prefab.name
                select new KatalogueData
                {
                    prefab = prefab,
                    description = joinKatalogueData?.description ?? string.Empty
                }).ToList();
        
        private List<GameObject> GetPrefabs(string[] guids)
        {
            var prefads = new List<GameObject>();
            
            foreach (var guid in guids)
            {
                prefads.Add(AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid)));
            }

            return prefads;
        }

        private string[] GetPrefasbGuids() =>
            AssetDatabase.FindAssets(M.FindPrefabsAssetsFilter, new[] { M.MenuItemGenerateMainFolderName + folderPath });
        
    }
}