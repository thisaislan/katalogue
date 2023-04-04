using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

using M = Thisaislan.Katalogue.Editor.Metas.Metadata;
using C = Thisaislan.Katalogue.Editor.Constants.Consts;
using KatalogueData = Thisaislan.Katalogue.Editor.ScriptableObjects.Katalogue.KatalogueData;

namespace Thisaislan.Katalogue.Editor
{
    public class KatalogueEditorWindow : EditorWindow
    {
        
        private ScriptableObjects.Katalogue katalogueInUse;
        
        private List<ScriptableObjects.Katalogue> katalogueList;
        private List<KatalogueData> katalogueDataList;
        
        private string[] katalogueNamesList;

        private string searchString;
        private Vector2 scrollPos;
        
        private bool isSingleMode;
        private int selectedPositionPopup;
        
        #region SettingsRegion

        public void Init(ScriptableObjects.Katalogue katalogue)
        {
            Init(true, katalogue);
            SetWindowTitle(C.EditorWindowTitleSuffix + katalogue.name);
            ShowUtility();
            InitKatalogueDataList();
        }
        
        public void Init()
        {
            if (isSingleMode)
            {
                Close();
                GetWindow<KatalogueEditorWindow>()?.Init();
            }
            else
            {
                SetWindowTitle(C.MainWindowTitle);
                InitKatalogueLists();
                Init(false, katalogueList.Count > 0 ? katalogueList[0] : null);
                InitKatalogueDataList();
            }
        }
        
        private void Init(bool isSingleMode, ScriptableObjects.Katalogue katalogue)
        {
            this.katalogueInUse = katalogue;
            this.searchString = string.Empty;
            this.isSingleMode = isSingleMode; 
            this.selectedPositionPopup = 0;

            AddEventListener();
        }
        
        private void InitKatalogueDataList()
        {
            KatalalogueEditor.CleanDataList(katalogueInUse);
            SetDataList();
        }
        
        private void SetDataList()
        {
            if (katalogueInUse != null) { katalogueDataList = katalogueInUse.katalogueDatas; }
        }

        private void InitKatalogueLists()
        {
            InitKatalogueList();
            InitKatalogueNameList();
        }
        
        private void SetWindowTitle(string title) =>
            titleContent = new GUIContent(title);
        
        private void InitKatalogueList() =>
            katalogueList = GetKatalogue(
                GetKataloguePaths(KatalalogueEditor.GetKatalogueGuids())
            ).ToList();

        private void InitKatalogueNameList() =>
            katalogueNamesList = GetKatalogueNames();

        private string[] GetKatalogueNames() =>
            katalogueList.Select(katalogue => katalogue.name).ToArray();
        
        private IEnumerable<string> GetKataloguePaths(string[] katalogueGuids) =>
            katalogueGuids.Select(guid => AssetDatabase.GUIDToAssetPath(guid));

        private IEnumerable<ScriptableObjects.Katalogue> GetKatalogue(IEnumerable<string> kataloguePaths) =>
            kataloguePaths.Select(path => AssetDatabase.LoadAssetAtPath<ScriptableObjects.Katalogue>(path));

        private void AddEventListener() =>
            KatalalogueEditor.DataChangedEvent += OnDataChangedEvent;

        #endregion //SettingsRegion

        #region OverrideRegion

        private void OnGUI()
        {
            if (!isSingleMode) { DrawPopupSection(); }
            
            DrawSearchSection();
            DrawDataListSection();

        }

        private void OnDestroy() =>
            KatalalogueEditor.DataChangedEvent -= OnDataChangedEvent;

        #endregion //OverrideRegion

        #region DrawRegion

        private void DrawPopupSection()
        {
            EditorGUILayout.BeginVertical();
            
            KatalalogueEditor.EditorGUILayoutSmallSpace();
            
            EditorGUILayout.BeginHorizontal();
            
            KatalalogueEditor.EditorGUILayoutSmallSpace();

            if (KatalalogueEditor.GUILayoutRefreshButton()) { ResetKatalogueLists(); }

            EditorGUI.BeginChangeCheck();
            
            try
            {
                selectedPositionPopup = EditorGUILayout.Popup(
                    selectedPositionPopup, 
                    katalogueNamesList, 
                    GUILayout.MinWidth(M.PopupSelectKatalogueMinWidth),
                    KatalalogueEditor.GUILayoutMaxWidth()
                ); 
            }
            catch { ResetKatalogueLists(); }

            if (EditorGUI.EndChangeCheck())
            {
                var selectedKatalogue = katalogueList[selectedPositionPopup];

                if (selectedKatalogue != null)
                {
                    katalogueInUse = selectedKatalogue;
                    InitKatalogueDataList();
                }
                else
                {
                    ResetKatalogueLists();
                }
                
                KatalalogueEditor.CleanFocus();
            }

            KatalalogueEditor.EditorGUILayoutMediumSpace();
            
            EditorGUILayout.EndHorizontal();
            
            KatalalogueEditor.EditorGUILayoutSmallSpace();
            
            EditorGUILayout.EndHorizontal();
        }
        
        private void DrawSearchSection() =>
            KatalalogueEditor.DrawSearch(ref searchString, SearchOnDataList, SetDataList);
        
        private void DrawDataListSection()
        {
            EditorGUILayout.BeginVertical();

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            
            try
            {
                foreach (var katalogueData in katalogueDataList)
                {
                    KatalalogueEditor.DrawDataElement(katalogueData, () => CreateNewPrefab(katalogueData.prefab));
                }
            }
            catch
            {
                ResetDataList();
            }
            
            EditorGUILayout.EndScrollView();
            
            EditorGUILayout.EndVertical();
        }

        #endregion //DrawRegion

        #region UtilsRegion
        
        private void OnDataChangedEvent(ScriptableObjects.Katalogue katalogue)
        {
            if (katalogueInUse != null && katalogueInUse.Equals(katalogue))
            {
                ResetDataList();
            }
        }
        
        private void ResetKatalogueLists()
        {
            InitKatalogueLists();
            SetTheFirstKatalogueToUse();
            InitKatalogueDataList();
        }

        private void SetTheFirstKatalogueToUse()
        {
            if (katalogueList.Count > 0)
            {
                katalogueInUse = katalogueList[0];
            }
            else
            {
                katalogueInUse = null;
                katalogueDataList = null;
            }

            selectedPositionPopup = 0;
        }

        private void SearchOnDataList() =>
            katalogueDataList = katalogueInUse.katalogueDatas.FindAll(
                katalogueData => katalogueData.prefab.name.Contains(
                    searchString, StringComparison.InvariantCultureIgnoreCase));

        private void UpdateDataListBySearch()
        {
            if (string.IsNullOrWhiteSpace(searchString)) { SetDataList(); }
            else { SearchOnDataList(); }
        }

        private void ResetDataList()
        {
            KatalalogueEditor.CleanDataList(katalogueInUse);
            UpdateDataListBySearch();
        }

        private void CreateNewPrefab(GameObject prefab)
        {
            var newGameObject = Instantiate(prefab);
            
            newGameObject.name = prefab.name;
            
            var lastSelected = Selection.activeTransform;

            if (lastSelected != null) { GameObjectUtility.SetParentAndAlign(newGameObject, lastSelected.gameObject); }
            else { GameObjectUtility.SetParentAndAlign(newGameObject, null); }

            Undo.RegisterCreatedObjectUndo(newGameObject, M.RegisterCreatedObjectUndoName + newGameObject.name);
            
            Selection.activeObject = newGameObject;

            if (isSingleMode) { Close(); }
        }
        
        #endregion //UtilsRegion
        
    }
}