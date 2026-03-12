using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SpawnZone : MonoBehaviour
{
    public Game Game;

    [Header("Prefab")]
    public GameObject ballPrefab;

    [Header("Timing")]
    public float spawnInterval = 1f;

    private RectTransform _rectTransform;
    private float _timer;

    public Slider SpawnRateSlider;

    public ToggleGroup ToggleGroup;
    public Toggle MultToggle1;
    public Toggle MultToggle2;
    public Toggle MultToggle4;
    public Toggle MultToggle10;

    public int CurrentMult;

	private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        SpawnRateSlider.onValueChanged.AddListener(SliderChanged);
    }

	private void Start()
	{
        ToggleChanged(true);
    }

	private void SliderChanged(float t)
    {
		float minInterval = 0.01f;
		float maxInterval = 2f;

		float logMin = Mathf.Log10(minInterval);
		float logMax = Mathf.Log10(maxInterval);

        float logValue = Mathf.Lerp(logMin, logMax, t);
        spawnInterval = Mathf.Pow(10f, logValue);
    }

    private void Update()
    {
        HandleAutoSpawn();
        //HandleClickSpawn();
    }

    private void HandleAutoSpawn()
    {
        _timer += Time.deltaTime;

        if (_timer >= spawnInterval)
        {
            TrySpawnRandom();
        }
    }

    private void HandleClickSpawn()
    {
        if (Mouse.current == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            TrySpawnAtClickX();
        }
    }

    private void TrySpawnRandom()
    {
        if (Game.Bees.Value <= CurrentMult)
            return;

        Game.Bees.Add(-CurrentMult);
        _timer = 0f;

        var newBee = SpawnRandom().GetComponent<Bee>();
        newBee.Setup(Game, CurrentMult);
    }

    private void TrySpawnAtClickX()
    {
        if (Game.Bees.Value <= CurrentMult)
            return;

        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(
            new Vector3(mouseScreenPos.x, mouseScreenPos.y,
                        -Camera.main.transform.position.z)
        );

        Rect rect = _rectTransform.rect;

        Vector3 localClick = _rectTransform.InverseTransformPoint(mouseWorld);

        if (localClick.x < rect.xMin || localClick.x > rect.xMax)
        {
            return;
        }

        Game.Bees.Add(-CurrentMult);

        float worldX = mouseWorld.x;

        Vector3 zoneCenterWorld = _rectTransform.TransformPoint(_rectTransform.rect.center);
        float worldY = zoneCenterWorld.y;

        Vector3 spawnPos = new Vector3(worldX, worldY, 0f);

        var newBee = Instantiate(ballPrefab, spawnPos, Quaternion.identity, this.transform)
                     .GetComponent<Bee>();

        newBee.Setup(Game, CurrentMult);
    }

    private GameObject SpawnRandom()
    {
        Rect rect = _rectTransform.rect;

        float x = Random.Range(rect.xMin, rect.xMax);
        float y = Random.Range(rect.yMin, rect.yMax);

        Vector3 localPoint = new Vector3(x, y, 0f);
        Vector3 worldPoint = _rectTransform.TransformPoint(localPoint);

        return Instantiate(ballPrefab, worldPoint, Quaternion.identity, this.transform);
    }

    public void SetMult(int mult)
	{
        CurrentMult = mult;
	}

    public void ToggleChanged(bool isOn)
    {
        var active = ToggleGroup.GetFirstActiveToggle();

        if (active == MultToggle1 || active == null) { SetMult(1); }
        if (active == MultToggle2) { SetMult(2); }
        if (active == MultToggle4) { SetMult(4); }
        if (active == MultToggle10) { SetMult(10); }
    }
}
