using System;
using TMPro;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
	public UpgradeDefinition UpgradeDefinition;

	public TextMeshProUGUI NameText;
	public TextMeshProUGUI LevelText;
	public TextMeshProUGUI CostsText;

	private Func<int> getLevel;
	private Func<int, bool> canPurchase;
	private Action<int> purchaseAction;

	private void Start()
	{
		NameText.text = UpgradeDefinition.Name;
	}


	public void SetupUpgrade(
		Game game,
		Func<int> getLevel,
		Action onPurchase,
		Func<int, bool> extraCanPurchase = null)
	{
		this.getLevel = getLevel;

		this.canPurchase = (level) =>
		{
			if (level >= UpgradeDefinition.Max)
				return false;

			if (extraCanPurchase != null && !extraCanPurchase(level))
				return false;

			int cost = UpgradeDefinition.Costs[level];

			return game.CanAfford(
				UpgradeDefinition.ResourceType,
				cost);
		};

		this.purchaseAction = (level) =>
		{
			int cost = UpgradeDefinition.Costs[level];

			game.Spend(
				UpgradeDefinition.ResourceType,
				cost);

			onPurchase();
		};

		UpdateUI();
	}

	public void UpdateUI()
	{
		NameText.text = UpgradeDefinition.Name;
		int currentLevel = getLevel();
		LevelText.text = currentLevel.ToString();

		if (currentLevel >= UpgradeDefinition.Max)
		{
			CostsText.text = "MAX";
			//Button.interactable = false;
			return;
		}

		CostsText.text = UpgradeDefinition.Costs[currentLevel].ToString();
	}

	public void Click()
	{
		if (IsMaxed())
		{
			return;
		}

		var level = getLevel();

		if (canPurchase(level))
		{
			purchaseAction?.Invoke(level);
			UpdateUI();
		}
	}

	public bool IsMaxed()
	{
		return getLevel() >= UpgradeDefinition.Max;
	}
}
