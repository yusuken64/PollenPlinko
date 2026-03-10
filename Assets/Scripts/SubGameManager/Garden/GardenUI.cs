using UnityEngine;

public class GardenUI : MonoBehaviour
{
	public Game Game;
	public FlowerSpawnZone FlowerSpawnZone;

	public UpgradeButton UpgradeButton_MaxFlowers;
	public UpgradeButton UpgradeButton_NectarRate;

    public int AddedMaxFlowers;
    public int AddedNectarRate;

    public void Start()
    {
        UpgradeButton_MaxFlowers.Setup(
            getLevel: () => AddedMaxFlowers,
            canPurchase: (level) =>
            {
                if (level >= UpgradeButton_MaxFlowers.UpgradeDefinition.Max)
                    return false;

                int cost = UpgradeButton_MaxFlowers.UpgradeDefinition.Costs[level];

                return Game.CanAfford(
                    UpgradeButton_MaxFlowers.UpgradeDefinition.ResourceType,
                    cost);
            },
            purchase: (level) =>
            {
                int cost = UpgradeButton_MaxFlowers.UpgradeDefinition.Costs[level];

                Game.Spend(
                    UpgradeButton_MaxFlowers.UpgradeDefinition.ResourceType,
                    cost);

                AddedMaxFlowers++;
                FlowerSpawnZone.maxFlowers = 4 + AddedMaxFlowers;
            });

        UpgradeButton_NectarRate.Setup(
            getLevel: () => AddedNectarRate,
            canPurchase: (level) =>
            {
                if (level >= UpgradeButton_NectarRate.UpgradeDefinition.Max)
                    return false;

                int cost = UpgradeButton_NectarRate.UpgradeDefinition.Costs[level];

                return Game.CanAfford(
                    UpgradeButton_NectarRate.UpgradeDefinition.ResourceType,
                    cost);
            },
            purchase: (level) =>
            {
                int cost = UpgradeButton_NectarRate.UpgradeDefinition.Costs[level];

                Game.Spend(
                    UpgradeButton_NectarRate.UpgradeDefinition.ResourceType,
                    cost);

                AddedNectarRate++;
                FlowerSpawnZone.NectarWeight = AddedNectarRate * 0.2f;
            });
    }
}
