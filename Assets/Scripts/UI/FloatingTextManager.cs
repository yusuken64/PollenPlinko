using UnityEngine;

public class FloatingTextManager : MonoBehaviour
{
	public static FloatingTextManager Instance;
	
	public FloatingText FloatingTextPrefab;
	public Transform Container;

	private SimplePool<FloatingText> _pool;

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
	}

	public void ShowText(Vector3 worldPosition, string message)
	{
		var text = _pool.Get();

		text.gameObject.SetActive(true);
		text.Setup(worldPosition, message);
		text.SetRelease(() => _pool.Release(text));
		//text.PlayAndReturn(_pool);
	}
}
