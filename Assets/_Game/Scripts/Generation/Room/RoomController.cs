using _Game.Scripts.Items.Box;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Game.Scripts.Generation.Room
{
public class RoomController: MonoBehaviour
{
    [SerializeField] private RoomConfig _config;
    [SerializeField] private Tilemap _floorMap;
    [SerializeField] private Tilemap _wallMap;
    [SerializeField] private GameObject _boxPrefab;

    [SerializeField] private Transform[] _objectPoints;
    
    public void Build(BoxItemConfig boxItem) 
    {
        SetFloor();
        SetObjects(boxItem);
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
    
    private void SetSimpleRange(Tilemap tilemap, TileBase tile)
    {
        var halfSize = _config.Size / 2;
        for (var x = -halfSize; x < halfSize; x++)
        {
            for (var y = -halfSize; y < halfSize; y++)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
        tilemap.RefreshAllTiles();
    }

    private void SetRandomRange(Tilemap tilemap, TileBase[] tiles)
    {
        var halfSize = _config.Size / 2;
        for (var x = -halfSize; x < halfSize; x++)
        {
            for (var y = -halfSize; y < halfSize; y++)
            {
                var rand = Random.Range(0, tiles.Length);
                tilemap.SetTile(new Vector3Int(x, y, 0), tiles[rand]);
            }
        }
        tilemap.RefreshAllTiles();
    }
    
    private void SetObjects(BoxItemConfig boxItem)
    {
        foreach (var point in _objectPoints)
        {
            var box = Instantiate(_boxPrefab, point.position, Quaternion.identity, transform).GetComponent<BoxItem>();
            box.Construct(boxItem);
        }
    }
}
}