using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotsGame : MonoBehaviour, ISubGame
{
	public Camera SlotsCamera;
	public Camera GameCamera => SlotsCamera;

	public void HandleUpdate(Vector3 worldPoint)
	{
	}

	public List<Reel> Reels;

	const int Rows = 3;

	private void Start()
	{
		for (int i = 0; i < 3; i++)
		{
			Reels.Add(new Reel()
			{
				Symbols = new List<SymbolType>()
				{
					SymbolType.A,
					SymbolType.K,
					SymbolType.K,
					SymbolType.Q,
					SymbolType.Q,
					SymbolType.Q,
					SymbolType.J,
					SymbolType.J,
					SymbolType.J,
					SymbolType.J,
					SymbolType.J,
					SymbolType.J,
					SymbolType.T,
					SymbolType.T,
					SymbolType.T,
					SymbolType.T,
					SymbolType.T,
					SymbolType.T,
					SymbolType.T,
					SymbolType.T,
					SymbolType.N,
					SymbolType.N,
					SymbolType.N,
					SymbolType.N,
					SymbolType.N,
					SymbolType.N,
					SymbolType.N,
					SymbolType.N,
					SymbolType.N,
					SymbolType.N,
					SymbolType.N,
					SymbolType.N,
					SymbolType.N,
					SymbolType.N,
				}
			});
		}
	}

	public ReelView[] ReelViews;
	private bool spinning;

	void DisplayResult(SlotResult result)
	{
		for (int i = 0; i < ReelViews.Length; i++)
		{
			ReelViews[i].SetFromResult(result, i);
		}
	}

	public void Spin_Click()
	{
		if (spinning) { return; }
		Spin();
	}

	public void Spin10_Click()
	{
		if (spinning) { return; }
		StartCoroutine(SpinGamesRoutine(10));
	}

	public void Spin()
	{
		SlotResult result = GenerateResult();
		int payout = EvaluateWin(result);
		DebugResult(result);
		StartCoroutine(SpinRoutine(result, payout));
	}

	IEnumerator SpinGamesRoutine(int games)
	{
		spinning = true;

		for (int i = 0; i < games; i++)
		{
			SlotResult result = GenerateResult();

			int payout = EvaluateWin(result);
			DebugResult(result);

			yield return StartCoroutine(SpinRoutine(result, payout));
		}

		spinning = false;
	}

	IEnumerator SpinRoutine(SlotResult result, int payout)
	{
		float spinTime = 1.5f;
		float timer = 0;

		while (timer < spinTime)
		{
			foreach (var reel in ReelViews)
				reel.RandomizeSymbols();

			timer += 0.05f;
			yield return new WaitForSeconds(0.05f);
		}

		for (int i = 0; i < ReelViews.Length; i++)
		{
			ReelViews[i].SetFromResult(result, i);
			yield return new WaitForSeconds(0.35f);
			var isWin = payout > 0;
			if (isWin)
			{
				ReelViews[i].PlayWinAnimation();
			}
		}
	}

	public SlotResult GenerateResult()
	{
		SlotResult result = new SlotResult(Rows, Reels.Count);

		for (int reel = 0; reel < Reels.Count; reel++)
		{
			Reel r = Reels[reel];

			int stopIndex = r.RandomIndex();

			result.Grid[0, reel] = r.Get(stopIndex - 1);
			result.Grid[1, reel] = r.Get(stopIndex);
			result.Grid[2, reel] = r.Get(stopIndex + 1);
		}

		return result;
	}

	public int EvaluateWin(SlotResult result)
	{
		SymbolType a = result.Get(1, 0);
		SymbolType b = result.Get(1, 1);
		SymbolType c = result.Get(1, 2);

		string line = "";
		for (int reel = 0; reel < Reels.Count; reel++)
		{
			line += result.Get(1, reel) + " ";
		}

		if (a == b && b == c)
		{
			int payout = GetPayout(a);
			Debug.Log("WIN: " + a + " payout=" + payout + line);
			return payout;
		}

		Debug.Log("No win " + line);
		return 0;
	}

	int GetPayout(SymbolType symbol)
	{
		switch (symbol)
		{
			case SymbolType.J:
				return 5;
			case SymbolType.Q:
				return 10;
			case SymbolType.K:
				return 25;
			case SymbolType.A:
				return 100;
			case SymbolType.T:
				return 2;
			case SymbolType.N:
				return 1;
			default:
				return 0;
		}
	}

	void DebugResult(SlotResult result)
	{
		for (int row = 0; row < 3; row++)
		{
			string line = "";

			for (int reel = 0; reel < Reels.Count; reel++)
			{
				line += result.Get(row, reel) + " ";
			}
		}
	}

	public void HandleMouseDown(Vector3 worldPoint)
	{
		throw new System.NotImplementedException();
	}

	public void HandleMouseDrag(Vector3 worldPoint)
	{
		throw new System.NotImplementedException();
	}

	public void HandleMouseUp(Vector3 worldPoint)
	{
		throw new System.NotImplementedException();
	}
}

[System.Serializable]
public class Reel
{
	public List<SymbolType> Symbols;

	public int RandomIndex()
	{
		return Random.Range(0, Symbols.Count);
	}

	public SymbolType Get(int index)
	{
		int wrapped = (index + Symbols.Count) % Symbols.Count;
		return Symbols[wrapped];
	}
}

public class SlotResult
{
	public SymbolType[,] Grid;

	public SlotResult(int rows, int reels)
	{
		Grid = new SymbolType[rows, reels];
	}

	public SymbolType Get(int row, int reel)
	{
		return Grid[row, reel];
	}
}

public enum SymbolType
{
	K,
	Q,
	J,
	A,
	T,
	N,
}