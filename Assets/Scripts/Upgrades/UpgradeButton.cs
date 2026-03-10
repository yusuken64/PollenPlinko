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

	public void Setup(
		Func<int> getLevel,
		Func<int, bool> canPurchase,
		Action<int> purchase)
	{
		this.getLevel = getLevel;
		this.canPurchase = canPurchase;
		this.purchaseAction = purchase;
		
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
