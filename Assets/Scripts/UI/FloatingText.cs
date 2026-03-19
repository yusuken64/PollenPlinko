using TMPro;
using UnityEngine;
using DG.Tweening;
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
	private Action _release;

	public void Setup(Vector3 position, string message)
    {
        this.gameObject.SetActive(true);
        transform.position = position;
        Text.text = message;

        transform.DOKill();
        Text.DOKill();

        float randomX = UnityEngine.Random.Range(-HorizontalJitter, HorizontalJitter);
        Vector3 targetPos = position + new Vector3(randomX, FloatHeight, 0f);

        transform.DOMove(targetPos, Duration)
            .SetEase(Ease.OutCubic);

        Text.alpha = 1f;
        Text.DOFade(0f, Duration)
            .SetEase(Ease.InQuad);

        transform.localScale = Vector3.one;
        transform.DOPunchScale(Vector3.one * ScalePunch, 0.3f, 8, 0.8f);

        DOVirtual.DelayedCall(Duration, () =>
        {
            _release?.Invoke();
            this.gameObject.SetActive(false);
        });
    }

    public void SetRelease(Action release)
    {
        _release = release;
    }
}