using _Game.Scripts.Generation;
using _Game.Scripts.UI;
using UnityEngine;

namespace _Game.Scripts
{
public class EntryPoint: MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private BuildingManager _buildingManager;
    [SerializeField] private Overlay _overlay;
    [SerializeField] private WinScreen _winScreen;

    private void Awake()
    {
        _overlay.Construct(_player, _buildingManager);
        _winScreen.Construct(_buildingManager);
        _buildingManager.CreateRoom();
    }
}
}