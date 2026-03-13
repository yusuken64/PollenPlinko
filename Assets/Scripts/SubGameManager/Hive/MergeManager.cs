using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
    public List<HiveItem> Prefabs;

    public Hive Hive;
    public IEnumerator MergeRoutine(List<Hex> cluster, Hex mergeHex, HiveItem baseItem)
    {
        List<Tween> tweens = new List<Tween>();

        foreach (var hex in cluster)
        {
            if (hex == mergeHex)
                continue;

            var obj = hex.OccupiedObject;
            if (obj == null)
                continue;

            Tween t = obj.transform
                .DOMove(mergeHex.transform.position, 0.35f)
                .SetEase(Ease.InBack);

            tweens.Add(t);
        }

        // wait until all animations finish
        foreach (var t in tweens)
            yield return t.WaitForCompletion();

        // destroy merged items
        foreach (var hex in cluster)
        {
            if (hex.OccupiedObject != null)
            {
                Destroy(hex.OccupiedObject.gameObject);
                hex.OccupiedObject = null;
            }
        }

        int mergeRequirement = 3;
        int spawnCount = cluster.Count / mergeRequirement;

        // spawn upgraded item
        SpawnMergedItems(mergeHex, baseItem, spawnCount);
    }

    void SpawnMergedItems(Hex centerHex, HiveItem baseItem, int count)
    {
        for (int i = 0; i < count; i++)
        {
            var nextPrefab = GetNextPrefab(baseItem);
            var item = Instantiate(nextPrefab);

            // find empty hex near center
            Hex target = Hive.HexGrid.GetEmptyHexNear(centerHex);

            if (target != null)
            {
                item.transform.DOMove(target.transform.position, 0.25f);
                item.Level = baseItem.Level + 1;
                target.SetItem(item, Hive);
            }
        }
    }

    public HiveItem GetNextPrefab(HiveItem item)
    {
        return Prefabs.FirstOrDefault(x => x.ItemType == item.ItemType);
    }
}
