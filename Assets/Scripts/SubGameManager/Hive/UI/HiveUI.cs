using UnityEngine;

public class HiveUI : MonoBehaviour
{
	public Game Game;
	public Hive Hive;

	public UpgradeButton UpgradeButton_AddHex;
	public UpgradeButton UpgradeButton_AddNurse;

	public int AddedHexes;
    public int AddedNurses;

    public void Start()
	{
        UpgradeButton_AddHex.Setup(
            getLevel: () => AddedHexes,
            canPurchase: (level) =>
            {
                if (level >= UpgradeButton_AddHex.UpgradeDefinition.Max)
                    return false;

                int cost = UpgradeButton_AddHex.UpgradeDefinition.Costs[level];

                return Game.CanAfford(
                    UpgradeButton_AddHex.UpgradeDefinition.ResourceType,
                    cost);
            },
            purchase: (level) =>
            {
                int cost = UpgradeButton_AddHex.UpgradeDefinition.Costs[level];

                Game.Spend(
                    UpgradeButton_AddHex.UpgradeDefinition.ResourceType,
                    cost);

                AddedHexes++;
                Hive.AddHex();
            }
        );

        UpgradeButton_AddNurse.Setup(
            getLevel: () => AddedNurses,
            canPurchase: (level) =>
            {
                if (level >= UpgradeButton_AddNurse.UpgradeDefinition.Max)
                    return false;

                int cost = UpgradeButton_AddNurse.UpgradeDefinition.Costs[level];

                return Game.CanAfford(
                    UpgradeButton_AddNurse.UpgradeDefinition.ResourceType,
                    cost);
            },
            purchase: (level) =>
            {
                int cost = UpgradeButton_AddNurse.UpgradeDefinition.Costs[level];

                Game.Spend(
                    UpgradeButton_AddNurse.UpgradeDefinition.ResourceType,
                    cost);

                AddedNurses++;
                Hive.SpawnNurse();
            }
        );
    }

	public void AddHex_Clicked()
	{
		Hive.AddHex();
	}

	public void AddNurse_Clicked()
	{
		Hive.SpawnNurse();
	}
}
