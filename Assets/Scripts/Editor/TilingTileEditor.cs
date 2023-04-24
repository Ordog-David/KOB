using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TilingTile))]
public class TilingTileEditor : Editor
{
    private TilingTile Tile => target as TilingTile;
    public Texture2D texture;

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        Tile.width = EditorGUILayout.IntField("Pattern width", Tile.width);
        Tile.height = EditorGUILayout.IntField("Pattern height", Tile.height);

        texture = EditorGUILayout.ObjectField("Texture", texture, typeof(Texture2D), false) as Texture2D;

        if (EditorGUI.EndChangeCheck())
        {
            RepaintTiles();
        }
    }

    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        if (Tile.sprites != null && Tile.sprites.Length > 0 && Tile.sprites[0] != null)
        {
            var spriteTexture = Tile.sprites[0].texture;
            var spritePreview = AssetPreview.GetAssetPreview(spriteTexture);
            var spriteIcon = new Texture2D(width, height);
            EditorUtility.CopySerialized(spritePreview, spriteIcon);
            EditorUtility.SetDirty(target);
            return spriteIcon;
        }

        return base.RenderStaticPreview(assetPath, subAssets, width, height);
    }

    private void RepaintTiles()
    {
        var texturePath = AssetDatabase.GetAssetPath(texture);
        Tile.sprites = AssetDatabase.LoadAllAssetsAtPath(texturePath).OfType<Sprite>().ToArray();

        EditorUtility.SetDirty(target);
        SceneView.RepaintAll();
    }
}
