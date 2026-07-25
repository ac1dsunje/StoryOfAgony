using _Game.Scripts.Items.Box;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Game.Scripts.Generation.Room
{
public class RoomController: MonoBehaviour
{
    [SerializeField] private RoomConfig _config;
    [SerializeField] private Tilemap _floorMap;
    [SerializeField] private Tilemap[] _wallMaps;
    [SerializeField] private GameObject _boxPrefab;

    private BoxItemConfig _boxItem;

    private FloorController _floor;
    private WallController _wall;

    public void Build(BoxItemConfig boxItem) 
    {
        _boxItem = boxItem;
        
        _floor = new FloorController(_config);
        _floor.OnSetObject += SetObject;
        _floor.Set(_floorMap);
        
        _wall = new WallController(_config);
        foreach (var wallMap in _wallMaps)
        {
            _wall.Set(wallMap);
        }
    }
    
    private void SetObject(Vector3Int cellPos)
    {
        var pos = _floorMap.GetCellCenterWorld(cellPos);

        var box = Instantiate(_boxPrefab, pos, Quaternion.identity, transform).GetComponent<BoxItem>();
        box.Construct(_boxItem);
    }

    private void OnDestroy()
    {
        _floor.OnSetObject -= SetObject;
    }
}
}