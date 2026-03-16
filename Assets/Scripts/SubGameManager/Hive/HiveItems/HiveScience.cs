public class HiveScience : HiveItem
{
	public override void AfterSetup()
	{
		base.AfterSetup();

		var puyoGame = FindFirstObjectByType<PuyoGame>(UnityEngine.FindObjectsInactive.Include);
		var maxLevel = FindFirstObjectByType<MergeManager>(UnityEngine.FindObjectsInactive.Include)
			.MaxLevelOf(this.ItemType);
		puyoGame.SetLevel(maxLevel);
	}
}
