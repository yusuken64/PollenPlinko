using System;
using UnityEngine;

public class Game : MonoBehaviour
{
	public Hive Hive;
	public SpawnZone SpawnZone;
	public FlowerSpawnZone FlowerSpawnZone;

	[Header("Resources")]
	public Resource Larvae = new();
	public Resource Bees = new();
	public Resource Pollen = new();
	public Resource Nectar = new();
	public Resource Honey = new();
	public Resource Jelly = new();
	public Resource Gold = new();

	[Header("Upgrades Bees")]
	//bee upgrades
	public int PollenPerBeeHit;

	internal bool CanAfford(ResourceType resourceType, int amount)
	{
		switch (resourceType)
		{
			case ResourceType.Larvae:
				return Larvae.Value >= amount;
				break;
			case ResourceType.Bees:
				return Bees.Value >= amount;
				break;
			case ResourceType.Pollen:
				return Pollen.Value >= amount;
				break;
			case ResourceType.Nectar:
				return Nectar.Value >= amount;
				break;
			case ResourceType.Honey:
				return Honey.Value >= amount;
				break;
			case ResourceType.RoyalJelly:
				break;
			case ResourceType.Gold:
				return Gold.Value >= amount;
				break;
		}

		return false;
	}

	internal void Spend(ResourceType resourceType, int amount)
	{
		switch (resourceType)
		{
			case ResourceType.Larvae:
				Larvae.Add(-amount);
				break;
			case ResourceType.Bees:
				Bees.Add(-amount);
				break;
			case ResourceType.Pollen:
				Pollen.Add(-amount);
				break;
			case ResourceType.Nectar:
				Nectar.Add(-amount);
				break;
			case ResourceType.Honey:
				Honey.Add(-amount);
				break;
			case ResourceType.RoyalJelly:
				break;
			case ResourceType.Gold:
				Gold.Add(-amount);
				break;
		}
	}

	internal void Gain(ResourceType resourceType, int amount)
	{
		switch (resourceType)
		{
			case ResourceType.Larvae:
				Larvae.Add(amount);
				break;
			case ResourceType.Bees:
				Bees.Add(amount);
				break;
			case ResourceType.Pollen:
				Pollen.Add(amount);
				break;
			case ResourceType.Nectar:
				Nectar.Add(amount);
				break;
			case ResourceType.Honey:
				Honey.Add(amount);
				break;
			case ResourceType.RoyalJelly:
				break;
			case ResourceType.Gold:
				Gold.Add(amount);
				break;
		}
	}

	public int BeeHitHP;

	[Header("Upgrades Garden")]
	//flower upgrades
	public int FlowerCountMax;
	public int FlowerRate;
	public int PollenPerFlower;
	public int NectarProbability;

	//queen upgrades
	public int SpawnRate;

	//bee upgrades
	public void PollenPerBeeHit_Click()
	{
		PollenPerBeeHit++;
	}

	public void BeeHitHP_Click()
	{
		BeeHitHP++;
	}

	//flower upgrades
	public void PollenPerFlower_Click() 
	{
		PollenPerFlower++;
	}
	public void FlowerCount_Click()
	{
		FlowerCountMax++;
		FlowerSpawnZone.maxFlowers = FlowerCountMax;
	}
	public void FlowerRate_Click()
	{
		FlowerRate++;
		FlowerSpawnZone.spawnInterval = 1 - (FlowerRate * 0.1f);
	}


	//queen upgrades
	public void SpawnRate_Click()
	{
		SpawnRate++;
		SpawnZone.spawnInterval -= 0.1f;
	}

	public void Ammo_Click()
	{
		Larvae.Value += 5;
	}
}

[Serializable]
public class Resource
{
	[SerializeField] private int _value;

	public int Value
	{
		get => _value;
		set
		{
			if (_value == value)
				return;

			_value = value;
			OnChanged?.Invoke(_value);
		}
	}

	public void Add(int amount)
	{
		Value += amount;
	}

	public event Action<int> OnChanged;
}