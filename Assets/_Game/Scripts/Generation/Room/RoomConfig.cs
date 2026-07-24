using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Game.Scripts.Generation.Room
{
[CreateAssetMenu(fileName = "RoomConfig", menuName = "Configs/Game/Room")]
public class RoomConfig: ScriptableObject
{
    [field: SerializeField] public int Size { get; private set; } = 10;
    [field: SerializeField] public TileBase[] FloorTiles { get; private set; }
    [field: SerializeField] public TileBase[] WallTiles { get; private set; }
    [field: SerializeField] public int ChanceToSpawnObject { get; private set; }
}
}