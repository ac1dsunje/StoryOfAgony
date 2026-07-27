using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Generation.Room
{
public class FloorController
{
    private readonly RoomConfig _config;
    private readonly TileController _tileController;

    public event Action<Vector3Int> OnSetBox;

    public FloorController(RoomConfig config, TileController tileController)
    {
        _tileController = tileController;
        _config = config;
    }
    
    public void Set(Tilemap tileMap)
    {
        switch (_config.FloorTiles.Length)
        {
            case 0:
                return;
            case 1:
                SetSimpleRange(tileMap, _config.FloorTiles[0]);
                break;
            default:
                SetRandomRange(tileMap, _config.FloorTiles);
                break;
        }
    }
    
    private void SetSimpleRange(Tilemap tilemap, TileBase tile)
    {
        var halfSize = _config.Size / 2;
        for (var x = -halfSize; x < halfSize - 1; x++)
        {
            for (var y = -halfSize; y < halfSize - 1; y++)
            {
                var pos =  new Vector3Int(x, y, 0);
                TrySetBox(pos);
                _tileController.AddTile(tilemap, pos, tile);
            }
        }
        tilemap.RefreshAllTiles();
    }

    private void SetRandomRange(Tilemap tilemap, TileBase[] tiles)
    {
        var halfSize = _config.Size / 2;
        for (var x = -halfSize; x < halfSize - 1; x++)
        {
            for (var y = -halfSize; y < halfSize - 1; y++)
            {
                var rand = Random.Range(0, tiles.Length);
                var pos = new Vector3Int(x, y, 0);
                TrySetBox(pos);
                _tileController.AddTile(tilemap, pos, tiles[rand]);
            }
        }
        tilemap.RefreshAllTiles();
    }
    
    private void TrySetBox(Vector3Int cellPos)
    {
        if (Random.Range(0, 100) >= _config.ChanceToSpawnBox) return;

        OnSetBox?.Invoke(cellPos);
    }
}
}