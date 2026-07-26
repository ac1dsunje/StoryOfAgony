using System;
using System.Collections.Generic;
using _Game.Scripts.Generation.Room;
using _Game.Scripts.Items.Box;
using UnityEngine;

namespace _Game.Scripts.Generation
{
public class BuildingManager : MonoBehaviour
{
    [SerializeField] private List<BoxItemConfig> _boxConfigs;
    [SerializeField] private GameObject _roomPrefab;

    public event Action<int, Sprite> OnQuotaChanged;
    public event Action OnLevelsEnded;
    
    private RoomController _room;

    public void CreateRoom()
    {

        TileController tileController = new();
        if (_boxConfigs.Count == 0) return;
        
        _room = Instantiate(_roomPrefab, transform.position, Quaternion.identity, transform).GetComponent<RoomController>();
        _room.OnQuotaChanged += OnQuotaChanged;
        _room.OnLevelsEnded += OnLevelsEnded;
        _room.Build(_boxConfigs, tileController);
    }

    private void OnDestroy()
    {
        _room.OnQuotaChanged -= OnQuotaChanged;
        _room.OnLevelsEnded -= OnLevelsEnded;
    }
}
}