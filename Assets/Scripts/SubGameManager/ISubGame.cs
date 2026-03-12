using UnityEngine;

public interface ISubGame
{
	public Camera GameCamera { get; }

	public void HandleUpdate(Vector3 worldPoint);
	void HandleMouseDown(Vector3 worldPoint);
	void HandleMouseDrag(Vector3 worldPoint);
	void HandleMouseUp(Vector3 worldPoint);
}
