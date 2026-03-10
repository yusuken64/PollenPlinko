using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PuyoGame : MonoBehaviour, ISubGame
{
    public Game Game;
    public PuyoBall[,] Balls;
    public PuyoBall PuyoBallPrefab;

    public Transform BallContainer;

    public int Width;
    public int Height;

    public float CellSize;
    public Vector3 StartPosition;

    public Camera PuyoCamera;

    [SerializeField] private float WaitTime = 0.5f;
    private float _waitTimer = 0f;
    private bool _cascading;
    private Queue<List<Vector2Int>> _popQueue = new Queue<List<Vector2Int>>();
    private bool _popping = false;
    private float _popDelay = 0.5f;
    public Camera GameCamera => PuyoCamera;

    public TextMeshProUGUI Text;
    public int Combo;
    public int Games;
    public int Bet;
    public int Win;

    public int RowsToAdd;
    public int ColsToAdd;

    private void Start()
	{
        Balls = new PuyoBall[Width, Height];
        ResizeBoard();
	}

	private void ResizeBoard()
	{
        ClearBoard();
        BallContainer.transform.localPosition = new Vector3((-Width / 2) * CellSize, (-Height / 2) * CellSize, 0);
		Balls = new PuyoBall[Width, Height];
	}

	internal void AddRow()
	{
        RowsToAdd++;
        if (!_cascading)
        {
            HandleBoardUpgrades();
        }
    }

    internal void AddCol()
    {
        ColsToAdd++;
        if (!_cascading)
        {
            HandleBoardUpgrades();
        }
    }

    private void HandleBoardUpgrades()
    {
        if (ColsToAdd == 0 &&
            RowsToAdd == 0)
		{
            return;
		}

        Width += ColsToAdd;
        Height += RowsToAdd;

        ColsToAdd = 0;
        RowsToAdd = 0;
        ResizeBoard();
    }

    public void StartGame()
    {
        FillBoard();
        StartCascade();
    }

    public void StartCascade()
    {
        _cascading = true;
        _waitTimer = WaitTime;
    }

    private Vector3 PositionFor(int x, int y)
    {
        return new Vector3(
            x * CellSize,
            y * CellSize,
            0);
    }

    private void Update()
    {
        Text.text = @$"Combo {Combo}
Games {Games}
BET {Bet}
Win {Win}";

        if (!_cascading)
            return;

        bool anyMoving = false;
        foreach (var ball in Balls)
        {
            if (ball != null && ball.Moving)
            {
                anyMoving = true;
                break;
            }
        }

        if (anyMoving)
        {
            _waitTimer = WaitTime;
            return;
        }

        // Handle popping gradually
        UpdatePopping();
        if (_popping)
            return;

        // Countdown before resolving board
        _waitTimer -= Time.deltaTime;
        if (_waitTimer > 0)
            return;

        // Resolve new matches
        var matches = FindMatches();
        if (matches.Count == 0)
        {
            if (Games > 0)
            {
                Games--;
                Combo = 0;
                ClearBoard();
                FillBoard();
                StartCascade();
            }
            else
            {
                _cascading = false;
                Combo = 0;

                //Game.Nectar.Add(Win);
                HandleBoardUpgrades();
            }
        }
        else
        {
            EnqueueMatches(matches);
            _waitTimer = _popDelay;
        }
    }

	private void UpdatePopping()
    {
        if (!_popping) return;

        if (_waitTimer > 0)
        {
            _waitTimer -= Time.deltaTime;
            return;
        }

        if (_popQueue.Count > 0)
        {
            var match = _popQueue.Dequeue();

            HashSet<Vector2Int> garbageToDestroy = new HashSet<Vector2Int>();

            Payout(match);
            foreach (var ballPos in match)
            {
                var puyoBall = Balls[ballPos.x, ballPos.y];
                if (puyoBall != null)
                {
                    Balls[ballPos.x, ballPos.y] = null;
                    PopBall(puyoBall);
                }

                CheckGarbage(ballPos, garbageToDestroy);
            }

            foreach (var pos in garbageToDestroy)
            {
                var garbage = Balls[pos.x, pos.y];
                if (garbage != null && garbage.IsGarbage)
                {
                    Balls[pos.x, pos.y] = null;
                    PopBall(garbage);
                }
            }

            Combo += 1;
            Win += ScoreMatch(match.Count, Combo) * Bet;
            _waitTimer = _popDelay;
        }
        else
        {
            _popping = false;
            _popQueue.Clear();
            ApplyGravity();
            FillBoard();
            _waitTimer = WaitTime;
        }
    }

	private void Payout(List<Vector2Int> match)
    {
        var ballPos = match[0];
        var puyoBall = Balls[ballPos.x, ballPos.y];

		switch (puyoBall.Color)
		{
			case PuyoColor.Red:
                Game.Honey.Add(1 * Bet);
				break;
			case PuyoColor.Blue:
                Game.Nectar.Add(1 * Bet);
                break;
			case PuyoColor.Yellow:
                Game.Pollen.Add(1 * Bet);
                break;
			case PuyoColor.Green:
                Game.Gold.Add(1 * Bet);
                break;
			case PuyoColor.Purple:
                Game.Jelly.Add(1 * Bet);
                break;
		}
	}

	public void PopBall(PuyoBall ball)
    {
        Transform t = ball.transform;
        SpriteRenderer sr = ball.GetComponent<SpriteRenderer>();

        Sequence seq = DOTween.Sequence();

        // squash/stretch
        seq.Append(t.DOScale(new Vector3(1.25f, 0.8f, 1f), 0.06f));

        // stretch opposite
        seq.Append(t.DOScale(new Vector3(0.8f, 1.25f, 1f), 0.06f));

        // hop slightly
        seq.Append(t.DOMoveY(t.position.y + 0.25f, 0.08f));

        // shrink away
        seq.Append(t.DOScale(0f, 0.12f).SetEase(Ease.InBack));

        // optional flash
        if (sr != null)
        {
            seq.Join(sr.DOColor(Color.white, 0.1f));
        }

        seq.OnComplete(() =>
        {
            Destroy(ball.gameObject);
        });
    }

    private void CheckGarbage(Vector2Int pos, HashSet<Vector2Int> garbage)
    {
        Vector2Int[] dirs =
        {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

        foreach (var dir in dirs)
        {
            Vector2Int n = pos + dir;

            if (n.x < 0 || n.x >= Width || n.y < 0 || n.y >= Height)
                continue;

            var ball = Balls[n.x, n.y];

            if (ball != null && ball.IsGarbage)
            {
                garbage.Add(n);
            }
        }
    }

    int ScoreMatch(int matchSize, int combo)
    {
        float groupMult = 1f + Mathf.Max(0, matchSize - 4) * 0.25f;
        groupMult = Mathf.Min(groupMult, 2.5f);

        float comboMult = ComboMult[Mathf.Min(combo - 1, ComboMult.Length - 1)];

        return (int)(groupMult * comboMult);
    }
    static readonly float[] ComboMult =
{
    1f,   // 1
    1.4f, // 2
    2f,   // 3
    3f,   // 4
    4.5f, // 5
    6f,   // 6
    8f,   // 7
    10f,  // 8
    12f,  // 9
    15f,  // 10
    18f,  // 11
    20f   // 12+
};
    private void EnqueueMatches(List<List<Vector2Int>> matches)
    {
        foreach (var match in matches)
        {
            _popQueue.Enqueue(match);
        }

        _popping = _popQueue.Count > 0;
    }

    private PuyoBall SpawnBall(int x, int y)
    {
        var isGarbage = UnityEngine.Random.Range(0, 3) == 0;

        var newBall = Instantiate(PuyoBallPrefab, BallContainer);

        if (isGarbage)
        {
            newBall.SetToGarbage();
        }
        else
        {
            PuyoColor randomColor =
                (PuyoColor)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(PuyoColor)).Length);
            newBall.SetColor(randomColor);
        }
        newBall.transform.localPosition = PositionFor(x, y + Height);
        newBall.SetDestination(PositionFor(x, y), x, y);

        return newBall;
    }

    public List<List<Vector2Int>> FindMatches()
    {
        PuyoBall[,] balls = Balls;

        int width = balls.GetLength(0);
        int height = balls.GetLength(1);

        bool[,] visited = new bool[width, height];

        List<List<Vector2Int>> matches = new List<List<Vector2Int>>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (visited[x, y] || balls[x, y] == null || balls[x, y].IsGarbage)
                    continue;

                List<Vector2Int> group = FloodFill(balls, visited, x, y);

                if (group.Count >= 3)
                {
                    matches.Add(group);
                }
            }
        }

        return matches;
    }

    private List<Vector2Int> FloodFill(PuyoBall[,] balls, bool[,] visited, int startX, int startY)
    {
        int width = balls.GetLength(0);
        int height = balls.GetLength(1);

        PuyoColor color = Balls[startX, startY].Color;

        List<Vector2Int> result = new List<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();

        queue.Enqueue(new Vector2Int(startX, startY));
        visited[startX, startY] = true;

        Vector2Int[] dirs =
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        while (queue.Count > 0)
        {
            Vector2Int p = queue.Dequeue();
            result.Add(new Vector2Int(p.x, p.y));

            foreach (var d in dirs)
            {
                int nx = p.x + d.x;
                int ny = p.y + d.y;

                if (nx < 0 || ny < 0 || nx >= width || ny >= height)
                    continue;

                if (visited[nx, ny])
                    continue;

                if (balls[nx, ny] == null)
                    continue;

                if (balls[nx, ny].Color != color)
                    continue;

                if (balls[nx, ny].IsGarbage)
                    continue;

                visited[nx, ny] = true;
                queue.Enqueue(new Vector2Int(nx, ny));
            }
        }

        return result;
    }

    void ApplyGravity()
    {
        int width = Balls.GetLength(0);
        int height = Balls.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            int writeY = 0;

            for (int y = 0; y < height; y++)
            {
                if (Balls[x, y] != null)
                {
                    if (y != writeY)
                    {
                        Balls[x, writeY] = Balls[x, y];
                        Balls[x, y] = null;

                        Balls[x, writeY].SetDestination(PositionFor(x, writeY), x, writeY);
                    }

                    writeY++;
                }
            }
        }
    }

    void FillBoard()
    {
        int width = Balls.GetLength(0);
        int height = Balls.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (Balls[x, y] == null)
                {
                    Balls[x, y] = SpawnBall(x, y);
                }
            }
        }
    }

    public void HandleUpdate(Vector3 worldPoint)
    {
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        if (hit.collider != null)
        {
            var ball = hit.collider.GetComponent<PuyoBall>();
            if (ball == null)
            {
                return;
            }

            var puyoBall = Balls[ball.x, ball.y];
            Balls[ball.x, ball.y] = null;
            Destroy(puyoBall.gameObject);

            ApplyGravity();
            FillBoard();
            StartCascade();
        }
        else
        {
            Debug.Log("Miss");
        }
    }

    public void Spin_Clicked()
    {
        Combo = 0;
        Games = 5;
		Win = 0;

		int cost = Bet * Games;
        Game.Nectar.Add(-cost);

        ClearBoard();
        FillBoard();
        StartCascade();
    }

    public void BetPlus_Clicked()
	{
        Bet++;
	}

    public void BetMinus_Clicked()
    {
        Bet--;
    }

    public void ClearBoard()
    {
        foreach (var ball in Balls)
        {
            if (ball != null)
            {
                Destroy(ball.gameObject);
            }
        }

        Array.Clear(Balls, 0, Balls.Length);

        _cascading = false;
        _waitTimer = 0f;
    }
}

public enum PuyoColor
{
    Red,
    Blue,
    Yellow,
    Green,
    Purple,
}
