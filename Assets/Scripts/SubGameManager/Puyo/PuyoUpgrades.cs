using UnityEngine;

public class PuyoUpgrades : MonoBehaviour
{
    public Game Game;
    public PuyoGame PuyoGame;

    public UpgradeButton UpgradeButton_Row;
    public UpgradeButton UpgradeButton_Col;

    public int AddedRows;
    public int AddedCols;

    void Start()
    {
        //UpgradeButton_Row.Setup(
        //    getLevel: () => AddedRows,
        //    canPurchase: (level) =>
        //    {
        //        if (level >= UpgradeButton_Row.UpgradeDefinition.Max)
        //            return false;

        //        int cost = UpgradeButton_Row.UpgradeDefinition.Costs[level];

        //        return Game.CanAfford(
        //            UpgradeButton_Row.UpgradeDefinition.ResourceType,
        //            cost);
        //    },
        //    purchase: (level) =>
        //    {
        //        int cost = UpgradeButton_Row.UpgradeDefinition.Costs[level];

        //        Game.Spend(
        //            UpgradeButton_Row.UpgradeDefinition.ResourceType,
        //            cost);

        //        AddedRows++;
        //        PuyoGame.AddRow();
        //    }
        //);


        //UpgradeButton_Col.Setup(
        //    getLevel: () => AddedCols,
        //    canPurchase: (level) =>
        //    {
        //        if (level >= UpgradeButton_Col.UpgradeDefinition.Max)
        //            return false;

        //        int cost = UpgradeButton_Col.UpgradeDefinition.Costs[level];

        //        return Game.CanAfford(
        //            UpgradeButton_Col.UpgradeDefinition.ResourceType,
        //            cost);
        //    },
        //    purchase: (level) =>
        //    {
        //        int cost = UpgradeButton_Col.UpgradeDefinition.Costs[level];

        //        Game.Spend(
        //            UpgradeButton_Col.UpgradeDefinition.ResourceType,
        //            cost);

        //        AddedCols++;
        //        PuyoGame.AddCol();
        //    }
        //);
    }
}
