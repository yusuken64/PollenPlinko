using System;
using UnityEngine;
using UnityEngine.UI;

public class Hex : MonoBehaviour
{
    public Image FillImage;
    public float fillrate;
	private Game _game;

    public GameObject OccupiedObject;

    public Vector2Int Coord;

    internal void Setup(Game game)
	{
        _game = game;
	}

    public void SetEgg(HiveItem egg, Hive hive)
    {
        if (OccupiedObject != null)
        {
            HiveItem existingItem = OccupiedObject.GetComponent<HiveItem>();
            if (existingItem != null)
            {
                // Save existing item's current hex
                Hex existingItemHex = existingItem.CurrentHex;

                // Place existing item back to its hex
                if (existingItemHex != null)
                {
                    existingItem.Setup(egg.CurrentHex, hive.Game);
                }
            }
        }

        // Place the new egg on this hex
        egg.Setup(this, hive.Game);
    }
}
