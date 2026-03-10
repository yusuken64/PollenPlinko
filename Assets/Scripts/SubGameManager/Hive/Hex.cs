using System;
using UnityEngine;
using UnityEngine.UI;

public class Hex : MonoBehaviour
{
    public Image FillImage;
    public float fillrate;
	private Game _game;

    public GameObject OccupiedObect;

    public Vector2Int Coord;

	private void Filled()
    {
        _game.Ammo_Click();
    }

    internal void Setup(Game game)
	{
        _game = game;
	}

    public void Click()
	{
        FillImage.fillAmount += 0.2f;
    }
}
