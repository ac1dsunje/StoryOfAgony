using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Game.Scripts.Room
{
public class RoomGenerator: MonoBehaviour
{
    [SerializeField] private RoomConfig _roomConfig;
    [SerializeField] private Tilemap _floorMap;
    [SerializeField] private Tilemap _wallMap;

    private void Awake()
    {
    }
}
}