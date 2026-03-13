using UnityEngine;

public interface ISubGame
{
	public Camera GameCamera { get; }

	void HandleMouseDown(Vector3 worldPoint);
	void HandleMouseDrag(Vector3 worldPoint);
	void HandleMouseUp(Vector3 worldPoint);
}
