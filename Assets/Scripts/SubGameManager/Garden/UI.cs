using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    public Game Game;
    public ResourceDisplay ResourceDisplay_Bees;
    public ResourceDisplay ResourceDisplay_Pollen;
    public ResourceDisplay ResourceDisplay_Nectar;
    public ResourceDisplay ResourceDisplay_Honey;
    public ResourceDisplay ResourceDisplay_Gold;

	private void Start()
    {
        ResourceDisplay_Bees.Register(Game.Bees);
        ResourceDisplay_Pollen.Register(Game.Pollen);
        ResourceDisplay_Nectar.Register(Game.Nectar);
        ResourceDisplay_Honey.Register(Game.Honey);
        ResourceDisplay_Gold.Register(Game.Gold);
    }
}
