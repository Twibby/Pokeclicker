using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalConstants : MonoBehaviour
{
    public static Color WhiteZebra = new Color(240f / 255f, 240f / 255f, 240f / 255f);
}

public enum Region
    {
        None = -1,
        Kanto = 0,
        Johto = 1,
        Hoenn = 2,
        Sinnoh = 3,
        Unova = 4,
        Kalos = 5,
        Alola = 6,
        Galar = 7,
        // Throws an error if no region after the final region
        Final = 8,
    }


    public enum PokemonType
    {
        None = -1,
        Normal = 0,
        Fire = 1,
        Water = 2,
        Electric = 3,
        Grass = 4,
        Ice = 5,
        Fighting = 6,
        Poison = 7,
        Ground = 8,
        Flying = 9,
        Psychic = 10,
        Bug = 11,
        Rock = 12,
        Ghost = 13,
        Dragon = 14,
        Dark = 15,
        Steel = 16,
        Fairy = 17,
    }
