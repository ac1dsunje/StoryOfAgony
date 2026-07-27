using System;
using System.Collections.Generic;
using _Game.Scripts.Fire;
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
    
    [SerializeField] private FireController _fire;

    private readonly List<BoxItemConfig> _availableBoxItems = new();
    private readonly List<BoxItem> _boxes = new();
    private BoxItemConfig _currentBoxConfig;

    private FloorController _floor;
    private WallController _wall;

    public event Action<int, Sprite> OnQuotaChanged;
    public event Action OnLevelsEnded;

    private float _countdown;

    private int _collectedAmount;
    [SerializeField] private int _quota;
    
    private TileController _tileController;

    public void Build(List<BoxItemConfig> boxItems, TileController tileController) 
    {
        _tileController  = tileController;
        foreach (var item in boxItems)
        {
            _availableBoxItems.Add(item);
        }
        
        _floor = new FloorController(_config, _tileController);
        _floor.OnSetBox += SetBox;
        
        _wall = new WallController(_config, _tileController);
        _fire.Construct(_tileController);
        
        Generate();
    }

    private void Generate()
    {
        StopAllCoroutines();
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
            _fire.SpreadFire(_config);
        }
        else
        {
            ClearObjects();
            OnLevelsEnded?.Invoke();
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
    
    private void SetBox(Vector3Int cellPos)
    {
        var pos = _floorMap.GetCellCenterWorld(cellPos);

        var box = Instantiate(_boxPrefab, pos, Quaternion.identity, transform).GetComponent<BoxItem>();
        box.Construct(_currentBoxConfig);
        box.OnBoxOpened += OnBoxOpened;
        box.OnBoxTookHit += OnBoxTookHit;
        _boxes.Add(box);

        _quota++;
        
        OnQuotaChanged?.Invoke(_quota, box.GetSprite());
    }

    private void OnBoxOpened(int amount)
    {
        _collectedAmount += amount;
        CheckQuota();
    }

    private void CheckQuota()
    {
        if (_collectedAmount < _quota) return;
        _exit.gameObject.SetActive(true);
        _exit.OnExit += Generate;
    }

    private void OnBoxTookHit(BoxItem box)
    {
        box.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _floor.OnSetBox -= SetBox;
        
        foreach (var box in _boxes)
        {
            box.OnBoxOpened -= OnBoxOpened;
            box.OnBoxTookHit -= OnBoxTookHit;
        }
    }
}
}