using System.Text;
using UnityEngine;

using C = Thisaislan.Katalogue.Editor.Constants.Consts;

namespace Thisaislan.Katalogue.Editor.Metas
{
    internal static class Metadata
    {
        
        internal const int KatalogueAssetMenuDataOrder = 1;
        internal const int KatalogueFolderAssetMenuDataOrder = 2;
        
        internal const int AddButtonPrefabToAddHeight = 20;
        internal const int AddButtonPrefabToAddMinWidth = 360;
        
        internal const int DefaultLineHeight = 1;
        internal const int DefaultLineMinWidth = 360;
        
        internal const int DescriptionElementOnShoHeight = 56;
        internal const int DescriptionPrefabToAddHeight = 72;
        internal const int DescriptionPrefabToAddMinWidth = 230;
        
        internal const int FieldPrefabToAddMinWidth = 226;
        internal const int FieldFolderSelectionMinWidth = 226;
        
        internal const int GUILayoutSpaceBig = 20;
        internal const int GUILayoutSpaceMedium = 10;
        internal const int GUILayoutSpaceSmall = 5;
        
        internal const int LabelPrefabToAddSize = 100;
        
        internal const int MenuItemGenerateMenuPriority = 1;
        internal const int MenuItemOpenMainWindowPriority = 2;
        
        internal const int PopupSelectKatalogueMinWidth = 322;
        
        internal const int RectTextureElementOnShowEditionSize = 76;
        internal const int RectTextureElementOnShowExhibitionSize = 83;
        internal const int RectTexturePrefabToAddSize = 100;
        
        internal const int RefreshButtonHeight = 18;
        internal const int RefreshButtonWidth = 50;
        
        internal const int KatalogueScrollHeight = 470;
        internal const int KatalogueFolderScrollHeight = 600;
        
        internal const int SearchFieldMinWidth = 350;
        
        internal const int SelectButtonHeight = 18;
        internal const int SelectButtonWidth = 100;
        
        internal const int XButtonHeight = 10;
        internal const int XButtonWidth = 20;
        
        internal const string AssetRootMenuDataName = "Katalogue";
        
        internal const string KatalogueAssetMenuDataName = AssetRootMenuDataName + "/New Katalogue";
        internal const string KatalogueFodlerAssetMenuDataName = AssetRootMenuDataName + "/New Katalogue Folder";

        internal const string KatalogueDataFileName = "Katalogue";
        internal const string KatalogueFolderDataFileName = "Katalogue Folder";

        internal const string MenuItemGenerateApplicationFolderName = "Katalogue";
        internal const string MenuItemGenerateInnerFolderName = "Editor";
        internal const string MenuItemGenerateMainFolderName = "Assets";
        internal const string MenuItemGenerateMenu = MenuItemGMainPath + "/Generate menus items";
        internal const string MenuItemGenerateMenuShortcut = " #%K";
        internal const string MenuItemGMainPath = "Tools/Katalogue";
        internal const string MenuItemOpenMainWindowMenu = MenuItemGMainPath + "/Open " + C.MainWindowTitle;
        
        internal const string PackageFolderName = "katalogue";
        
        internal const string RegisterCreatedObjectUndoName = "Create ";
        
        internal const string ScriptFileSuffixName = "Katalogue";
        internal const string ScriptFileType = ".cs";
        
        internal static readonly string FindKatalogueAssetsFilter = $"t:{nameof(Katalogue)}";
        internal static readonly string FindPrefabsAssetsFilter = $"t:{nameof(GameObject)}";

        internal const string KatalogueFolderSelectionStart = "Assets";

        internal static string GetScriptFile(string fileName, string realFileName, string filePath)
        {
            var stringBuilder = new StringBuilder();
            
            stringBuilder.AppendLine("// This class is Auto-Generated");
            stringBuilder.AppendLine("using UnityEngine;");
            stringBuilder.AppendLine("using UnityEditor;");
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("namespace Thisaislan.Katalogue.Editor.Generated");
            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine("    internal static class " + fileName);
            stringBuilder.AppendLine("    {");
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("        [MenuItem(\"GameObject/" + MenuItemGenerateApplicationFolderName + "/" + realFileName + "\", true)]");
            stringBuilder.AppendLine("        private static bool MenuItemValidate()");
            stringBuilder.AppendLine("        {");
            stringBuilder.AppendLine("            var fileExists = AssetDatabase.LoadAssetAtPath<ScriptableObjects.Katalogue>(\"" + filePath +"\") != null;");
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("            if (!fileExists)");
            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine("                var stackTraceLogType = Application.GetStackTraceLogType(LogType.Warning);");
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("                Application.SetStackTraceLogType(LogType.Warning, StackTraceLogType.None);");
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("                Debug.LogWarning(\""+ C.DebugMessageSuffix + C.DebugMessageFileNotFound + realFileName + C.DebugMessageFileNotFoundAction + "\");");
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("                Application.SetStackTraceLogType(LogType.Warning, stackTraceLogType);");
            stringBuilder.AppendLine("            }");
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("            return fileExists;");
            stringBuilder.AppendLine("        }");
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("        [MenuItem(\"GameObject/" + MenuItemGenerateApplicationFolderName + "/" + realFileName + "\")]");
            stringBuilder.AppendLine("        private static void MenuItem() =>");
            stringBuilder.AppendLine("                ScriptableObject.CreateInstance<KatalogueEditorWindow>()?.");
            stringBuilder.AppendLine("                        Init(AssetDatabase.LoadAssetAtPath<ScriptableObjects.Katalogue>(\"" + filePath + "\"));");
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("    }");
            stringBuilder.AppendLine("}");

            return stringBuilder.ToString();
        }

    }
}
