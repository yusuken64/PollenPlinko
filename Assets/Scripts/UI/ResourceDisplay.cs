using System;
using TMPro;
using UnityEngine;

public class ResourceDisplay : MonoBehaviour
{
    public TextMeshProUGUI Text;

    private Resource _resource;

    internal void Register(Resource resource)
    {
        // Unsubscribe from previous resource if any
        if (_resource != null)
        {
            _resource.OnChanged -= HandleResourceChanged;
        }

        _resource = resource;

        if (_resource == null)
            return;

        // Set initial value
        Text.text = _resource.Value.ToString();

        // Subscribe
        _resource.OnChanged += HandleResourceChanged;
    }

    private void HandleResourceChanged(int amount)
    {
        Text.text = amount.ToString();
    }

    private void OnDestroy()
    {
        if (_resource != null)
        {
            _resource.OnChanged -= HandleResourceChanged;
        }
    }
}
