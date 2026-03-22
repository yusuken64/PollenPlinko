using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static readonly string AUDIOPOOLID_DEFAULT	= "AUDIOPOOLID_DEFAULT";
	public static readonly string AUDIOPOOLID_PINS		= "AUDIOPOOLID_PINS";
	public static readonly string AUDIOPOOLID_FLOWER	= "AUDIOPOOLID_FLOWER";

	public static AudioManager Instance;

	public List<AudioPool> PoolConfigs;
	private Dictionary<string, AudioPool> Pools = new();

	private void Awake()
	{
		Instance = this;

		foreach (var pool in PoolConfigs)
		{
			pool.Initialize(gameObject);
			Pools[pool.Id] = pool;
		}
	}

	public void PlaySFX(AudioClip clip, string poolId, bool randomPitch = false)
	{
		if (!Pools.TryGetValue(poolId, out var pool))
		{
			Debug.LogWarning($"No audio pool with id {poolId}");
			return;
		}

		var source = pool.GetAvailableSource(gameObject);
		if (source == null) { return; }

		source.pitch = randomPitch ? Random.Range(0.95f, 1.05f) : 1f;
		source.PlayOneShot(clip);
	}
}

[System.Serializable]
public class AudioPool
{
	public string Id;
	public int InitialSize = 3;
	public int MaxSize = 10;

	public Transform Parent;

	public float DefaultVolume = 1f;
	public bool Spatial = false;

	private List<AudioSource> sources = new List<AudioSource>();

	public bool CycleByIndex;
	public int CycleIndex;

	public void Initialize(GameObject owner)
	{
		for (int i = 0; i < InitialSize; i++)
		{
			CreateSource(owner);
		}
	}

	private AudioSource CreateSource(GameObject owner)
	{
		var go = new GameObject();
		go.transform.SetParent(Parent != null ? Parent : owner.transform);
		go.name = $"{Id}_AudioSource_{sources.Count}";

		var source = go.AddComponent<AudioSource>();
		sources.Add(source);

		source.volume = DefaultVolume;
		source.spatialBlend = Spatial ? 1f : 0f;

		return source;
	}

	public AudioSource GetAvailableSource(GameObject owner)
	{
		//foreach (var source in sources)
		//{
		//	if (!source.isPlaying)
		//		return source;
		//}

		// Expand if under limit
		//if (sources.Count < MaxSize)
		//{
		//	return CreateSource(owner);
		//}

		if (CycleByIndex)
		{
			CycleIndex = (CycleIndex + 1) % sources.Count;
			return sources[CycleIndex];
		}

		return null;
	}
}