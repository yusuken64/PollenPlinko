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
    }

    private void HandleAutoSpawn()
    {
        _timer += Time.deltaTime;

        if (_timer >= spawnInterval)
        {
            Vector3 pos = GetRandomSpawnPosition();
            TrySpawn(pos);
            _timer = 0f;
        }
    }

    public void HandleClickSpawn(Vector3 worldPoint)
    {
        if (Game.Larvae.Value > 0)
		{
            Game.Larvae.Add(-1);
            Game.Bees.Add(1);
		}
        var spawnPos = GetMouseSpawnPosition(worldPoint);
        TrySpawn(spawnPos);
    }

    private void TrySpawn(Vector3 position)
    {
        if (Game.Bees.Value <= CurrentMult)
            return;

        Game.Bees.Add(-CurrentMult);

        SpawnBee(position);
    }

    private Bee SpawnBee(Vector3 pos)
    {
        var bee = Instantiate(ballPrefab, pos, Quaternion.identity, transform)
            .GetComponent<Bee>();

        bee.Setup(Game, CurrentMult);

        return bee;
    }

    private Vector3 GetMouseSpawnPosition(Vector3 mousePos)
    {
        var spawn = GetRandomSpawnPosition();

        Rect rect = _rectTransform.rect;
        Vector3 local = _rectTransform.InverseTransformPoint(mousePos);

        local.x = Mathf.Clamp(local.x, rect.xMin, rect.xMax);

        spawn.x = _rectTransform.TransformPoint(local).x;

        return spawn;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Rect rect = _rectTransform.rect;

        float x = Random.Range(rect.xMin, rect.xMax);
        float y = Random.Range(rect.yMin, rect.yMax);

        Vector3 local = new Vector3(x, y, 0f);

        return _rectTransform.TransformPoint(local);
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
