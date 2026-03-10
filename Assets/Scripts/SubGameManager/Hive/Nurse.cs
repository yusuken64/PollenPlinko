using System.Linq;
using UnityEngine;

public class Nurse : MonoBehaviour
{
    public Hive Hive;

    public float MoveSpeed = 2f;

    private Hex _targetHex;
    private bool _isMoving;

    void Update()
    {
        MoveToTarget();

        if (ReachedTarget())
        {
            HarvestEgg();
            SelectNewTarget();
        }
    }

	private void HarvestEgg()
	{
		if ((_targetHex?.OccupiedObect) == null)
		{
			return;
		}

		var egg = _targetHex?.OccupiedObect?.GetComponent<Egg>();

		if (egg != null)
		{
			egg.Hatch();
		}
	}

	void SelectNewTarget()
    {
        if (_targetHex == null)
        {
            _targetHex = Hive.Hexes[new Vector2Int(0, 0)];
        }

        Hex currentHex = _targetHex;

        if (currentHex != null)
        {
            var fullNeighbors = Hive.GetNeighbors(currentHex)
                .Where(h => h.OccupiedObect != null)
                .ToList();

            if (fullNeighbors.Count > 0)
            {
                _targetHex = fullNeighbors[Random.Range(0, fullNeighbors.Count)];
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
}