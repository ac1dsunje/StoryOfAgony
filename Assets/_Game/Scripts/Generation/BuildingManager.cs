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

    private void Awake()
    {
        CreateRoom();
    }

    private void CreateRoom()
    {
        if (_boxConfigs.Count == 0) return;
        
        var room = Instantiate(_roomPrefab, transform.position, Quaternion.identity, transform).GetComponent<RoomController>();
        room.Build(_boxConfigs);
    }
}
}