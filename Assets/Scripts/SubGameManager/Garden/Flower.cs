using System;
using UnityEngine;

public class Flower : MonoBehaviour
{
	public int hp = 3;
	private Pin parentPin;
	private Game _game;

	public ResourceType ResourceType;
	private Action _release;

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
		hp -= v;
		if (hp <= 0)
		{
			parentPin.OccupiedFlower = null;
			//Destroy(gameObject);
			_release.Invoke();
			_game.FlowerSpawnZone.FlowerDestroyed(this);
		}
	}

	internal void Setup(Game game, Pin pin)
	{
        _game = game;
		hp = game.PollenPerFlower;
		parentPin = pin;
	}

	internal void SetRelease(Action release)
	{
		_release = release;
	}
}
