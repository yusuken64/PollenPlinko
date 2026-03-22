using UnityEngine;

public class FloatingTextManager : MonoBehaviour
{
	public static FloatingTextManager Instance;
	
	public FloatingText FloatingTextPrefab;
	public Transform Container;

	private SimplePool<FloatingText> _pool;

	public bool SupressText;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}

		_pool = new SimplePool<FloatingText>(
			createFunc: () =>
			{
				var obj = Instantiate(FloatingTextPrefab, Container);
				obj.gameObject.SetActive(false);
				return obj;
			}
		);

		_pool.LimitCount = true;
		_pool.Limit = 500;
	}

	public void ShowText(Vector3 worldPosition, string message)
	{
		if (SupressText) { return; }

		var text = _pool.Get();

		text.gameObject.SetActive(true);
		text.Setup(worldPosition, message, Color.black);
		text.SetRelease(_pool.Release);
		//text.PlayAndReturn(_pool);
	}
}
