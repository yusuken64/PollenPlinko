using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlowerSpawnZone : MonoBehaviour
{
    [Header("Prefab")]
    public Flower FlowerPrefab_Pollen;
    public Flower FlowerPrefab_Nectar;

    [Header("Timing")]
    public float spawnInterval = 1f;

    [Header("Limit")]
    public int maxFlowers = 4;

    public Game Game;

    private RectTransform _rectTransform;
    private float _timer;

    private readonly List<Flower> _activeFlowers = new();

    public TextMeshProUGUI FlowerText;

    public Image FlowerFill;

    public Pin PinPrefab;
    public RectTransform PinArea;
    public int Columns;
    public int Rows;

    public List<Pin> Pins;

    public float PollenWeight = 1;
    public float NectarWeight;
    private SimplePool<Flower> _poolPollen;
    private SimplePool<Flower> _poolNectar;


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

        _poolPollen = new SimplePool<Flower>(
          createFunc: () =>
          {
              var obj = Instantiate(FlowerPrefab_Pollen, this.transform);
              obj.gameObject.SetActive(false);
              return obj;
          }
         );

        _poolNectar = new SimplePool<Flower>(
          createFunc: () =>
          {
              var obj = Instantiate(FlowerPrefab_Nectar, this.transform);
              obj.gameObject.SetActive(false);
              return obj;
          }
         );
    }

    private void Start()
    {
        if (_activeFlowers.Count() <= 0)
        {
            SpawnFlowers();
        }
    }

    private void Update()
    {
        FlowerText.text = $"Flowers { _activeFlowers.Count()} / {maxFlowers}";
        FlowerFill.fillAmount = _timer / spawnInterval;
    }

    public void SpawnFlowers()
    {
        _activeFlowers.Clear();

        var hiveUI = FindFirstObjectByType<HiveUI>(FindObjectsInactive.Include);
        var flowers = hiveUI.AddedFlowers + 1;
        var flowers_nectar = hiveUI.AddedFlowersNectar;

        for (int i = 0; i < flowers; i++)
        {
            var newItem = Spawn(_poolPollen);
            if (newItem != null)
            {
                _activeFlowers.Add(newItem);
            }
        }

        for (int i = 0; i < flowers_nectar; i++)
        {
            var newItem = Spawn(_poolNectar);
            if (newItem != null)
            {
                _activeFlowers.Add(newItem);
            }
        }
    }

    internal void FlowerDestroyed(Flower flower)
    {
        if (!_activeFlowers.Contains(flower))
		{
            throw new System.Exception("no flower");
		}
        _activeFlowers.Remove(flower);
        if (_activeFlowers.Count() <= 0)
        {
            SpawnFlowers();
        }
    }

    [ContextMenu("Check Flowers")]
    public void CheckFlowers()
    {
        if (_activeFlowers.Count() <= 0)
        {
            SpawnFlowers();
        }
    }

    private Flower Spawn(SimplePool<Flower> pool)
    {
        if (!Pins.Any(x => x.OccupiedFlower == null))
        {
            return null;
        }

        var emptyPins = Pins.Where(x => x.OccupiedFlower == null).ToList();
        var randomPin = emptyPins[Random.Range(0, emptyPins.Count())];

        float randomAngle = Random.Range(0f, 360f);
        Quaternion rotation = Quaternion.Euler(0f, 0f, randomAngle);

        Flower newItem = pool.Get();
        newItem.transform.position = randomPin.transform.position;
        newItem.transform.SetPositionAndRotation(randomPin.transform.position, rotation);

        randomPin.OccupiedFlower = newItem.gameObject;
        newItem.Setup(Game, randomPin);
        newItem.gameObject.SetActive(true);
        newItem.SetRelease(() =>
        {
            newItem.gameObject.SetActive(false);
            pool.Release(newItem);
        });

        return newItem;
    }
}