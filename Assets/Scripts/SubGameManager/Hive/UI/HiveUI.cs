using UnityEngine;

public class HiveUI : MonoBehaviour
{
    public Game Game;
    public Hive Hive;

    public UpgradeButton UpgradeButton_AddHex;
    public UpgradeButton UpgradeButton_AddNurse;

    public UpgradeButton UpgradeButton_AddHouse;
    public UpgradeButton UpgradeButton_AddStorage;
    public UpgradeButton UpgradeButton_AddFlower;

    public int AddedHexes;
    public int AddedNurses;

    public int AddedHouses;
    public int AddedStorage;
    public int AddedFlowers;

    public void Start()
    {
        UpgradeButton_AddHex.SetupUpgrade(
            Game,
            () => AddedHexes,
            () =>
            {
                AddedHexes++;
                Hive.HexGrid.AddHex();
            }
        );

        UpgradeButton_AddNurse.SetupUpgrade(
            Game,
            () => AddedNurses,
            () =>
            {
                AddedNurses++;
                Hive.SpawnNurse();
            }
        );

        UpgradeButton_AddHouse.SetupUpgrade(
            Game,
            () => AddedHouses,
            () =>
            {
                AddedHouses++;
                Hive.SpawnItem(Hive.HousePrefab);
            },
            extraCanPurchase: (level) => Hive.HexGrid.GetEmptyHex() != null
        );

        UpgradeButton_AddStorage.SetupUpgrade(
            Game,
            () => AddedStorage,
            () =>
            {
                AddedStorage++;
                Hive.SpawnItem(Hive.StoragePrefab);
            },
            extraCanPurchase: (level) => Hive.HexGrid.GetEmptyHex() != null
        );

        UpgradeButton_AddFlower.SetupUpgrade(
            Game,
            () => AddedFlowers,
            () =>
            {
                AddedFlowers++;
                Hive.SpawnItem(Hive.FlowerPrefab);
            },
            extraCanPurchase: (level) => Hive.HexGrid.GetEmptyHex() != null
        );
    }
}
