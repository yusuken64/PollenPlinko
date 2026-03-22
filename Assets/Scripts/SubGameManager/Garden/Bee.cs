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
	public float YThreshold;

	public AudioClip AudioClip;

	internal void Setup(int beeHitHP, int mult)
	{
		this.hp = beeHitHP;
		Mult = mult;
		Text.text = mult.ToString();
		this.gameObject.SetActive(true);
		Spawn();
	}

	private SimplePool<Bee> _pool;

	public void SetPool(SimplePool<Bee> pool)
	{
		_pool = pool;
	}

	public void ReleaseToPool()
	{
		gameObject.SetActive(false);
		_pool.Release(this);
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

	private void Update()
	{
		if (transform.localPosition.y < YThreshold)
		{
			Die();
		}
	}

	public void Spawn()
	{
		isDead = false;
		BeeShadow.gameObject.SetActive(false);

		gameObject.layer = _aliveLayer;

		//SetLayerRecursively(gameObject, _aliveLayer);

	}

	public void Die()
	{
		isDead = true;
		BeeShadow.gameObject.SetActive(true);

		// Change collision layer
		gameObject.layer = _deadLayer;

		//// Optional: also change children layers if needed
		//SetLayerRecursively(gameObject, _deadLayer);

		//ReleaseToPool();
	}

	//private void SetLayerRecursively(GameObject obj, int layer)
	//{
	//	obj.layer = layer;

	//	foreach (Transform child in obj.transform)
	//	{
	//		SetLayerRecursively(child.gameObject, layer);
	//	}
	//}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		AudioManager.Instance.PlaySFX(AudioClip, AudioManager.AUDIOPOOLID_PINS);
	}
}