using System.Collections.Generic;
using _Game.Scripts.Generation;
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
    [SerializeField] private TextMeshProUGUI _quotaText;
    [SerializeField] private Image _quotaImage;
    
    private PlayerController _player;
    private BuildingManager _buildingManager;

    public void Construct(PlayerController player, BuildingManager buildingManager)
    {
        _player = player;
        _player.OnItemsChanged += OnItemsChanged;

        _buildingManager = buildingManager;
        _buildingManager.OnQuotaChanged += OnQuotaChanged;
        _holder.SetActive(false);
    }

    private void OnQuotaChanged(int amount, Sprite sprite)
    {
        _quotaText.text = $"Quota: {amount}";
        _quotaImage.sprite = sprite;
    } 

    private void OnItemsChanged(List<QuestItem> items)
    {
        if (items.Count > 0)
        {
            _holder.SetActive(true);
            _countText.text = $"{items.Count}";
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

        _buildingManager.OnQuotaChanged -= OnQuotaChanged;
    }
}
}