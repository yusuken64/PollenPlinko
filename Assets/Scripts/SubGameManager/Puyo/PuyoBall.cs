using System;
using UnityEngine;

public class PuyoBall : MonoBehaviour
{
    public PuyoColor Color;
    public SpriteRenderer SpriteRenderer;

	public bool IsGarbage;

	public bool Moving;
	private Vector3 _destination;
	public float FallSpeed = 1;

    public Color ColorRed;
    public Color ColorYellow;
    public Color ColorGreen;
    public Color ColorBlue;
    public Color ColorPurple;
    public Color ColorGarbage;
	internal int x;
	internal int y;

	internal void SetColor(PuyoColor color)
    {
        Color = color;
		switch (color)
		{
			case PuyoColor.Red:
				SpriteRenderer.color = ColorRed;
				break;
			case PuyoColor.Blue:
				SpriteRenderer.color = ColorBlue;
				break;
			case PuyoColor.Yellow:
				SpriteRenderer.color = ColorYellow;
				break;
			case PuyoColor.Green:
				SpriteRenderer.color = ColorGreen;
				break;
			case PuyoColor.Purple:
				SpriteRenderer.color = ColorPurple;
				break;
		}
	}

	internal void SetToGarbage()
	{
		IsGarbage = true;
		SpriteRenderer.color = ColorGarbage;
	}

	internal void SetDestination(Vector3 destination, int  x, int y)
	{
		Moving = true;
		_destination = destination;
		this.x = x;
		this.y = y;
	}

	private void Update()
	{
		if (!Moving)
			return;

		transform.localPosition = Vector3.MoveTowards(
			transform.localPosition,
			_destination,
			FallSpeed * Time.deltaTime
		);

		if (transform.localPosition == _destination)
		{
			Moving = false;
		}
	}
}
