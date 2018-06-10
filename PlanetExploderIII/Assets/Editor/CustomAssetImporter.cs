using UnityEditor;

public class CustomAssetImporter : AssetPostprocessor
{
    private void OnPreprocessModel()
    {
        //http://www.sarpersoher.com/a-custom-asset-importer-for-unity/
        var importer = assetImporter as ModelImporter;

        importer.globalScale = 100;
        importer.importAnimation = false;
        importer.importCameras = false;
        importer.importLights = false;
        importer.importMaterials = false;
        importer.animationType = ModelImporterAnimationType.None;
    }
}
