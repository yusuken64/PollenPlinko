using UnityEngine;

public class TestSpawner : MonoBehaviour
{
	public CircleScript BallPrefab;
	public RectTransform SpawnArea;

	public float TimeMax;
	public float CurrentTime;
	private SimplePool<CircleScript> pool;

	private void Awake()
	{
		pool = new SimplePool<CircleScript>(
			createFunc: () =>
			{
				var obj = Instantiate(BallPrefab, this.transform);
				obj.SetRelease(pool);
				return obj;
			});
	}

	private void Update()
	{
		CurrentTime += Time.deltaTime;

		if (CurrentTime >= TimeMax)
		{
			CurrentTime = 0;
			var newItem = pool.Get();
			newItem.transform.position = GetRandomSpawnPosition();
			newItem.CurrentLife = 0;
			Rigidbody2D rb = newItem.GetComponent<Rigidbody2D>();
			rb.linearVelocity = Vector3.zero;
			newItem.GetComponent<Bee>().Setup(3, 1);
		}
	}

	public Vector3 GetRandomSpawnPosition()
	{
		Rect rect = SpawnArea.rect;

		float x = Random.Range(rect.xMin, rect.xMax);
		float y = Random.Range(rect.yMin, rect.yMax);

		Vector3 local = new Vector3(x, y, 0f);

		return SpawnArea.TransformPoint(local);
	}

}
