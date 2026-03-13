using System;
using UnityEngine;
using UnityEngine.UI;

public class Hex : MonoBehaviour
{
    public GameObject Highlight;
    private Game _game;

    public HiveItem OccupiedObject;

    public Vector2Int Coord;

    internal void Setup(Game game)
    {
        _game = game;
    }

    public void SetItem(HiveItem item, Hive hive)
    {
        if (OccupiedObject != null)
        {
            HiveItem existingItem = OccupiedObject;
            if (existingItem != null)
            {
                Hex existingItemHex = existingItem.CurrentHex;

                if (existingItemHex != null)
                {
                    existingItem.Setup(item.CurrentHex, hive.Game, item.Level);
                }
            }
        }

        // Place the new egg on this hex
        item.Setup(this, hive.Game, item.Level);
    }
}
