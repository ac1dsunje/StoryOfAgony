using System;
using _Game.Scripts.Player;
using UnityEngine.SceneManagement;

namespace _Game.Scripts
{
public class SceneLoader: IDisposable
{
    private PlayerController _player;

    public SceneLoader(PlayerController player)
    {
        _player = player;
        _player.OnDeath += ReloadScene;
    }
    
    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Dispose()
    {
        _player.OnDeath -= ReloadScene;
    }
}
}