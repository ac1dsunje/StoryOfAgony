using System.Collections.Generic;
using _Game.Scripts.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.UI
{
public class Overlay: ScreenManager
{
    [SerializeField] private GameObject _holder;
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private Image _image;
    
    private PlayerController _player;

    public void Construct(PlayerController player)
    {
        _player = player;
        _player.OnItemsChanged += OnItemsChanged;
        _holder.SetActive(false);
    }

    private void OnItemsChanged(List<QuestItem> items)
    {
        if (items.Count > 0)
        {
            _holder.SetActive(true);
            _countText.text = $"{items.Count.ToString()}";
            _image.sprite = items[0].Sprite;
        }
        else
        {
            _holder.SetActive(false);
            _image.sprite = null;
        }
    }

    private void OnDestroy()
    {
        _player.OnItemsChanged -= OnItemsChanged;
    }
}
}