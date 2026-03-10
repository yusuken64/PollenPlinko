using UnityEngine;

public class Shop : MonoBehaviour, ISubGame
{
	public Camera ShopCamera;
	public Camera GameCamera => ShopCamera;

	public void HandleUpdate(Vector3 worldPoint)
	{
	}
}
