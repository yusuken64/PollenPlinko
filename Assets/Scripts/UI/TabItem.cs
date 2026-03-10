using UnityEngine;
using UnityEngine.UI;

public class TabItem : MonoBehaviour
{
	public Button TabButton; //Header
	public GameObject TabContent;

	internal void SetSelected(bool selected)
	{
		this.TabContent.gameObject.SetActive(selected);
	}
}
