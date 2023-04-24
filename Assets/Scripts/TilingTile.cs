using System;
using UnityEngine;
using UnityEngine.Tilemaps;

// Generic visual tile for filling map with parts of a texture.
[Serializable]
[CreateAssetMenu(fileName = "New Tiling Tile", menuName = "2D/Tiles/Tiling Tile")]
public class TilingTile : Tile
{
    public Sprite[] sprites;
    public int width;
    public int height;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);

        var x = Modulo(position.x, width);
        var y = height - 1 - Modulo(position.y, height);
        var offset = y * height + x;

        if (sprites != null && sprites.Length > offset && sprites[offset] != null)
        {
            tileData.sprite = sprites[offset];
        }
    }

    private int Modulo(int n, int m)
    {
        return (n % m + m) % m;
    }
}
