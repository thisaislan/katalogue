using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

using M = Thisaislan.Katalogue.Editor.Metas.Metadata;
using C = Thisaislan.Katalogue.Editor.Constants.Consts;
using KatalogueData = Thisaislan.Katalogue.Editor.ScriptableObjects.Katalogue.KatalogueData;

namespace Thisaislan.Katalogue.Editor
{
    internal static class KatalalogueEditor
    {
        internal delegate void DataChangedDelegate(ScriptableObjects.Katalogue katalogue);
        internal static event DataChangedDelegate DataChangedEvent;
        
        #region DrawRegion

        internal static void DrawDataElement(
            KatalogueData katalogueData,
            Action actionOnDeletion,
            Action actionOnEdition
        ) =>
            DrawDataElement(katalogueData, true, actionOnDeletion, actionOnEdition);

        internal static void DrawDataElement(
            KatalogueData katalogueData,
            Action actionOnSelection
        ) =>
            DrawDataElement(katalogueData, false, actionOnSelection, null);

        internal static void DrawSearch(ref string value, Action actionOnSearch, Action actionOnClea)
        {
            GUILayout.BeginHorizontal();

            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            EditorGUI.BeginChangeCheck();
            
            value = EditorGUILayoutSearchTextField(value, M.SearchFieldMinWidth);

            if (EditorGUI.EndChangeCheck())
            {
                value = value.Trim();
                actionOnSearch.Invoke();
            }

            GUI.enabled = !string.IsNullOrWhiteSpace(value);
            
            if (GUILayoutXButton())
            {
                actionOnClea.Invoke();
                value = string.Empty;
                CleanFocus();
            }

            GUI.enabled = true;

            GUILayout.EndHorizontal();
            
            EditorGUILayoutMediumSpace();
            
            GUILayout.EndHorizontal();
            
            EditorGUILayoutSmallSpace();
        }
        
         private static void DrawDataElement(
            KatalogueData katalogueData,
            bool isEditionMode,
            Action actionOnClick,
            Action actionOnEdition
        )
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

            EditorGUILayoutSmallSpace();

            DrawDataElementImageSection(katalogueData, isEditionMode);

            EditorGUILayout.BeginVertical();
            
            EditorGUILayoutSmallSpace();

            DrawDataElementHeaderSection(katalogueData, isEditionMode, actionOnClick);
            
            DrawDataElementDescriptionSection(katalogueData, isEditionMode, actionOnEdition);

            EditorGUILayout.EndVertical();
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayoutMediumSpace();

            EditorGUILayout.EndHorizontal();
        }

        private static void DrawDataElementDescriptionSection(
            KatalogueData katalogueData,
            bool isEditionMode,
            Action actionOnEdition
        )
        {
            if (isEditionMode)
            {
                EditorGUI.BeginChangeCheck();

                katalogueData.description = EditorGUILayoutTextField(
                    katalogueData.description, M.DescriptionElementOnShoHeight,
                    M.DescriptionPrefabToAddMinWidth
                );

                if (EditorGUI.EndChangeCheck())
                {
                    actionOnEdition.Invoke();
                }
            }
            else
            {
                EditorGUILayoutExhibitionTextField(
                    katalogueData.description,
                    M.DescriptionElementOnShoHeight,
                    M.DescriptionPrefabToAddMinWidth
                );
            }

            EditorGUILayoutSmallSpace();
        }

