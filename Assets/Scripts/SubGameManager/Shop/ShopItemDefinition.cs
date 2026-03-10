using UnityEngine;

[CreateAssetMenu(menuName = "Game/Idle/ShopItem")]
public class ShopItemDefinition : ScriptableObject
{
	public string ItemName;

	[Header("Economy")]
	public int BaseCost;
	public int BasePayout;

	[Header("Production")]
	public float ProductionTime;

	[Header("Scaling")]
	public float CostMultiplier = 1.07f;
	public BusinessMilestone[] Milestones;

	[Header("Visuals")]
	public Sprite Icon;
}

[System.Serializable]
public class BusinessMilestone
{
	public int RequiredCount;
	public float Multiplier;
}