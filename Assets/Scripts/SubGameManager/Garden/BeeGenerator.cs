using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BeeGenerator : MonoBehaviour
{
    [Header("Timing")]
    public float spawnInterval = 1f;

    [Header("Limit")]
    public int maxFlowers = 4;

    public Game Game;

    private RectTransform _rectTransform;
    private float _timer;

    private readonly List<GameObject> _activeFlowers = new();

    public TextMeshProUGUI FlowerText;
    public Image FlowerFill;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        FlowerText.text = $"Balls {Game.Bees.Value}";
    }

    public void Load_Clicked()
	{
        var amount = Mathf.Min(Game.Larvae.Value, 10);
        Game.Larvae.Add(-amount);
        Game.Bees.Add(amount);
	}

    public void Load_Clicket(int amount)
    {
        var exchangeAmount = Mathf.Min(Game.Larvae.Value, amount);
        Game.Larvae.Add(-exchangeAmount);
        Game.Bees.Add(exchangeAmount);
    }

    private void SetLayerRecursively(GameObject obj, int layer)
	{
		obj.layer = layer;

		foreach (Transform child in obj.transform)
		{
			SetLayerRecursively(child.gameObject, layer);
		}
	}
}