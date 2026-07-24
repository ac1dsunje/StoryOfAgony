using UnityEngine;

namespace _Game.Scripts
{
[CreateAssetMenu(menuName = "Configs/Game/Objects/QuestItem")]
public class QuestItem: ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
}
}