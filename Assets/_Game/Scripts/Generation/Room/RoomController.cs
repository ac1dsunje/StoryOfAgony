using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Tilemap[] _wallMaps;
    [SerializeField] private GameObject _boxPrefab;
    [SerializeField] private ExitController _exit;

    [SerializeField] private Tilemap _fireTileMap;

    private readonly List<BoxItemConfig> _availableBoxItems = new();
    private readonly List<BoxItem> _boxes = new();
    private BoxItemConfig _currentBoxConfig;

    private FloorController _floor;
    private WallController _wall;

    public event Action<int, Sprite> OnQuotaChanged;

    private float _countdown;

    private int _collectedAmount;
    private int _quota;

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
        StartCoroutine(FillFireFloor());
    }

    private IEnumerator FillFireFloor()
    {
        var layer = 0;

        while (layer < _config.Size / 2)
        {
            yield return new WaitForSeconds(5f);

            var min = - _config.Size / 2 + layer;
            var max = _config.Size / 2 - 1 - layer;

            for (var x = min; x <= max; x++)
            {
                _fireTileMap.SetTile(new Vector3Int(x, min, 0), _config.FireTile);
                _fireTileMap.SetTile(new Vector3Int(x, max, 0), _config.FireTile);
            }

            for (var y = min + 1; y < max; y++)
            {
                _fireTileMap.SetTile(new Vector3Int(min, y, 0), _config.FireTile);
                _fireTileMap.SetTile(new Vector3Int(max, y, 0), _config.FireTile);
            }

            layer++;
            _fireTileMap.RefreshAllTiles();
        }
    }

    private void Generate()
    {
        _collectedAmount = 0;
        _quota = 0;
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
        box.OnBoxOpened += OnBoxOpened;
        box.OnBoxTookHit += OnBoxTookHit;
        _boxes.Add(box);

        _quota += 1;
        
        OnQuotaChanged?.Invoke(_quota, box.GetSprite());
    }

    private void OnBoxOpened(int amount)
    {
        _collectedAmount += amount;
        if (_collectedAmount >= _quota)
        {
            _exit.gameObject.SetActive(true);
        }
    }

    private void OnBoxTookHit(BoxItem box)
    {
        box.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _floor.OnSetObject -= SetObject;
        
        foreach (var box in _boxes)
        {
            box.OnBoxOpened -= OnBoxOpened;
            box.OnBoxTookHit -= OnBoxTookHit;
        }
    }
}
}