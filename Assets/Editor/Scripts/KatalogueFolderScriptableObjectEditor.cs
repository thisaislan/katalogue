using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using M = Thisaislan.Katalogue.Editor.Metas.Metadata;
using C = Thisaislan.Katalogue.Editor.Constants.Consts;
using KatalogueData = Thisaislan.Katalogue.Editor.ScriptableObjects.Katalogue.KatalogueData;

namespace Thisaislan.Katalogue.Editor
{
    [CustomEditor(typeof(ScriptableObjects.KatalogueFolder))]
    internal class KatalogueFolderScriptableObjectEditor : UnityEditor.Editor
    {

        private ScriptableObjects.KatalogueFolder katalogueFolder;

        private List<KatalogueData> katalogueDatas;

        private Vector2 scrollPos;

        private bool shouldShowTip;

        private string searchString;

        #region OverrideRegion

        public void OnEnable()
        {
            AddEventListener();
            InitData();
            InitDataList();
        }

        public override void OnInspectorGUI()
        {
            DrawHeaderSection();
            
            if (!shouldShowTip)
            {
                DrawFolderSection();
                DrawSearchSection();
                DrawDataListSection();
            }
        }

        private void OnDestroy() =>
            RemoveEventListener();

        #endregion //OverrideRegion

        #region DrawRegion

        private void DrawHeaderSection()
        {
            GUI.enabled = shouldShowTip;
            
            shouldShowTip = EditorGUILayout.Foldout(
                    shouldShowTip, 
                    shouldShowTip ? C.KatalogueFileTipCloseLabel : C.KatalogueFileTipLabel,
                    EditorStyles.foldoutHeader 
                );
            
            GUI.enabled = true;
            
            if (shouldShowTip)
            {
                GUILayout.Label(C.KatalogueFolderipDescription, EditorStyles.wordWrappedLabel);
            }

            KatalalogueEditor.EditorGUILayoutMediumSpace();
        }

        private void DrawFolderSection()
        {
            KatalalogueEditor.EditorGUIDefaultLine();

            EditorGUILayout.BeginHorizontal();

            KatalalogueEditor.EditorGUILayoutSmallSpace();

            GUILayout.Label(
                C.KatalogueFolderLabel, EditorStyles.boldLabel, GUILayout.Width(M.LabelPrefabToAddSize)
                );

            KatalalogueEditor.EditorGUILayoutMediumSpace();

            if (KatalalogueEditor.EditorGUILayoutOpenFolderButton(katalogueFolder.folderPath))
            {
                SetSelectedFolderPath(
                    EditorUtility.OpenFolderPanel(
                        C.KatalogueFolderTitleSelection,
                        M.KatalogueFolderSelectionStart,
                        string.Empty
                ));

                GUIUtility.ExitGUI();
            }

            KatalalogueEditor.EditorGUILayoutMediumSpace();
            
            EditorGUILayout.EndHorizontal();
            
            KatalalogueEditor.EditorGUILayoutSmallSpace();
            
            KatalalogueEditor.EditorGUIDefaultLine();
        }

        private void DrawSearchSection() =>
            KatalalogueEditor.DrawSearch(ref searchString, SearchOnDataList, SetDataList);

        private void DrawDataListSection()
        {
            EditorGUILayout.BeginVertical();

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos,GUILayout.Height(M.KatalogueFolderScrollHeight));
            
            try
            {
                foreach (var katalogueData in katalogueDatas)
                {
                    KatalalogueEditor.DrawDataElement(
                            katalogueData,
                            null,
                            () => { KatalalogueEditor.PersistData(katalogueFolder); }
                        );
                }
            }
            catch { ResetDataList(); }
            
            EditorGUILayout.EndScrollView();
            
            EditorGUILayout.EndVertical();
        }

        #endregion //DrawRegion

        #region UtilsRegion

        private void InitData() =>
            katalogueFolder = (ScriptableObjects.KatalogueFolder)target;
        
        private void InitDataList()
        {
            if (katalogueFolder != null)
            {
                katalogueFolder.Init();
                SetDataList();
            }
        }
        
        private void ResetDataList()
        {
            if (katalogueFolder != null)
            {
                katalogueFolder.Init();
                UpdateDataListBySearch();
            }
        }

        private void SetDataList() =>
            katalogueDatas = katalogueFolder.katalogueDatas;

        private void SearchOnDataList()
        {
            katalogueDatas = katalogueFolder.katalogueDatas.FindAll(
                katalogueData => katalogueData.prefab.name.Contains(
                    searchString, StringComparison.InvariantCultureIgnoreCase));
        }
        
        private void UpdateDataListBySearch()
        {
            if (string.IsNullOrWhiteSpace(searchString)) { SetDataList(); }
            else { SearchOnDataList(); }
        }
        
        private void SetSelectedFolderPath(string selectedPath)
        {
            if (!string.IsNullOrWhiteSpace(selectedPath) && selectedPath.Contains(Application.dataPath))
            {
                SetFolderPath(RemoveApplicationPath(selectedPath));
            }
        }

        private static string RemoveApplicationPath(string selectedPath) => 
            selectedPath.Replace(Application.dataPath, string.Empty);

        private void SetFolderPath(string folderPath)
        {
            if (!string.IsNullOrWhiteSpace(folderPath) && folderPath != katalogueFolder.folderPath)
            {
                katalogueFolder.folderPath = folderPath;
                KatalalogueEditor.PersistData(katalogueFolder);
                InitDataList();
            }
        }
        
        private void AddEventListener() =>
            KatalalogueEditor.prefabChangedEvent += OnPrefabChangedEvent;
        
        private void RemoveEventListener() =>
            KatalalogueEditor.prefabChangedEvent -= OnPrefabChangedEvent;

        private void OnPrefabChangedEvent(string prefabPath)
        {
            if (!prefabPath.Contains(AssetDatabase.GetAssetPath(this)))
            {
                if (prefabPath.Contains(M.MenuItemGenerateMainFolderName + katalogueFolder.folderPath))
                {
                    ResetDataList();
                }
            }
        }

        #endregion //UtilsRegion

    }
}