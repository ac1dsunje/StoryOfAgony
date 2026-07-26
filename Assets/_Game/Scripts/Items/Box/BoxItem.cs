using System;
using System.Collections.Generic;
using _Game.Scripts.Fire;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Items.Box
{
public class BoxItem : MonoBehaviour, IDamageAble
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _lighter;
    private BoxItemConfig _config;

    public event Action<int> OnBoxOpened;
    public event Action<BoxItem> OnBoxTookHit;

    public bool IsEmpty { get; private set; } = false;
    
    private readonly List<QuestItem> _items = new();

    public void Construct (BoxItemConfig config)
    {
        _config = config;
        _spriteRenderer.sprite = _config.Sprite;
        FillItems();
    }

    private void FillItems()
    {
        var count = Random.Range(1, _config.Amount+1);
        for (var i = 0; i < count; i++)
        {
            _items.Add(_config.QuestItem);
        }
    }

    public void SetLighter(bool state)
    {
        _lighter.SetActive(state);
    }

    public Sprite GetSprite() => _config.QuestItem.Sprite;

    public List<QuestItem> GetItems()
    {
        IsEmpty = true;
        OnBoxOpened?.Invoke(_items.Count);
        return _items;
    }

    public void TakeHit()
    {
        OnBoxTookHit?.Invoke(this);
    }
}
}