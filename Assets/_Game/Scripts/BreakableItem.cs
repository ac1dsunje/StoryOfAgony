using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Scripts
{
public class BreakableItem : MonoBehaviour
{
    [SerializeField] private Sprite _sprite;
    
    [SerializeField] private QuestItem _object;
    [SerializeField] private int _amount;
    
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer.sprite = _sprite;
    }

    public List<QuestItem> GetItems()
    {
        var objects = new List<QuestItem>();
        var count = Random.Range(1, _amount+1);

        for (var i = 0; i < count; i++)
        {
            objects.Add(_object);
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