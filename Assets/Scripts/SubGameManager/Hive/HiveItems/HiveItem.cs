using UnityEngine;

public abstract class HiveItem : MonoBehaviour
{
	public Hex CurrentHex;
	public Game Game;

	public void Setup(Hex hex, Game game)
	{
		if (CurrentHex != null &&
			CurrentHex.OccupiedObject == this.gameObject)
		{
			CurrentHex.OccupiedObject = null;
		}

		CurrentHex = hex;
		Game = game;

		CurrentHex.OccupiedObject = this.gameObject;
		transform.parent = hex.transform;
		transform.localPosition = Vector3.zero;

	}

	public virtual void HandleClick() { }
}
