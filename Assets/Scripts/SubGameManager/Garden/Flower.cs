using System;
using UnityEngine;

public class Flower : MonoBehaviour
{
	public int hp = 3;
	private Pin parentPin;
	private Game _game;

	public ResourceType ResourceType;
	private Action _release;
	private bool _isDead;

	private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Bee>(out var bee))
        {
			TakeDamage(1);

			bee.TakeDamage(1);
			int amount = bee.Mult;
			_game.Gain(ResourceType, amount);
			FloatingTextManager.Instance.ShowText(this.transform.position, $"+{amount}");

			AudioManager.Instance.PlaySFX(_game.Garden.FlowerHit, AudioManager.AUDIOPOOLID_FLOWER);
		}
	}
	internal void TakeDamage(int v)
	{
		if (_isDead) return;

		hp -= v;
		if (hp <= 0)
		{
			_isDead = true;
			parentPin.OccupiedFlower = null;
			parentPin = null;
			//Destroy(gameObject);
			_game.FlowerSpawnZone.FlowerDestroyed(this);
			_release.Invoke();
			_release = null;
		}
	}

	internal void Setup(Game game, Pin pin)
	{
        _game = game;
		hp = game.PollenPerFlower;
		parentPin = pin;
		_isDead = false;
	}

	internal void SetRelease(Action release)
	{
		_release = release;
	}
}
