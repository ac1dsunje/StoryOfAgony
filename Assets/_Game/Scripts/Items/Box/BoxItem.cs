using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Items.Box
{
public class BoxItem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private BoxItemConfig _config;

    public void Construct (BoxItemConfig config)
    {
        _config = config;
        _spriteRenderer.sprite = _config.Sprite;
    }

    public List<QuestItem> GetItems()
    {
        var objects = new List<QuestItem>();
        var count = Random.Range(1, _config.Amount+1);

        for (var i = 0; i < count; i++)
        {
            objects.Add(_config.QuestItem);
        }

        Break();
        
        return objects;
    }

    private void Break()
    {
        Destroy(gameObject);
    }
}
}