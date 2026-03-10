using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SubGameManager : MonoBehaviour
{
	public List<SubGameView> SubGameViews;

    private SubGameView _activeView;

    private void Awake()
    {
        foreach (var view in SubGameViews)
        {
            view.Initialize(this);
            view.Collapse();
        }

        SubGameClicked(SubGameViews[0]);
    }

    public void SubGameClicked(SubGameView clickedView)
    {
        // If already active, do nothing
        if (_activeView == clickedView)
        {
            _activeView.Collapse();
            _activeView = null;
            return;
        }

        // Deactivate previous
        if (_activeView != null)
        {
            _activeView.Collapse();
        }

        // Activate new
        _activeView = clickedView;
        _activeView.Expand();
    }

    public Canvas Canvas;

    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minSize = 2f;
    [SerializeField] private float maxSize = 20f;

    public void Update()
	{
        if (_activeView == null) { return; }

        if (!IsPointerInsideMainViewport())
        {
            return;
        }

        float scroll = Mouse.current.scroll.ReadValue().y;
        var subGameCamera = _activeView._subGameCamera;

        if (Mathf.Abs(scroll) > 0.01f)
        {
            // Scroll up usually gives positive value depending on platform
            subGameCamera.orthographicSize -= scroll * zoomSpeed * Time.deltaTime;

            subGameCamera.orthographicSize =
                Mathf.Clamp(subGameCamera.orthographicSize, minSize, maxSize);
        }

        if (!Mouse.current.leftButton.wasPressedThisFrame)
        {
            return;
        }

        Vector2 mousePos = Mouse.current.position.ReadValue();
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _activeView._rt,
            mousePos,
            Canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Canvas.worldCamera,
            out Vector2 localPoint))
            return;

        Rect rect = _activeView._rt.rect;

        float u = (localPoint.x - rect.xMin) / rect.width;
        float v = (localPoint.y - rect.yMin) / rect.height;

        // Optional clamp safety
        if (u < 0 || u > 1 || v < 0 || v > 1)
            return;

        // Feed UV into hive camera raycast
        Vector3 worldPoint = subGameCamera.ViewportToWorldPoint(new Vector3(u, v, 0));

        _activeView.SubGame?.HandleUpdate(worldPoint);
    }

    private bool IsPointerInsideMainViewport()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        return RectTransformUtility.RectangleContainsScreenPoint(
            _activeView._rt,
            mousePos,
            Canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Canvas.worldCamera
        );
    }

}
