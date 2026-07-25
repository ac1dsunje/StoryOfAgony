using System.Collections.Generic;
using _Game.Scripts.Items;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.UI
{
public class Overlay: ScreenManager
{
    [SerializeField] private TextMeshProUGUI _countText;
    
    private PlayerController _player;

    public void Construct(PlayerController player)
    {
        _player = player;
        _player.OnItemAdded += OnItemAdded;
    }

    private void OnItemAdded(List<QuestItem> items)
    {
        _countText.text = $"Items collected: {items.Count.ToString()}";
    }

    private void OnDestroy()
    {
        _player.OnItemAdded -= OnItemAdded;
    }
}
}