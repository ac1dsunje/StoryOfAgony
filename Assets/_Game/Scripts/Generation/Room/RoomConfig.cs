using UnityEngine;

namespace _Game.Scripts.Generation.Room
{
[CreateAssetMenu(fileName = "RoomConfig", menuName = "Configs/Game/Room")]
public class RoomConfig: ScriptableObject
{
    [field: SerializeField] public Sprite[] FloorSprites { get; private set; }
    [field: SerializeField] public Sprite[] WallSprites { get; private set; }
}
}