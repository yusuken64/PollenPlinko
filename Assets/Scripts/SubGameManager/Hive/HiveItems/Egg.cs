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
        Game.Larvae.Add((int)Mathf.Pow(3, Level));
        Destroy(gameObject);
    }
}
