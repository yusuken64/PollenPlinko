using System.Collections.Generic;
using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
	public Game Game;

	public UnlockableObject HiveSubGameButton;
	public UnlockableObject GardenSubGameButton;
	public UnlockableObject PuyoSubGameButton;

	List<UnlockableObject> UnlockableObjects;

	private void Start()
	{
		UnlockableObjects = new List<UnlockableObject>()
		{
			HiveSubGameButton,
			GardenSubGameButton,
			PuyoSubGameButton,
		};

		HiveSubGameButton.VisibleCondition = () => true;
		HiveSubGameButton.UnlockCondition = () => true;

		GardenSubGameButton.VisibleCondition = () => Game.Larvae.Value > 10;
		GardenSubGameButton.UnlockCondition = () => false;
	}

	private void Update()
	{
		foreach(var unlockable in UnlockableObjects)
		{
			unlockable.Refresh();
		}
	}
}
