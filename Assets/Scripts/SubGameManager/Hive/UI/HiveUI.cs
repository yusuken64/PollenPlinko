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
    public UpgradeButton UpgradeButton_AddFlower_Nectar;
    public UpgradeButton UpgradeButton_AddBed;
    public UpgradeButton UpgradeButton_AddScience;


    public int AddedHexes;
    public int AddedNurses;

    public int AddedHouses;
    public int AddedStorage;
    public int AddedFlowers;
    public int AddedFlowersNectar;
    public int AddedBeds;
    public int AddedScience;

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

        UpgradeButton_AddFlower_Nectar.SetupUpgrade(
            Game,
            () => AddedFlowersNectar,
            () =>
            {
                AddedFlowersNectar++;
                Hive.SpawnItem(Hive.FlowerNectarPrefab);
            },
            extraCanPurchase: (level) => Hive.HexGrid.GetEmptyHex() != null
        );

        UpgradeButton_AddBed.SetupUpgrade(
            Game,
            () => AddedBeds,
            () =>
            {
                AddedBeds++;
                Hive.SpawnItem(Hive.BedPrefab);
            },
            extraCanPurchase: (level) => Hive.HexGrid.GetEmptyHex() != null
        );

        UpgradeButton_AddScience.SetupUpgrade(
            Game,
            () => AddedScience,
            () =>
            {
                AddedScience++;
                Hive.SpawnItem(Hive.SciencePrefab);
            },
            extraCanPurchase: (level) => Hive.HexGrid.GetEmptyHex() != null
        );
    }
}
