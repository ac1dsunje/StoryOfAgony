using _Game.Scripts.Generation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Game.Scripts.UI
{
public class WinScreen: ScreenManager
{
    [SerializeField] private Button _restartButton;

    private BuildingManager _buildingManager;
    
    public void Construct(BuildingManager buildingManager)
    {
        _buildingManager = buildingManager;
        _buildingManager.OnLevelsEnded += Show;
    }
    
    private void Start()
    {
        Hide();
        _restartButton.onClick.AddListener(RestartGame);
    }

    protected override void Show()
    {
        base.Show();
        Time.timeScale = 0;
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnDestroy()
    {
        _restartButton.onClick.RemoveListener(RestartGame);
        _buildingManager.OnLevelsEnded -= Show;
    }
}
}