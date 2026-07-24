using UnityEngine;

namespace _Game.Scripts.Items.Box
{
[CreateAssetMenu(fileName = "BoxItemConfig", menuName = "Configs/Game/Objects/Box")]
public class BoxItemConfig: ScriptableObject
{
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public QuestItem QuestItem { get; private set; }
    [field: SerializeField] public int Amount { get; private set; }
}
}