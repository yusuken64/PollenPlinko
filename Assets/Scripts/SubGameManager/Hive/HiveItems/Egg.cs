using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Egg : HiveItem
{
	public override void HandleClick()
	{
        Hatch();
	}

	public void Hatch()
    {
        CurrentHex.OccupiedObject = null;
        Game.Larvae.Add(1);
        Destroy(gameObject);
    }
}
