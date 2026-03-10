using TMPro;
using UnityEngine;

public class Symbol : MonoBehaviour
{
	public TextMeshPro SymbolText;
	public SpriteRenderer SpriteRenderer;

	public void SetSymbol(SymbolType symbolType)
	{
		SymbolText.text = symbolType.ToString();
	}
}
