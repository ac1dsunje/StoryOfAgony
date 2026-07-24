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

    private readonly List<BoxItemConfig> _availableBoxConfigs = new();

    private void Awake()
    {
        foreach (var config in _boxConfigs)
        {
            _availableBoxConfigs.Add(config);
        }

        CreateRoom();
    }

    private void CreateRoom()
    {
        if (_availableBoxConfigs.Count == 0) return;
        
        var room = Instantiate(_roomPrefab, transform.position, Quaternion.identity, transform).GetComponent<RoomController>();
        var config = _availableBoxConfigs[Random.Range(0, _availableBoxConfigs.Count)];
        room.Build(config);
        _availableBoxConfigs.Remove(config);
    }
}
}