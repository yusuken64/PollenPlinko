using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
	public Game Game;
	public ShopItemDefinition ShopItemDefinition;

	public float Progress;

	public int Count;

	public float AutoPower = 1f;

	public bool AutoProgress;
	public bool AutoRestart;

	public Image FillImage;

	public void Sell_Click()
	{
		StartIfAble();
	}

	private void StartIfAble()
	{
		if (AutoProgress) { return; }
		if (Game.Honey.Value >= 1)
		{
			Game.Honey.Add(-1);
			AutoProgress = true;
		}
	}

	public void Buy_Click()
	{
		int cost = Mathf.RoundToInt(
			ShopItemDefinition.BaseCost *
			Mathf.Pow(ShopItemDefinition.CostMultiplier, Count));

		if (Game.Honey.Value >= cost)
		{
			Count++;
			Game.Honey.Add(-cost);
		}
	} 

	private void Update()
	{
		UpdateUI();
		if (!AutoProgress) { return; }

		Progress += AutoPower * Time.deltaTime;
		CheckFinished();
	}

	private void UpdateUI()
	{
		FillImage.fillAmount = Progress / ShopItemDefinition.ProductionTime;
	}

	private void CheckFinished()
	{
		if (Progress < ShopItemDefinition.ProductionTime)
			return;

		// Carry over extra progress
		Progress -= ShopItemDefinition.ProductionTime;

		int payout = ShopItemDefinition.BasePayout * Count;
		Game.Wax.Add(payout);

		AutoProgress = false;
		if (AutoRestart)
		{
			StartIfAble();
		}
	}
}