        private static void DrawDataElementHeaderSection(
            KatalogueData katalogueData,
            bool isEditionMode,
            Action actionOnClick
        )
        {
            EditorGUILayout.BeginHorizontal();
            
            try
            {
                GUILayout.Label(katalogueData.prefab.name, isEditionMode ? EditorStyles.miniLabel : EditorStyles.label);
            }
            catch
            {
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndHorizontal();
                throw;
            }

            GUILayout.FlexibleSpace();

            if (isEditionMode)
            {
                if (GUILayoutXButton())
                {
                    actionOnClick.Invoke();
                    CleanFocus();
                }
            }
            else
            {
                if (GUILayoutSelectButton()) { actionOnClick.Invoke(); }

                if (Event.current.type == EventType.Repaint &&
                    GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
                {
                    SelectLabelHover();
                }

                EditorGUILayoutSmallSpace();
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayoutSmallSpace();
        }

        private static void DrawDataElementImageSection(KatalogueData katalogueData, bool isEditionMode)
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayoutSmallSpace();

            if (GUILayoutSquareButton(
                    isEditionMode ? M.RectTextureElementOnShowEditionSize : M.RectTextureElementOnShowExhibitionSize,
                    katalogueData.prefab))
            {
                Selection.activeObject = katalogueData.prefab;
            }

            if (Event.current.type == EventType.Repaint &&
                GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
            {
                GUIOpenHover();
            }
            
            EditorGUILayout.EndVertical();

            EditorGUILayoutMediumSpace();
        }

        #endregion //DrawRegion

        #region LayoutRegion
        
        internal static bool GUILayoutAddPrefabButton() =>
            GUILayoutButton(
                    C.AddButtonPrefabToAddLabel, 
                    M.AddButtonPrefabToAddHeight,
                    M.AddButtonPrefabToAddMinWidth
                );

        internal static bool GUILayoutButton(string label, float height, float minWidth) =>
            GUILayout.Button(
                    label, 
                    GUILayout.Height(height), 
                    GUILayout.MinWidth(minWidth),
                    GUILayoutMaxWidth()
                );

        internal static void EditorGUIDefaultLine() =>
            EditorGUILine(Color.gray, M.DefaultLineHeight, M.DefaultLineMinWidth);

        internal static void EditorGUILine(Color color, float height, float minWidth)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUI.DrawRect(EditorGUILayoutControlRect(false, height, minWidth), color);
            EditorGUILayoutMediumSpace();
            EditorGUILayout.EndHorizontal();
        }

        internal static Rect EditorGUILayoutControlRect(bool hasLabel, float height, float minWidth) =>
            EditorGUILayout.GetControlRect(
                    hasLabel,
                    height,
                    GUILayout.Height(height),
                    GUILayout.MinWidth(minWidth),
                    GUILayoutMaxWidth()
                );

        internal static GameObject EditorGUILayoutObjectField(GameObject prefab, float minWidth) =>
            EditorGUILayout.ObjectField(
                    prefab, 
                    typeof(GameObject), 
                    false, 
                    GUILayout.MinWidth(minWidth),
                    GUILayoutMaxWidth()
                ) as GameObject;

        internal static string EditorGUILayoutTextField(string value, float height, float minWidth) =>
             EditorGUILayout.TextArea(
                     value,
                     EditorStyles.textArea ,
                     GUILayout.Height(height),
                     GUILayout.MinWidth(minWidth),
                     GUILayoutMaxWidth()
             );

        internal static void EditorGUILayoutExhibitionTextField(string value, float height, float minWidth)
        {
            GUI.enabled = false;
            
            GUILayout.Label(
                value,
                EditorStyles.textArea, 
                GUILayout.Height(height),
                GUILayout.MinWidth(minWidth),
                GUILayoutMaxWidth());
            
            GUI.enabled = true;
        }

        internal static string EditorGUILayoutSearchTextField(string value, float minWidth) =>
             EditorGUILayout.TextArea(
                 value,
                 EditorStyles.toolbarSearchField,
                 GUILayout.MinWidth(minWidth),
                 GUILayoutMaxWidth()
             );

         internal static bool GUILayoutXButton() =>
             GUILayout.Button(
                     C.XButtonLabel,
                     EditorStyles.iconButton,
                     GUILayout.Height(M.XButtonHeight),
                     GUILayout.Width(M.XButtonWidth)
                 );

         internal static bool GUILayoutSelectButton() =>
             GUILayout.Button(
                     C.SelectButtonLabel,
                     EditorStyles.miniButtonMid,
                     GUILayout.Height(M.SelectButtonHeight),
                     GUILayout.Width(M.SelectButtonWidth)
                 );
         
         internal static bool GUILayoutRefreshButton() =>
                 GUILayout.Button(
                     C.RefreshButtonLabel,
                     EditorStyles.miniButtonMid,
                     GUILayout.Height(M.RefreshButtonHeight),
                    GUILayout.Width(M.RefreshButtonWidth)
                 );

         internal static void GUILayoutSquareLabel(float size, GameObject prefab) =>
            GUI.Label(GUILayoutUtilitySquareRect(size), AssetPreview.GetAssetPreview(prefab));
         
         internal static bool GUILayoutSquareButton(float size, GameObject prefab) =>
             GUI.Button(GUILayoutUtilitySquareRect(size), AssetPreview.GetAssetPreview(prefab));
         
         internal static void GUIOpenHover() =>
             GUI.Label(
                     GUILayoutUtility.GetLastRect(),
                     C.OpenButtonLabel,
                     EditorStyles.toolbarButton
                 );

         internal static void GUILayoutSquareLabelEmptyBox(float size) =>
            GUI.Label(GUILayoutUtilitySquareRect(size), string.Empty, EditorStyles.helpBox);

         internal static Rect GUILayoutUtilitySquareRect(float size) =>
            GUILayoutUtility.GetRect(size, size);
        
        internal static void EditorGUILayoutSmallSpace() =>
            EditorGUILayout.Space(M.GUILayoutSpaceSmall);
        
        internal static void EditorGUILayoutMediumSpace() =>
            EditorGUILayout.Space(M.GUILayoutSpaceMedium);
        
        internal static void EditorGUILayoutBigSpace() =>
            EditorGUILayout.Space(M.GUILayoutSpaceBig);

        internal static GUILayoutOption GUILayoutMaxWidth() =>
            GUILayout.MaxWidth(Int32.MaxValue);

        #endregion //LayoutRegion

        #region UtilsRegion

        internal static void NotifyDataChanged(ScriptableObjects.Katalogue katalogue) =>
            DataChangedEvent?.Invoke(katalogue);
        
        internal static void CleanDataList(ScriptableObjects.Katalogue katalogue)
        {
            if (katalogue != null)
            {
                for (int index = 0; index < katalogue.katalogueDatas.Count; index++)
                {
                    if (katalogue.katalogueDatas[index].IsPrefabNull())
                    {
                        katalogue.katalogueDatas.RemoveAt(index);
                        index--;
                    }
                }

                katalogue.katalogueDatas = katalogue.katalogueDatas.OrderBy(data => data.prefab.name).ToList();

                PersistData(katalogue);
            }
        }

        internal static void PersistData(ScriptableObjects.Katalogue katalogue)
        {
            EditorUtility.SetDirty(katalogue);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        internal static void CleanFocus() =>
            GUI.FocusControl(null);

        internal static string[] GetKatalogueGuids() =>
            AssetDatabase.FindAssets(
                M.FindKatalogueAssetsFilter, 
                new[] { $"{M.MenuItemGenerateMainFolderName}/{M.MenuItemGenerateApplicationFolderName}" }
            );
        
        internal static void CheckFolders()
        {
            CheckApplicationFolder();
            DeleteInnerFolder();
            CreateInnerFolder();
        }
        
        private static void SelectLabelHover() =>
            GUI.Label(
                    GUILayoutUtility.GetLastRect(),
                    Selection.activeTransform == null ? 
                        C.SelectButtonAddOnSceneLabelHover : 
                        C.SelectButtonAddOnObjectLabelHover,
                    GUI.skin.button
            );

        private static void CreateInnerFolder() =>
            AssetDatabase.CreateFolder(
                $"{M.MenuItemGenerateMainFolderName}/{M.MenuItemGenerateApplicationFolderName}", 
                M.MenuItemGenerateInnerFolderName
            );

        private static void DeleteInnerFolder() =>
            AssetDatabase.DeleteAsset(
                $"{M.MenuItemGenerateMainFolderName}/" +
                $"{M.MenuItemGenerateApplicationFolderName}/" +
                $"{M.MenuItemGenerateInnerFolderName}"
            );

        private static void CheckApplicationFolder() =>
            CheckFolder(M.MenuItemGenerateMainFolderName, M.MenuItemGenerateApplicationFolderName);

        private static void CheckFolder(string parentFolder, string folder)
        {
            if (!IsValidFolder($"{parentFolder}/{folder}")) { AssetDatabase.CreateFolder(parentFolder, folder); }
        }

        private static bool IsValidFolder(string path) =>
            AssetDatabase.IsValidFolder(path);
        
        #endregion  //UtilsRegion
        
    }
}