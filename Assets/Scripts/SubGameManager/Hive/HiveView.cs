using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class HiveView : MonoBehaviour
{
    [SerializeField] private RectTransform _rt;
    [SerializeField] private RectTransform _collapsedTarget;
    [SerializeField] private RectTransform _expandedTarget;

    [SerializeField] private Vector2 _minimapAnchorMin = new Vector2(0, 0);
    [SerializeField] private Vector2 _minimapAnchorMax = new Vector2(1, 0);
    [SerializeField] private Vector2 _minimapPivot = new Vector2(1, 0);
    [SerializeField] private Vector2 _minimapSize = new Vector2(300, 200);

    [SerializeField] private Vector2 _fullscreenAnchorMin = Vector2.zero;
    [SerializeField] private Vector2 _fullscreenAnchorMax = Vector2.one;
    [SerializeField] private Vector2 _fullscreenPivot = new Vector2(0.5f, 0.5f);
    [SerializeField] private Vector2 _fullscreenSize = Vector2.zero;

    [SerializeField] private float _collapseDuration = 0.3f;

    public Camera HiveCamera;
    private float _baseCameraSize;
    [SerializeField] private float _minimapScaleFactor = 3f;

    public GameObject HiveUI;
    public bool IsCollapsed;

    private void Start()
    {
        _baseCameraSize = HiveCamera.orthographicSize;
        Expand();
    }

    public void Outside_Clicked()
    {
        if (IsCollapsed)
        {
            Expand();
        }
        else
        {
            Collapse();
        }
    }

    public void Collapse()
    {
        HiveUI.gameObject.SetActive(false);
        IsCollapsed = true;
        _rt.DOKill();
        HiveCamera.DOKill();

        _rt.DOAnchorMin(_minimapAnchorMin, _collapseDuration);
        _rt.DOAnchorMax(_minimapAnchorMax, _collapseDuration);
        _rt.DOPivot(_minimapPivot, _collapseDuration);
        _rt.DOAnchorPos(Vector2.zero, _collapseDuration);
        _rt.DOSizeDelta(_minimapSize, _collapseDuration);

        float targetSize = _baseCameraSize * _minimapScaleFactor;
        HiveCamera.DOOrthoSize(targetSize, _collapseDuration)
            .SetEase(Ease.InOutSine);
    }

    public void Expand()
    {
        HiveUI.gameObject.SetActive(true);
        IsCollapsed = false;
        _rt.DOKill();
        HiveCamera.DOKill();

        _rt.DOAnchorMin(_fullscreenAnchorMin, _collapseDuration);
        _rt.DOAnchorMax(_fullscreenAnchorMax, _collapseDuration);
        _rt.DOPivot(_fullscreenPivot, _collapseDuration);
        _rt.DOAnchorPos(Vector2.zero, _collapseDuration);
        _rt.DOSizeDelta(_fullscreenSize, _collapseDuration);

        HiveCamera.DOOrthoSize(_baseCameraSize, _collapseDuration)
            .SetEase(Ease.InOutSine);
    }
}
