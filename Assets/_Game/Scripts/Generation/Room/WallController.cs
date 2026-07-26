using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Game.Scripts.Generation.Room
{
public class WallController
{
    private readonly RoomConfig _config;
    private readonly TileController _tileController;

    public WallController(RoomConfig config, TileController tileController)
    {
        _config = config;
        _tileController = tileController;
    }

    public void Set(Tilemap tilemap)
    {
        switch (_config.WallTiles.Length)
        {
            case 0:
                return;
            case 1:
                SetSimpleLine(tilemap, _config.WallTiles[0]);
                break;
            default:
                SetRandomLine(tilemap, _config.WallTiles);
                break;
        }
    }

    private void SetSimpleLine(Tilemap tilemap, TileBase tile)
    {
        var halfSize = _config.Size / 2;
        for (var x = -halfSize + 1; x <= halfSize; x++)
        {
            AddTile(tilemap, new Vector3Int(x, halfSize, 0), tile);
        }
        
        for (var y = -halfSize + 1; y < halfSize; y++)
        {
            AddTile(tilemap, new Vector3Int(halfSize, y, 0), tile);
        }
        
        _tileController.UpdateCollider(tilemap);
        tilemap.RefreshAllTiles();
    }
    
    private void SetRandomLine(Tilemap tilemap, TileBase[] tiles)
    {
        var halfSize = _config.Size / 2;
        for (var x = -halfSize + 1; x <= halfSize; x++)
        {
            var rand = Random.Range(0, tiles.Length);
            AddTile(tilemap, new Vector3Int(x, halfSize, 0), tiles[rand]);
        }
        
        for (var y = -halfSize + 1; y < halfSize; y++)
        {
            var rand = Random.Range(0, tiles.Length);
            AddTile(tilemap, new Vector3Int(halfSize, y, 0), tiles[rand]);
        }
        
        _tileController.UpdateCollider(tilemap);
        tilemap.RefreshAllTiles();
    }

    private void AddTile(Tilemap tilemap, Vector3Int cellPos, TileBase tile)
    {
        tilemap.SetTile(cellPos, tile);
    }
}
}