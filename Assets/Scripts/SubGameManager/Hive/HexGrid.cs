using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public Game Game;
    public GameObject StartPositionObject;

    public float hexWidth = 1;
    public float hexHeight = 1;

    public Dictionary<Vector2Int, Hex> Hexes = new Dictionary<Vector2Int, Hex>();
    public Hex HexPrefab;

    public void GenerateHexRing(int radius)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        Vector3 origin = StartPositionObject.transform.position;

        for (int q = -radius; q <= radius; q++)
        {
            int r1 = Mathf.Max(-radius, -q - radius);
            int r2 = Mathf.Min(radius, -q + radius);

            for (int r = r1; r <= r2; r++)
            {
                Vector3 worldPos = AxialToWorld(q, r, hexWidth, hexHeight, origin);

                Hex newHex = Instantiate(HexPrefab, worldPos, Quaternion.identity, transform);
                newHex.Setup(Game);

                Vector2Int vector2Int = new Vector2Int(q, r);
                Hexes[vector2Int] = newHex;
                newHex.Coord = vector2Int;
            }
        }
    }

    public void AddHex()
    {
        Vector3 origin = StartPositionObject.transform.position;

        // Determine max existing ring
        int maxRadius = 0;

        foreach (var key in Hexes.Keys)
        {
            int dist = HexDistance(Vector2Int.zero, key);
            if (dist > maxRadius)
                maxRadius = dist;
        }

        // Scan rings from inner to outer
        for (int radius = 0; radius <= maxRadius + 1; radius++)
        {
            int expected = RingSize(radius);

            int actual = Hexes.Keys.Count(k => HexDistance(Vector2Int.zero, k) == radius);

            if (actual < expected)
            {
                // Find a missing coordinate on this ring
                foreach (Vector2Int coord in GetRingCoordinates(radius))
                {
                    if (!Hexes.ContainsKey(coord))
                    {
                        Vector3 worldPos = AxialToWorld(coord.x, coord.y, hexWidth, hexHeight, origin);

                        Hex newHex = Instantiate(HexPrefab, worldPos, Quaternion.identity, transform);
                        newHex.Setup(Game);

                        Hexes[coord] = newHex;
                        newHex.Coord = coord;
                        return;
                    }
                }
            }
        }
    }
    public IEnumerable<Hex> GetNeighbors(Hex hex)
    {
        if (hex == null)
            yield break;

        Vector2Int[] dirs =
        {
        new Vector2Int(1, 0),
        new Vector2Int(1, -1),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0),
        new Vector2Int(-1, 1),
        new Vector2Int(0, 1)
    };

        foreach (var dir in dirs)
        {
            Vector2Int neighborCoord = hex.Coord + dir;

            if (Hexes.TryGetValue(neighborCoord, out Hex neighbor))
                yield return neighbor;
        }
    }

    IEnumerable<Vector2Int> GetRingCoordinates(int radius)
    {
        if (radius == 0)
        {
            yield return Vector2Int.zero;
            yield break;
        }

        // Start at (0 - radius, radius)
        int q = -radius;
        int r = radius;

        // Six hex directions in axial coords
        Vector2Int[] dirs =
        {
        new Vector2Int(1, 0),
        new Vector2Int(1, -1),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0),
        new Vector2Int(-1, 1),
        new Vector2Int(0, 1)
    };

        // Walk 6 sides, each length = radius
        Vector2Int pos = new Vector2Int(q, r);

        foreach (var dir in dirs)
        {
            for (int i = 0; i < radius; i++)
            {
                yield return pos;
                pos += dir;
            }
        }
    }

    int HexDistance(Vector2Int a, Vector2Int b)
    {
        // axial hex distance
        int dq = a.x - b.x;
        int dr = a.y - b.y;
        return (Mathf.Abs(dq) + Mathf.Abs(dr) + Mathf.Abs(dq + dr)) / 2;
    }

    int RingSize(int radius)
    {
        return radius == 0 ? 1 : 6 * radius;
    }

    Vector3 AxialToWorld(int q, int r, float width, float height, Vector3 origin)
    {
        // Pointy-top layout conversion
        float x = width * (0.75f * q);
        float y = height * (r + q * 0.5f);

        return new Vector3(origin.x + x, origin.y + y, origin.z);
    }

    public Hex GetEmptyHex()
    {
        var emptyHexes = Hexes.Values.Where(h => h.OccupiedObject == null).ToList();
        if (emptyHexes.Count == 0) return null;

        int index = Random.Range(0, emptyHexes.Count);
        return emptyHexes[index];
    }
}