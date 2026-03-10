using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlowerSpawnZone : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject FlowerPrefab_Pollen;
    public GameObject FlowerPrefab_Nectar;

    [Header("Timing")]
    public float spawnInterval = 1f;

    [Header("Limit")]
    public int maxFlowers = 4;

    public Game Game;

    private RectTransform _rectTransform;
    private float _timer;

    private readonly List<GameObject> _activeFlowers = new();

    public TextMeshProUGUI FlowerText;
    public Image FlowerFill;

    public Pin PinPrefab;
    public RectTransform PinArea;
    public int Columns;
    public int Rows;

    public List<Pin> Pins;

    public float PollenWeight = 1;
    public float NectarWeight;

	[ContextMenu("Setup Pins")]
    public void SetupPins()
    {
        Pins.Clear();
        // Clear existing
        for (int i = PinArea.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(PinArea.GetChild(i).gameObject);
        }

        // Get world corners of the PinArea
        Vector3[] corners = new Vector3[4];
        PinArea.GetWorldCorners(corners);

        Vector3 bottomLeft = corners[0];
        Vector3 topRight = corners[2];

        float width = topRight.x - bottomLeft.x;
        float height = topRight.y - bottomLeft.y;

        float spacingX = width / (Columns + 1);
        float spacingY = height / (Rows + 1);

        for (int y = 0; y < Rows; y++)
        {
            bool isStaggeredRow = y % 2 == 1;

            int columnsThisRow = isStaggeredRow ? Columns - 1 : Columns;
            float rowOffset = isStaggeredRow ? spacingX * 0.5f : 0f;

            for (int x = 0; x < columnsThisRow; x++)
            {
                Pin pin = Instantiate(PinPrefab, PinArea);

                float worldX = bottomLeft.x + spacingX * (x + 1) + rowOffset;
                float worldY = bottomLeft.y + spacingY * (y + 1);

                pin.transform.position = new Vector3(worldX, worldY, bottomLeft.z);
                Pins.Add(pin);
            }
        }
    }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        // Clean up destroyed references
        _activeFlowers.RemoveAll(f => f == null);

        // If we're already at max, do nothing
        if (_activeFlowers.Count >= maxFlowers)
            return;

        _timer += Time.deltaTime;

        if (_timer >= spawnInterval)
        {
            _timer = 0f;
			var newItem = Spawn();
			if (newItem != null)
			{
				_activeFlowers.Add(newItem);
			}
		}

		FlowerText.text = $"Flowers { _activeFlowers.Count()} / {maxFlowers}";
        FlowerFill.fillAmount = _timer / spawnInterval;
    }

    private GameObject Spawn()
    {
        if (FlowerPrefab_Pollen == null || _rectTransform == null)
            return null;

        if(!Pins.Any(x => x.OccupiedFlower == null))
		{
            return null;
		}

        float total = PollenWeight + NectarWeight;
        float roll = Random.value * total;

        GameObject flowerPrefab;
        if (roll < PollenWeight)
        {
            flowerPrefab = FlowerPrefab_Pollen;
        }
		else
		{
            flowerPrefab = FlowerPrefab_Nectar;
		}

        var emptyPins = Pins.Where(x => x.OccupiedFlower == null).ToList();
        var randomPin = emptyPins[Random.Range(0, emptyPins.Count())];

        float randomAngle = Random.Range(0f, 360f);
        Quaternion rotation = Quaternion.Euler(0f, 0f, randomAngle);
        var newItem = Instantiate(flowerPrefab, randomPin.transform.position, rotation, this.transform);

        randomPin.OccupiedFlower = newItem;
        newItem.GetComponent<Flower>().Setup(Game, randomPin);

        return newItem;
    }
}