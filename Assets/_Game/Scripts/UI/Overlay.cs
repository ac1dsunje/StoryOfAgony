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
    }
}
}