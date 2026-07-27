using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Game.Scripts.Generation
{
public class TileController
{
    public void AddTile(Tilemap tilemap, Vector3Int cellPos, TileBase tile)
    {
        tilemap.SetTile(cellPos, tile);
        tilemap.SetColliderType(cellPos, Tile.ColliderType.Sprite);
    }
}
}