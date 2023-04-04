using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

using M = Thisaislan.Katalogue.Editor.Metas.Metadata;
using C = Thisaislan.Katalogue.Editor.Constants.Consts;

namespace Thisaislan.Katalogue.Editor
{
    internal static class KatalogueEditorMenu
    {

        #region MenuItemOptionsRegion

        [MenuItem(M.MenuItemGenerateMenu + M.MenuItemGenerateMenuShortcut, priority = M.MenuItemGenerateMenuPriority)]
        private static void MenuItemGenerateMenuItems() =>
            GenerateMenuItems();

        [MenuItem(M.MenuItemOpenMainWindowMenu, priority = M.MenuItemOpenMainWindowPriority)]
        private static void OpenMainWindow() =>
            EditorWindow.GetWindow<KatalogueEditorWindow>()?.Init();

        #endregion //MenuItemOptionsRegion
        
        private static void GenerateMenuItems()
        {
            KatalalogueEditor.CheckFolders();

            if (GenerateMenuItems(KatalalogueEditor.GetKatalogueGuids()) > 0) { ShowSuccessMessageOnConsole(); }
            else { ShowFileNotFoundMessageOnConsole(); }
        }

        private static void ShowSuccessMessageOnConsole() =>
            ShowMessageOnConsole(C.DebugMessageFilesCreatedSuccessfully);

        private static void ShowFileNotFoundMessageOnConsole() =>
            ShowMessageOnConsole(C.DebugMessageKatalogueFilesNotFound);

        private static void ShowMessageOnConsole(string message)
        {
            var stackTraceLogType = Application.GetStackTraceLogType(LogType.Warning);
            
            Application.SetStackTraceLogType(LogType.Warning, StackTraceLogType.None);
            
            Debug.LogWarning(message);

            Application.SetStackTraceLogType(LogType.Warning, stackTraceLogType);
        }

        private static int GenerateMenuItems(string[] guidArray)
        {
            foreach (var guid in guidArray)
            {
                CreateEditorMenuFile(AssetDatabase.GUIDToAssetPath(guid));
            }

            return guidArray.Length;
        }

        private static void CreateEditorMenuFile(string kataloguePath) =>
            CreateEditorFile(kataloguePath, GetKatalogueFileName(kataloguePath));

        private static string GetKatalogueFileName(string kataloguePath) =>
            AssetDatabase.LoadAssetAtPath<ScriptableObjects.Katalogue>(kataloguePath).name;
        private static void CreateEditorFile(string kataloguePath, string realFileName) =>
            CreateFile(kataloguePath, GetFolderName(), GetFileName(realFileName), realFileName);

        private static string GetFileName(string katalogueName) =>
            M.ScriptFileSuffixName + katalogueName.Replace(" ","");
        
        private static string GetFolderName() =>
            $"{M.MenuItemGenerateApplicationFolderName}/{M.MenuItemGenerateInnerFolderName}/";

        private static void CreateFile(string kataloguePath, string folderName, string realFileName, string katalogueName)
        {
            WriteScript(GetScriptFilePath(folderName, realFileName), realFileName, katalogueName, kataloguePath);
            ImportScript(folderName, realFileName);
        }
        
        private static string GetScriptFilePath(string folderName, string fileName) =>
            $"{Application.dataPath}/{folderName}/{fileName}{M.ScriptFileType}";

        private static void WriteScript(string scriptFilePath, string fileName, string realkatalogueName, string filePath) =>
            File.WriteAllText(scriptFilePath, M.GetScriptFile(fileName, realkatalogueName, filePath), Encoding.UTF8); 

        private static void ImportScript(string folderName, string fileName) =>
            AssetDatabase.ImportAsset($"{M.MenuItemGenerateMainFolderName}/{folderName}/{fileName}{M.ScriptFileType}");
        
    }
}