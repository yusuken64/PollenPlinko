using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    public TextMeshProUGUI textLabel;
    public CanvasGroup canvasGroup;

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
        Hide();
    }

    private void Update()
    {
        UpdateContent();
    }

    public EventSystem eventSystem;

    private PointerEventData _pointerData = null;
    private List<RaycastResult> _results = new List<RaycastResult>();

    private void UpdateContent()
    {
        // --- UI RAYCAST ---
        _pointerData ??= new PointerEventData(eventSystem);
        Vector2 mousePos = Mouse.current.position.ReadValue();
        _pointerData.position = mousePos;

        _results.Clear();
        EventSystem.current.RaycastAll(_pointerData, _results);

        foreach (var result in _results)
        {
            var info = result.gameObject.GetComponentInParent<ToolTipInfo>();
            if (info != null)
            {
                Show(info);
                return;
            }
        }

        // --- WORLD RAYCAST ---
        Ray ray = _camera.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var info = hit.collider.GetComponent<ToolTipInfo>();
            if (info != null)
            {
                Show(info);
                return;
            }
        }

        Hide();
    }

	private void Show(ToolTipInfo info)
	{
        textLabel.text = info.ToolTipString;
        canvasGroup.alpha = 1;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        transform.position = mousePos + info.Offset;
    }

    private void Hide()
    {
        canvasGroup.alpha = 0;
    }
}
