using UnityEngine;

public class HiveUI : MonoBehaviour
{
	public Game Game;
	public Hive Hive;

	public UpgradeButton UpgradeButton_AddHex;
	public UpgradeButton UpgradeButton_AddNurse;
	public UpgradeButton UpgradeButton_AddHouse;

	public int AddedHexes;
    public int AddedNurses;
    public int AddedHouses;

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
                Hive.HexGrid.AddHex();
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

        UpgradeButton_AddHouse.Setup(
            getLevel: () => AddedHouses,
            canPurchase: (level) =>
            {
                if (Hive.HexGrid.GetEmptyHex() == null)
				{
                    return false;
				}

                if (level >= UpgradeButton_AddHouse.UpgradeDefinition.Max)
                    return false;

                int cost = UpgradeButton_AddHouse.UpgradeDefinition.Costs[level];

                return Game.CanAfford(
                    UpgradeButton_AddHouse.UpgradeDefinition.ResourceType,
                    cost);
            },
            purchase: (level) =>
            {
                int cost = UpgradeButton_AddHouse.UpgradeDefinition.Costs[level];

                Game.Spend(
                    UpgradeButton_AddHouse.UpgradeDefinition.ResourceType,
                    cost);

                AddedHouses++;
                Hive.SpawnHouse();
            }
        );
    }

	public void AddHex_Clicked()
	{
		Hive.HexGrid.AddHex();
	}

	public void AddNurse_Clicked()
	{
		Hive.SpawnNurse();
	}
}
