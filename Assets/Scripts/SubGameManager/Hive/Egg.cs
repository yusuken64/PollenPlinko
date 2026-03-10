using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Egg : MonoBehaviour
{
	private Hex _hex;
	private Game _game;

	public void Setup(Hex hex, Game game)
	{
        _hex = hex;
        _game = game;
    }

    public void Hatch()
    {
        _hex.OccupiedObect = null;
        _game.Bees.Add(1);
        Destroy(gameObject);
    }
}
