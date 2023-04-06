using M = Thisaislan.Katalogue.Editor.Metas.Metadata;

namespace Thisaislan.Katalogue.Editor.Constants
{
    internal static class Consts
    {
        
        internal const string DebugMessageSuffix = "Katalogue -> ";
        internal const string DebugMessageFilesCreatedSuccessfully = DebugMessageSuffix + "Menus generated successfully";
        internal const string DebugMessageKatalogueFilesNotFound = DebugMessageSuffix + "No menu was generated because " +
                                                                   "no file of type Katalogue was found in the " +
                                                                   "Katalogue folder";
        
        internal const string AddButtonPrefabToAddLabel = "Add";
        
        internal const string DebugMessageFileNotFound = "Katalogue file not found: ";
        
        internal const string DebugMessageFileNotFoundAction =
            ". Please try to generate menu items again (" + M.MenuItemGenerateMenu + ")."; 
        
        internal const string EditorWindowTitleSuffix = "Katalogue - ";
        
        internal const string KatalogueDescriptionLabel = "Description";
        
        internal const string KatalogueFileTipCloseLabel = "Click here to close tips";
        internal const string KatalogueFileTipLabel = "Click here for some tips";
        
        internal const string KataloguePrefabToAddLabel = "Prefab";
        
        internal const string KatalogueFolderLabel = "Folder";
        
        internal const string KatalogueFolderEmptySelection = "None (Folder)";
        internal const string KatalogueFolderTitleSelection = "Select a folder";
        internal const string KatalogueFolderFolderSelection = "None (Folder)";
        
        internal const string MainWindowTitle = "Katalogue window";
        
        internal const string OpenButtonLabel = "Open";
        
        internal const string RefreshButtonLabel = "↻";

        internal const string SelectButtonAddOnObjectLabelHover = "Add to Object";
        internal const string SelectButtonAddOnSceneLabelHover = "Add in Scene";
        internal const string SelectButtonLabel = "Select";
        
        internal const string XButtonLabel = "  ✕";
        
        internal const string KatalogueFileTipDescription = "\n" +
            "You can have as many Katalogues files as you like, these files can contain any Prefeb you want to put in" +
            " a catalog.\n\n" +
            "Try using Katalogue files to categorize your prefabs and always keep these files inside of the " +
            "Kalalogue folder or subfolders.\n\n" +
            "Use short descriptions with distinct information about each prefab.\n\n" +
            "Generate new menus every time you create, delete or rename a Katalogue file (" + M.MenuItemGenerateMenu +").\n\n" +
            "Files in the "+ M.PackageFolderName + "/" +M.MenuItemGenerateInnerFolderName + " folder are " +
            "auto-generated, but can be committed for ease.\n\n" +
            "Want to see all Katalogues files at once? Go to " + M.MenuItemOpenMainWindowMenu + ".\n\n" +
            "You can open a prefab just by clicking on its image.\n\n" +
            "Katalogue file will prevent the insertion of duplicate prefabs.\n\n" +
            "You can create a new GameObject in the scene or inside the selected game object." +
            " The Select button will hint this to you.\n\n" +
            "Enjoy!\n" +
            ": )";
        
        internal const string KatalogueFolderipDescription = "\n" +
            "If you want to create a catalog with all the prefabs inside one folder then this is the file you want.\n\n" +
            "You can have as many Katalogues Folders as you like.\n\n" +
            "Keep these files inside of the Kalalogue folder or subfolders.\n\n" +
            "Use short descriptions with distinct information about each prefab.\n\n" +
            "Generate new menus every time you create, delete or rename a Katalogue Folder (" + M.MenuItemGenerateMenu +").\n\n" +
            "Files in the "+ M.PackageFolderName + "/" +M.MenuItemGenerateInnerFolderName + " folder are " +
            "auto-generated, but can be committed for ease.\n\n" +
            "Want to see all Katalogues files at once? Go to " + M.MenuItemOpenMainWindowMenu + ".\n\n" +
            "You can open a prefab just by clicking on its image.\n\n" +
            "Katalogue Folder allow duplicated prefabs.\n\n" +
            "You can create a new GameObject in the scene or inside the selected game object." +
            " The Select button will hint this to you.\n\n" +
            "Enjoy!\n" +
            ": )";

    }
}