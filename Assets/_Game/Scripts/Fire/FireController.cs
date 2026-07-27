using System.Collections;
using _Game.Scripts.Generation;
using _Game.Scripts.Generation.Room;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Game.Scripts.Fire
{
public class FireController: MonoBehaviour
{
    private Tilemap _tilemap;
    private TileController _tileController;

    private void Awake()
    {
        _tilemap = GetComponent<Tilemap>();
    }

    public void Construct(TileController tileController)
    {
        _tileController = tileController;
    }

    public void SpreadFire(RoomConfig config)
    {
        Refresh();
        StartCoroutine(FillFloor(config));
    }

    private void Refresh()
    {
        StopAllCoroutines();
        _tilemap.ClearAllTiles();
        _tilemap.RefreshAllTiles();
    }

    private IEnumerator FillFloor(RoomConfig config)
    {
        var layer = 0;

        while (layer < config.Size / 2)
        {
            yield return new WaitForSeconds(5f);

            var min = - config.Size / 2 + layer;
            var max = config.Size / 2 - 1 - layer;

            for (var x = min; x <= max; x++)
            {
                _tileController.AddTile(_tilemap, new Vector3Int(x, max, 0), config.FireTile);
                _tileController.AddTile(_tilemap, new Vector3Int(x, min, 0), config.FireTile);
            }

            for (var y = min + 1; y < max; y++)
            {
                _tileController.AddTile(_tilemap, new Vector3Int(min, y, 0), config.FireTile);
                _tileController.AddTile(_tilemap, new Vector3Int(max, y, 0), config.FireTile);
            }
            layer++;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out IDamageAble target)) return;
        
        target.TakeHit();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
}