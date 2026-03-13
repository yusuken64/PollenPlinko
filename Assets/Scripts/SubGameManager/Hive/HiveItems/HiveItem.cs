using System;
using TMPro;
using UnityEngine;

public abstract class HiveItem : MonoBehaviour
{
	public Hex CurrentHex;
	public Game Game;

	public TextMeshPro LevelText;
	public int Level;
	public string ItemType; //all levels of the same item should share ItemType

	public void Setup(Hex hex, Game game, int level)
	{
		if (CurrentHex != null &&
			CurrentHex.OccupiedObject == this)
		{
			CurrentHex.OccupiedObject = null;
		}

		CurrentHex = hex;
		Game = game;
		Level = level;

		if (LevelText != null)
		{
			LevelText.text = level.ToString();
		}

		CurrentHex.OccupiedObject = this;
		transform.parent = CurrentHex.transform;
		transform.localPosition = Vector3.zero;
	}

	public virtual void HandleClick() { }

	internal bool CanMerge(HiveItem originalItem)
	{
		return this.GetType() == originalItem.GetType() &&
			Level == originalItem.Level;
	}
}
