using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
        canvas = _activeView.BackgroundRT.GetComponentInParent<Canvas>();
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

    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minSize = 2f;
    [SerializeField] private float maxSize = 20f;
	private Canvas canvas;

	public void Update()
    {
        if (_activeView == null) return;

        if (!IsPointerInsideMainViewport())
            return;

        var mouse = Mouse.current;

        float scroll = mouse.scroll.ReadValue().y;
        var subGameCamera = _activeView._subGameCamera;

        if (Mathf.Abs(scroll) > 0.01f)
        {
            subGameCamera.orthographicSize -= scroll * zoomSpeed * Time.deltaTime;
            subGameCamera.orthographicSize =
                Mathf.Clamp(subGameCamera.orthographicSize, minSize, maxSize);
        }
        
        if (IsPointerOverUI())
		{
            return;
		}

        if (!TryGetWorldPoint(out Vector3 worldPoint))
            return;

        if (mouse.leftButton.wasPressedThisFrame)
        {
            _activeView.SubGame?.HandleMouseDown(worldPoint);
        }
        else if (mouse.leftButton.isPressed)
        {
            _activeView.SubGame?.HandleMouseDrag(worldPoint);
        }
        else if (mouse.leftButton.wasReleasedThisFrame)
        {
            _activeView.SubGame?.HandleMouseUp(worldPoint);
        }
    }

    bool IsPointerOverUI()
    {
        // Works for mouse
        if (EventSystem.current.IsPointerOverGameObject())
            return true;

        // Works for touch input (mobile)
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    return true;
            }
        }

        return false;
    }

    bool TryGetWorldPoint(out Vector3 worldPoint)
    {
        worldPoint = default;

        Vector2 mousePos = Mouse.current.position.ReadValue();

        var rt = _activeView.RawImageRT;
        var cam = _activeView._subGameCamera;

        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);

        Vector2 bottomLeft = RectTransformUtility.WorldToScreenPoint(null, corners[0]);
        Vector2 topRight = RectTransformUtility.WorldToScreenPoint(null, corners[2]);

        Rect screenRect = new Rect(
            bottomLeft.x,
            bottomLeft.y,
            topRight.x - bottomLeft.x,
            topRight.y - bottomLeft.y
        );

        float u = (mousePos.x - screenRect.xMin) / screenRect.width;
        float v = (mousePos.y - screenRect.yMin) / screenRect.height;

        if (u < 0 || u > 1 || v < 0 || v > 1)
            return false;

        float depth = -cam.transform.position.z;

        worldPoint = cam.ViewportToWorldPoint(new Vector3(u, v, depth));
        worldPoint.z = 0;

        return true;
    }

    private bool IsPointerInsideMainViewport()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        return RectTransformUtility.RectangleContainsScreenPoint(
            _activeView.BackgroundRT,
            mousePos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera
        );
    }
}
