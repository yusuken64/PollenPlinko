using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Game/Upgrades/UpgradeDefinition", order = 1)]
public class UpgradeDefinition : ScriptableObject
{
	public string ID;
	public string Name;

	public int Max;
	public ResourceType ResourceType;

	public CostScalingMode ScalingMode;

	public int BaseCost = 10;
	public float LinearIncrease = 5f;
	public float ExponentialMultiplier = 1.15f;
	public List<int> Costs;

	[ContextMenu("Set Costs")]
	public void SetCosts()
	{
		for (int i = 0; i < Max; i++)
		{
			int cost = 0;

			switch (ScalingMode)
			{
				case CostScalingMode.Manual:
					return; // do nothing
				case CostScalingMode.Linear:
					cost = Mathf.RoundToInt(BaseCost + i * LinearIncrease);
					break;
				case CostScalingMode.Exponential:
					cost = Mathf.RoundToInt(BaseCost * Mathf.Pow(ExponentialMultiplier, i));
					break;
			}

			Costs.Add(cost);
		}
	}
}

public enum CostScalingMode
{
	Manual,
	Linear,
	Exponential
}

public enum ResourceType
{
	Larvae,
	Bees,
	Pollen,
	Nectar,
	Honey,
	RoyalJelly,
	Gold,
}
