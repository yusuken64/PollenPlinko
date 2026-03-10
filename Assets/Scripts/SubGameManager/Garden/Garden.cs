using UnityEngine;

public class Garden : MonoBehaviour, ISubGame
{
	public Camera GardenCamera;
	public Camera GameCamera => GardenCamera;

	public void HandleUpdate(Vector3 worldPoint)
	{
	}
}
