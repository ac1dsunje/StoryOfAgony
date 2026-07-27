using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Game.Scripts.Generation.Room
{
public class FireController
{

    private readonly RoomConfig _config;
    private readonly TileController _tileController;

    public FireController(RoomConfig config, TileController tileController)
    {
        _tileController = tileController;
        _config = config;
    }
    
    public IEnumerator FillFloor(Tilemap tileMap)
    {
        tileMap.ClearAllTiles();
        tileMap.RefreshAllTiles();
        var layer = 0;

        while (layer < _config.Size / 2)
        {
            yield return new WaitForSeconds(5f);

            var min = - _config.Size / 2 + layer;
            var max = _config.Size / 2 - 1 - layer;

            for (var x = min; x <= max; x++)
            {
                _tileController.AddTile(tileMap, new Vector3Int(x, max, 0), _config.FireTile);
                _tileController.AddTile(tileMap, new Vector3Int(x, min, 0), _config.FireTile);
            }

            for (var y = min + 1; y < max; y++)
            {
                _tileController.AddTile(tileMap, new Vector3Int(min, y, 0), _config.FireTile);
                _tileController.AddTile(tileMap, new Vector3Int(max, y, 0), _config.FireTile);
            }
            layer++;
            tileMap.RefreshAllTiles();
        }
    }
}
}