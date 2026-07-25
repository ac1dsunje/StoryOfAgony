using System.Collections.Generic;
using _Game.Scripts.Items.Box;
using Unity.VisualScripting;
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
    [SerializeField] private ExitController _exit;

    private readonly List<BoxItemConfig> _availableBoxItems = new();
    private readonly List<BoxItem> _boxes = new();
    private BoxItemConfig _currentBoxConfig;

    private FloorController _floor;
    private WallController _wall;

    private float _countdown;

    private void Awake()
    {
        _exit.OnExit += Generate;
    }

    public void Build(List<BoxItemConfig> boxItems) 
    {

        foreach (var item in boxItems)
        {
            _availableBoxItems.Add(item);
        }
        
        _floor = new FloorController(_config);
        _floor.OnSetObject += SetObject;
        
        _wall = new WallController(_config);
        
        Generate();
    }

    private void Generate()
    {
        if (_availableBoxItems.Count > 0)
        {
            ClearObjects();
            _currentBoxConfig = _availableBoxItems[Random.Range(0, _availableBoxItems.Count)];
            _availableBoxItems.Remove(_currentBoxConfig);
        
            _floor.Set(_floorMap);
            foreach (var wallMap in _wallMaps)
            {
                _wall.Set(wallMap);
            }
        }
        else
        {
            ClearObjects();
            Debug.Log("No more items");
        }
    }

    private void ClearObjects()
    {
        if (_boxes.Count <= 0) return;
        foreach (var box in _boxes)
        {
           box.TakeHit();
        }
        _boxes.Clear();
    }
    
    private void SetObject(Vector3Int cellPos)
    {
        var pos = _floorMap.GetCellCenterWorld(cellPos);

        var box = Instantiate(_boxPrefab, pos, Quaternion.identity, transform).GetComponent<BoxItem>();
        box.Construct(_currentBoxConfig);
        _boxes.Add(box);
    }

    private void OnDestroy()
    {
        _floor.OnSetObject -= SetObject;
    }
}
}