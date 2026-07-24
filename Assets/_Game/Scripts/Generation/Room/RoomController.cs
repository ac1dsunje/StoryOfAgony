using _Game.Scripts.Items.Box;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Generation.Room
{
public class RoomController: MonoBehaviour
{
    [SerializeField] private RoomConfig _config;
    [SerializeField] private Tilemap _floorMap;
    [SerializeField] private Tilemap _wallMap;
    [SerializeField] private GameObject _boxPrefab;

    private BoxItemConfig _boxItem;

    public void Build(BoxItemConfig boxItem) 
    {
        _boxItem = boxItem;
        
        SetFloor();
        SetWalls();
    }

    private void SetFloor()
    {
        switch (_config.FloorTiles.Length)
        {
            case 0:
                return;
            case 1:
                SetSimpleRange(_floorMap, _config.FloorTiles[0]);
                break;
            default:
                SetRandomRange(_floorMap, _config.FloorTiles);
                break;
        }
    }

    private void SetWalls()
    {
        switch (_config.WallTiles.Length)
        {
            case 0:
                return;
            case 1:
                SetSimpleLine(_wallMap, _config.WallTiles[0]);
                break;
            default:
                SetRandomLine(_wallMap, _config.WallTiles);
                break;
        }
    }

    private void SetSimpleLine(Tilemap tilemap, TileBase tile)
    {
        var halfSize = _config.Size / 2;
        for (var x = -halfSize + 1; x <= halfSize; x++)
        {
            tilemap.SetTile(new Vector3Int(x, halfSize, 0), tile);
        }
        
        for (var y = -halfSize + 1; y < halfSize; y++)
        {
            tilemap.SetTile(new Vector3Int(halfSize, y, 0), tile);
        }
        
        tilemap.RefreshAllTiles();
    }
    
    private void SetRandomLine(Tilemap tilemap, TileBase[] tiles)
    {
        var halfSize = _config.Size / 2;
        for (var x = -halfSize + 1; x <= halfSize; x++)
        {
            var rand = Random.Range(0, tiles.Length);
            tilemap.SetTile(new Vector3Int(x, halfSize, 0), tiles[rand]);
        }
        
        for (var y = -halfSize + 1; y < halfSize; y++)
        {
            var rand = Random.Range(0, tiles.Length);
            tilemap.SetTile(new Vector3Int(halfSize, y, 0), tiles[rand]);
        }
        
        tilemap.RefreshAllTiles();
    }
    
    private void SetSimpleRange(Tilemap tilemap, TileBase tile)
    {
        var halfSize = _config.Size / 2;
        for (var x = -halfSize; x < halfSize - 1; x++)
        {
            for (var y = -halfSize; y < halfSize - 1; y++)
            {
                var pos = new Vector3Int(x, y, 0);
                TrySetObject(pos);
                tilemap.SetTile(pos, tile);
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
                TrySetObject(pos);
                tilemap.SetTile(pos, tiles[rand]);
            }
        }
        tilemap.RefreshAllTiles();
    }
    
    private void TrySetObject(Vector3Int cellPos)
    {
        if (Random.Range(0, 100) >= _config.ChanceToSpawnObject) return;

        var pos = _floorMap.GetCellCenterWorld(cellPos);

        var box = Instantiate(_boxPrefab, pos, Quaternion.identity, transform).GetComponent<BoxItem>();
        box.Construct(_boxItem);
    }
}
}