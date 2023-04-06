using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using M = Thisaislan.Katalogue.Editor.Metas.Metadata;
using C = Thisaislan.Katalogue.Editor.Constants.Consts;
using KatalogueData = Thisaislan.Katalogue.Editor.ScriptableObjects.Katalogue.KatalogueData;

namespace Thisaislan.Katalogue.Editor
{
    [CustomEditor(typeof(ScriptableObjects.Katalogue))]
    internal class KatalogueScriptableObjectEditor : UnityEditor.Editor
    {

        private ScriptableObjects.Katalogue katalogue;

        private List<KatalogueData> katalogueDatas;

        private GameObject prefabToAdd;
        
        private string descriptionToAdd;

        private Vector2 scrollPos;

        private bool shouldShowTip;

        private string searchString;

        #region OverrideRegion

        public void OnEnable()
        {
            InitData();
            InitDataList();
        }

        public override void OnInspectorGUI()
        {
            DrawHeaderSection();
            
            if (!shouldShowTip)
            {
                DrawAddPrefabSection();
                DrawSearchSection();
                DrawDataListSection();
            }
        }
        
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
                GUILayout.Label(C.KatalogueFileTipDescription, EditorStyles.wordWrappedLabel);
            }

            KatalalogueEditor.EditorGUILayoutMediumSpace();
        }

        private void DrawAddPrefabSection()
        {
            DrawAddPrefabSectionFirstLine();
            DrawAddPrefabSectionBody();
            DrawAddPrefabSectionButton();
        }

        private void DrawAddPrefabSectionFirstLine()
        {
            KatalalogueEditor.EditorGUIDefaultLine();

            EditorGUILayout.BeginHorizontal();

            KatalalogueEditor.EditorGUILayoutSmallSpace();

            GUILayout.Label(
                C.KataloguePrefabToAddLabel, EditorStyles.boldLabel, GUILayout.Width(M.LabelPrefabToAddSize)
                );

            KatalalogueEditor.EditorGUILayoutMediumSpace();

            prefabToAdd = KatalalogueEditor.EditorGUILayoutObjectField(prefabToAdd, M.FieldPrefabToAddMinWidth);

            KatalalogueEditor.EditorGUILayoutMediumSpace();

            EditorGUILayout.EndHorizontal();
            
            KatalalogueEditor.EditorGUILayoutSmallSpace();
            
            KatalalogueEditor.EditorGUIDefaultLine();
        }

        private void DrawAddPrefabSectionBody()
        {
            EditorGUILayout.BeginHorizontal();

            KatalalogueEditor.EditorGUILayoutSmallSpace();

            if (prefabToAdd == null) { KatalalogueEditor.GUILayoutSquareLabelEmptyBox(M.RectTexturePrefabToAddSize); }
            else { KatalalogueEditor.GUILayoutSquareLabel(M.RectTexturePrefabToAddSize, prefabToAdd); }

            KatalalogueEditor.EditorGUILayoutMediumSpace();

            EditorGUILayout.BeginVertical();

            GUILayout.Label(C.KatalogueDescriptionLabel, EditorStyles.miniBoldLabel);

            descriptionToAdd = KatalalogueEditor.EditorGUILayoutTextField(
                descriptionToAdd, M.DescriptionPrefabToAddHeight,
                M.DescriptionPrefabToAddMinWidth
            );

            EditorGUILayout.EndVertical();

            KatalalogueEditor.EditorGUILayoutBigSpace();

            EditorGUILayout.EndHorizontal();

            KatalalogueEditor.EditorGUILayoutMediumSpace();
        }

        private void DrawAddPrefabSectionButton()
        {
            EditorGUILayout.BeginHorizontal();
            
            if (KatalalogueEditor.GUILayoutAddPrefabButton())
            {
                if (prefabToAdd != null)
                {
                    if (AddNewData()){
                    
                        UpdateDataListBySearch();
                        CleanFields();
                    
                        KatalalogueEditor.CleanFocus();
                    }
                }
            }
            
            KatalalogueEditor.EditorGUILayoutMediumSpace();

            EditorGUILayout.EndHorizontal();
            
            KatalalogueEditor.EditorGUILayoutSmallSpace();
        }

        private void DrawSearchSection() =>
            KatalalogueEditor.DrawSearch(ref searchString, SearchOnDataList, SetDataList);

        private void DrawDataListSection()
        {
            EditorGUILayout.BeginVertical();

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos,GUILayout.Height(M.KatalogueScrollHeight));
            
            try
            {
                foreach (var katalogueData in katalogueDatas)
                {
                    KatalalogueEditor.DrawDataElement(
                            katalogueData,
                            () => { DeleteDataFromList(katalogueData); },
                            () => { KatalalogueEditor.PersistData(katalogue); }
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
            katalogue = (ScriptableObjects.Katalogue)target;
        
        private void InitDataList()
        {
            if (katalogue != null)
            {
                katalogue.Init();
                SetDataList();
            }
        }
        
        private void ResetDataList()
        {
            if (katalogue != null)
            {
                katalogue.Init();
                UpdateDataListBySearch();
            }
        }

        private void SetDataList() =>
            katalogueDatas = katalogue.katalogueDatas;

        private void SearchOnDataList()
        {
            katalogueDatas = katalogue.katalogueDatas.FindAll(
                katalogueData => katalogueData.prefab.name.Contains(
                    searchString, StringComparison.InvariantCultureIgnoreCase));
        }

        private bool AddNewData() =>
            katalogue.AddNewKatalogueData(prefabToAdd, descriptionToAdd);
        
        private void UpdateDataListBySearch()
        {
            if (string.IsNullOrWhiteSpace(searchString)) { SetDataList(); }
            else { SearchOnDataList(); }
        }

        private void CleanFields()
        {
            prefabToAdd = null;
            descriptionToAdd = null;
        }
        
        private void DeleteDataFromList(KatalogueData katalogueData)
        {
            katalogue.RemoveKatalogueData(katalogueData);
            UpdateDataListBySearch();
        }
        
        #endregion //UtilsRegion

    }
}