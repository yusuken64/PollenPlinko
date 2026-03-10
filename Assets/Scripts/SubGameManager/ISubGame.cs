using UnityEngine;

public interface ISubGame
{
	public Camera GameCamera { get; }

	public void HandleUpdate(Vector3 worldPoint);
}
