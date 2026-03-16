using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ToggleBoolButton : MonoBehaviour
{
	public bool IsOn;
	public TextMeshProUGUI label;

	public string OnText;
	public string OffText;

	public UnityEvent<bool> OnValueChanged;

	public void Toggle()
	{
		IsOn = !IsOn;
		UpdateUI();
		OnValueChanged?.Invoke(IsOn);
	}

	private void UpdateUI()
	{
		label.text = IsOn ? OnText : OffText;
	}

	public void SetToOn()
	{
		IsOn = true;
		UpdateUI();
		OnValueChanged?.Invoke(IsOn);
	}

	public void SetToOff()
	{
		IsOn = false;
		UpdateUI();
		OnValueChanged?.Invoke(IsOn);
	}

	public void Set(bool value)
	{
		IsOn = value;
		UpdateUI();
		OnValueChanged?.Invoke(IsOn);
	}
}