using UnityEngine;

public class KillPlane : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
		other.gameObject.GetComponent<Bee>().Die();
	}
}
