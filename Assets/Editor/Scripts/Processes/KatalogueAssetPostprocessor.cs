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
            if(WasKatalogueImported(importedAssets)){  KatalalogueEditor.CheckFolders(); }
        }
        
        private static bool WasKatalogueImported(string[] importedAssets) =>
            importedAssets.Length > 0 && importedAssets[0].Contains(M.PackageFolderName);

    }
}