using System.Linq;
using UnityEngine;

public class Queen : MonoBehaviour
{
    public Hive Hive;
    public Egg EggPrefab;

    public float MoveSpeed = 2f;

    private Hex _targetHex;
    private bool _isMoving;

    void Update()
    {
        MoveToTarget();

        if (ReachedTarget())
        {
            LayEgg();
            SelectNewTarget();
        }
    }

    void SelectNewTarget()
    {
        if (_targetHex == null)
		{
            _targetHex = Hive.Hexes[new Vector2Int(0,0)];
        }
        
        Hex currentHex = _targetHex; // or whatever represents bee's current location

        // Step 1. Try empty neighbors first
        if (currentHex != null)
        {
            var emptyNeighbors = Hive.GetNeighbors(currentHex)
                .Where(h => h.OccupiedObect == null)
                .ToList();

            if (emptyNeighbors.Count > 0)
            {
                _targetHex = emptyNeighbors[Random.Range(0, emptyNeighbors.Count)];
                _isMoving = true;
                return;
            }
        }

        // Step 2. Fallback to any empty hex in hive
        var emptyHexes = Hive.Hexes.Values
            .Where(h => h.OccupiedObect == null)
            .ToList();

        if (emptyHexes.Count == 0)
            return;

        _targetHex = emptyHexes[Random.Range(0, emptyHexes.Count)];
        _isMoving = true;
    }

    void MoveToTarget()
    {
        if (_targetHex == null)
		{
            return;
		}

        Vector3 direction = (_targetHex.transform.position - transform.position).normalized;
        transform.position += direction * MoveSpeed * Time.deltaTime;
    }

    bool ReachedTarget()
    {
        if (_targetHex == null)
        {
            return true;
		}

        return Vector3.Distance(transform.position, _targetHex.transform.position) < 0.05f;
    }

    void LayEgg()
    {
        if (_targetHex == null || _targetHex.OccupiedObect != null)
        {
            return;
        }

        var newEgg = Instantiate(EggPrefab, _targetHex.transform.position, Quaternion.identity, _targetHex.transform);

        _targetHex.OccupiedObect = newEgg.gameObject;
        newEgg.Setup(_targetHex, Hive.Game);
    }
}
