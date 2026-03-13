using UnityEngine;

public class Garden : MonoBehaviour, ISubGame
{
	public SpawnZone SpawnZone;
	public Camera GardenCamera;
	public Camera GameCamera => GardenCamera;

	public void HandleMouseDown(Vector3 worldPoint)
	{
		SpawnZone.HandleClickSpawn(worldPoint);
	}

	public void HandleMouseDrag(Vector3 worldPoint)
	{
	}

	public void HandleMouseUp(Vector3 worldPoint)
	{
	}
}
