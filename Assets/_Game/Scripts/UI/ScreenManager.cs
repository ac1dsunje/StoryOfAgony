using UnityEngine;

namespace _Game.Scripts.UI
{
[RequireComponent(typeof(CanvasGroup))]
public abstract class ScreenManager: MonoBehaviour
{
    private CanvasGroup _screen;

    private void Awake()
    {
        _screen = GetComponent<CanvasGroup>();
    }

    protected virtual void Show()
    {
        _screen.alpha = 1;
        _screen.blocksRaycasts = true;
        _screen.interactable = true;
    }

    protected virtual void Hide()
    {
        _screen.alpha = 0;
        _screen.blocksRaycasts = false;
        _screen.interactable = false;
    }
}
}