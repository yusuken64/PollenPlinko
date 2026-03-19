using UnityEngine;

public class Egg : HiveItem
{
	public override void HandleClick()
	{
		Hatch();
	}

	public void Hatch()
	{
		CurrentHex.OccupiedObject = null;
		int amount = (int)Mathf.Pow(3, Level);
		Game.Larvae.Add(amount);
		Destroy(gameObject);

		FloatingTextManager.Instance.ShowText(this.transform.position, $"+{amount}");
	}
}
