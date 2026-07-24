using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Items.Box
{
public class BoxItem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private BoxItemConfig _config;

    public bool IsEmpty { get; private set; } = false;

    public void Construct (BoxItemConfig config)
    {
        _config = config;
        _spriteRenderer.sprite = _config.Sprite;
    }

    public List<QuestItem> GetItems()
    {
        IsEmpty = true;
        var objects = new List<QuestItem>();
        var count = Random.Range(1, _config.Amount+1);

        for (var i = 0; i < count; i++)
        {
            objects.Add(_config.QuestItem);
        }
        
        gameObject.SetActive(false);
        return objects;
    }
}
}