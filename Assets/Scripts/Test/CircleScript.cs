using System;
using UnityEngine;

public class CircleScript : MonoBehaviour
{
	public float LifeTime = 5;
	private SimplePool<CircleScript> _pool;

	public float CurrentLife;

	void Update()
    {
		CurrentLife += Time.deltaTime;
        if (CurrentLife >= LifeTime)
		{
			_pool.Release(this);
		}
    }

	internal void SetRelease(SimplePool<CircleScript> pool)
	{
		_pool = pool;
	}
}
