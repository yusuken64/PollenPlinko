using TMPro;
using UnityEngine;
using System;

public class FloatingText : MonoBehaviour
{
    public TextMeshPro Text;

    [Header("Animation")]
    public float FloatHeight = 2f;
    public float Duration = 1.5f;

    [Header("Optional")]
    public float HorizontalJitter = 0.5f;
    public float ScalePunch = 0.2f;

    private Action<FloatingText> _release;

    private Vector3 _startPos;
    private Vector3 _targetPos;

    private Color _startColor;
    private Color _endColor;

    private float _time;
    private bool _isActive;

    public void Setup(Vector3 position, string message, Color startColor)
    {
        gameObject.SetActive(true);

        _startPos = position;
        transform.position = position;
        transform.localScale = Vector3.one;

        Text.text = message;

        float randomX = UnityEngine.Random.Range(-HorizontalJitter, HorizontalJitter);
        _targetPos = _startPos + new Vector3(randomX, FloatHeight, 0f);

        _startColor = startColor;
        _endColor = new Color(_startColor.r, _startColor.g, _startColor.b, 0f);

        _time = 0f;
        _isActive = true;
    }

    private void Update()
    {
        if (!_isActive)
            return;

        _time += Time.deltaTime;
        float t = _time / Duration;

        if (t >= 1f)
        {
            Complete();
            return;
        }

        // Ease.OutCubic
        float moveT = 1f - Mathf.Pow(1f - t, 3f);

        // Ease.InQuad
        float fadeT = t * t;

        // Position
        transform.position = Vector3.Lerp(_startPos, _targetPos, moveT);

        // Fade
        Text.color = Color.Lerp(_startColor, _endColor, fadeT);

        // Punch scale (first 0.3s)
        if (_time < 0.3f)
        {
            float punchT = _time / 0.3f;
            float punch = Mathf.Sin(punchT * Mathf.PI * 4f) * (1f - punchT);
            transform.localScale = Vector3.one + Vector3.one * punch * ScalePunch;
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }

    private void Complete()
    {
        _isActive = false;

        transform.position = _targetPos;
        Text.color = _endColor;
        transform.localScale = Vector3.one;

        _release?.Invoke(this);
        _release = null;

        gameObject.SetActive(false);
    }

    public void SetRelease(Action<FloatingText> release)
    {
        _release = release;
    }
}