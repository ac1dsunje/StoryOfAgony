using UnityEngine;

namespace _Game.Scripts.Fire
{
public class Fire: MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out IDamageAble target)) return;
        
        target.TakeHit();
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.TryGetComponent(out IDamageAble target)) return;
        
        target.TakeHit();
    }
}
}