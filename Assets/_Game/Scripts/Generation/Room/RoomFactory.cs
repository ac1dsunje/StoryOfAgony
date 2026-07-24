using _Game.Scripts.Items.Box;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Game.Scripts.Generation.Room
{
public class RoomFactory: MonoBehaviour
{
    [SerializeField] private RoomConfig _roomConfig;
    [SerializeField] private Tilemap _floorMap;
    [SerializeField] private Tilemap _wallMap;
    [SerializeField] private GameObject _boxPrefab;

    [SerializeField] private Transform[] _objectPoints;

    public void SetObjects(BoxItemConfig boxItem) {
        foreach (var points in _objectPoints)
        {
            var box = Instantiate(_boxPrefab, points.position, Quaternion.identity, transform).GetComponent<BoxItem>();
            box.Construct(boxItem);
        }
    }
}
}