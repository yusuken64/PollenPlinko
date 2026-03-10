using DG.Tweening;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ReelView : MonoBehaviour
{
    public Symbol SymbolPrefab;

    public Symbol[] symbols;

    [ContextMenu("Create Symbol")]
    public void CreateSymbols()
    {
        InstantiateSymbols();
    }

    public void InstantiateSymbols()
    {
        for (int i = 0; i < 3; i++)
        {
            Symbol s = Instantiate(SymbolPrefab, transform);
            s.transform.localPosition = new Vector3(0, 1 - i, 0); // top=1, middle=0, bottom=-1
            symbols[i] = s;
        }
    }

    public void SetFromResult(SlotResult result, int reelIndex)
    {
        for (int row = 0; row < 3; row++)
        {
            SymbolType symbolType = result.Get(row, reelIndex);
            symbols[row].SetSymbol(symbolType);
        }
    }

    public void RandomizeSymbols()
    {
        for (int i = 0; i < symbols.Length; i++)
        {
            SymbolType random = (SymbolType)Random.Range(0, System.Enum.GetValues(typeof(SymbolType)).Length);
            symbols[i].SetSymbol(random);
        }
    }

    public void PlayWinAnimation()
    {
        transform.DOKill();

        transform.localScale = Vector3.one;

        transform
            .DOScale(1.3f, 0.2f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                transform
                    .DOScale(1f, 0.2f)
                    .SetEase(Ease.InBack);
            });
    }
}