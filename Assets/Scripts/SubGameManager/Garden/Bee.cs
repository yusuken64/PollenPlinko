using System;
using TMPro;
using UnityEngine;

public class Bee : MonoBehaviour
{
	public int hp = 3;

	public GameObject BeeShadow;
	private bool isDead;
	private const int _aliveLayer = 8;
	private const int _deadLayer = 9;

	public int Mult;
	public TextMeshPro Text;

	public Action Release { get; private set; }

	internal void Setup(Game game, int mult)
	{
		this.hp = game.BeeHitHP;
		Mult = mult;
		Text.text = mult.ToString();
		this.gameObject.SetActive(true);
		Spawn();
	}

	internal void TakeDamage(int v)
	{
		if(isDead) { return; }
		hp -= v;
		if (hp <= 0)
		{
			Die();
		}
	}

	public void Spawn()
	{
		isDead = false;
		BeeShadow.gameObject.SetActive(false);

		gameObject.layer = _aliveLayer;

		SetLayerRecursively(gameObject, _aliveLayer);

	}

	private void Die()
	{
		isDead = true;
		BeeShadow.gameObject.SetActive(true);

		// Change collision layer
		gameObject.layer = _deadLayer;

		// Optional: also change children layers if needed
		SetLayerRecursively(gameObject, _deadLayer);
	}

	private void SetLayerRecursively(GameObject obj, int layer)
	{
		obj.layer = layer;

		foreach (Transform child in obj.transform)
		{
			SetLayerRecursively(child.gameObject, layer);
		}
	}

	internal void SetRelease(Action release)
	{
		Release = release;
	}
}