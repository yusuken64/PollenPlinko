using DG.Tweening;
using System;
using UnityEngine;

public class SubGameView : MonoBehaviour
{
    [SerializeField] public RectTransform _rt;
    [SerializeField] private RectTransform _collapsedTarget;
    [SerializeField] private RectTransform _expandedTarget;
    [SerializeField] private float _collapseDuration = 0.3f;

    internal void Initialize(SubGameManager subGameManager)
    {
        _subGameManager = subGameManager;
        SubGame = subGameBehaviour as ISubGame;
        _subGameCamera = SubGame?.GameCamera;
        _rt.gameObject.SetActive(true);
    }

    public Camera _subGameCamera;

    public bool IsCollapsed;
    private SubGameManager _subGameManager;

    [SerializeField] private MonoBehaviour subGameBehaviour;
    public ISubGame SubGame;

    public GameObject UIObject;

    private void TweenTo(RectTransform target)
    {
        _rt.DOKill();
        _subGameCamera?.DOKill();

        RectTransform parent = _rt.parent as RectTransform;

        // Get world corners of target
        Vector3[] worldCorners = new Vector3[4];
        target.GetWorldCorners(worldCorners);

        // Convert world corners into local space of our parent
        for (int i = 0; i < 4; i++)
        {
            worldCorners[i] = parent.InverseTransformPoint(worldCorners[i]);
        }

        // Bottom-left and top-right in parent's local space
        Vector2 bottomLeft = worldCorners[0];
        Vector2 topRight = worldCorners[2];

        Vector2 size = topRight - bottomLeft;
        Vector2 center = bottomLeft + size * 0.5f;

        // Force non-stretch anchors so it behaves predictably
        Vector2 anchor = new Vector2(0.5f, 0.5f);

        _rt.DOAnchorMin(anchor, _collapseDuration);
        _rt.DOAnchorMax(anchor, _collapseDuration);
        _rt.DOPivot(new Vector2(0.5f, 0.5f), _collapseDuration);
        _rt.DOSizeDelta(size, _collapseDuration);
        _rt.DOAnchorPos(center, _collapseDuration);

		if (SubGame?.GameCamera == null) { return; }
		float cameraSize = GetCameraSizeFor(target, _rt);
		_subGameCamera?.DOOrthoSize(cameraSize, _collapseDuration)
			.SetEase(Ease.InOutSine);
	}

    public void Collapse()
    {
        if (IsCollapsed) return;

        IsCollapsed = true;

        TweenTo(_collapsedTarget);
        if (UIObject != null)
        {
            UIObject?.gameObject.SetActive(false);
        }
    }

    public void Expand()
    {
        if (!IsCollapsed) return;

        IsCollapsed = false;

        TweenTo(_expandedTarget);
        if (UIObject != null)
        {
            UIObject?.gameObject.SetActive(true);
        }
    }

    private float GetCameraSizeFor(RectTransform target, RectTransform _rt)
    {
        var camera = SubGame.GameCamera;

        float expandedHeight = GetHeightInParentSpace(_rt);
        float targetHeight = GetHeightInParentSpace(target);

        float ratio = expandedHeight / targetHeight;

        return camera.orthographicSize * ratio;
    }

    private float GetHeightInParentSpace(RectTransform target)
    {
        RectTransform parent = _rt.parent as RectTransform;

        Vector3[] corners = new Vector3[4];
        target.GetWorldCorners(corners);

        for (int i = 0; i < 4; i++)
            corners[i] = parent.InverseTransformPoint(corners[i]);

        float height = corners[2].y - corners[0].y;

        return Mathf.Abs(height);
    }

    public void Click()
    {
        _subGameManager.SubGameClicked(this);
    }
}
