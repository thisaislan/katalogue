using UnityEditor;

using M = Thisaislan.Katalogue.Editor.Metas.Metadata;

namespace Thisaislan.Katalogue.Editor.Processes
{
    internal class KataloguePostprocessor : AssetPostprocessor
    {

        private static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths
        )
        {
            KatalalogueEditor.NotifyPrefabChanged(
                GetPathTonNotify(importedAssets, deletedAssets, movedAssets, movedFromAssetPaths)
            );
            
            if(WasKatalogueImported(importedAssets)){  KatalalogueEditor.CheckFolders(); }
            
        }

        private static string GetPathTonNotify(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            if (importedAssets.Length > 0) { return importedAssets[0]; }
            if (deletedAssets.Length > 0) { return deletedAssets[0]; }
            if (movedAssets.Length > 0) { return movedAssets[0]; }
            return movedFromAssetPaths[0];
        }

        private static bool WasKatalogueImported(string[] importedAssets) =>
            importedAssets.Length > 0 && importedAssets[0].Contains(M.PackageFolderName);

    }
}