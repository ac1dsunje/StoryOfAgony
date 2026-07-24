using UnityEngine;

namespace _Game.Scripts.Items
{
[CreateAssetMenu(fileName = "QuestItem", menuName = "Configs/Game/Objects/QuestItem")]
public class QuestItem: ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
}
}