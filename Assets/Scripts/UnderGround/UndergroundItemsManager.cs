using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndergroundItemsManager
{
    private static UndergroundItemsManager _singleton;
    public static UndergroundItemsManager Singleton
    {
        get
        {
            if (_singleton == null)
            {
                _singleton = new UndergroundItemsManager();
            }
            return _singleton;
        }
    }

    private List<UndergroundItem> _items = new List<UndergroundItem>();
    public List<UndergroundItem> Items { get { return _items; } }

    public UndergroundItemsManager()
    {
        _items = new List<UndergroundItem>();

        // Diamonds
        _items.Add(new UndergroundItem("Rare Bone", 1, 3));
        _items.Add(new UndergroundItem("Star Piece", 2, 5));
        _items.Add(new UndergroundItem("Revive", 3, 2));
        _items.Add(new UndergroundItem("Max Revive", 4, 4));
        _items.Add(new UndergroundItem("Iron Ball", 5, 2));
        _items.Add(new UndergroundItem("Heart Scale", 6, 10));
        _items.Add(new UndergroundItem("Light Clay", 7, 2));
        _items.Add(new UndergroundItem("Odd Keystone", 8, 6));
        _items.Add(new UndergroundItem("Hard Stone", 9, 4));
        _items.Add(new UndergroundItem("Oval Stone", 10, 3));
        _items.Add(new UndergroundItem("Everstone", 11, 3));
        _items.Add(new UndergroundItem("Smooth Rock", 12, 2));
        _items.Add(new UndergroundItem("Heat Rock", 13, 2));
        _items.Add(new UndergroundItem("Icy Rock", 14, 2));
        _items.Add(new UndergroundItem("Damp Rock", 15, 2));

        // Gem Plates
        _items.Add(new UndergroundGemItem("Draco Plate", 100, 100, PokemonType.Dragon));
        _items.Add(new UndergroundGemItem("Dread Plate", 101, 100, PokemonType.Dark));
        _items.Add(new UndergroundGemItem("Earth Plate", 102, 100, PokemonType.Ground));
        _items.Add(new UndergroundGemItem("Fist Plate", 103, 100, PokemonType.Fighting));
        _items.Add(new UndergroundGemItem("Flame Plate", 104, 100, PokemonType.Fire));
        _items.Add(new UndergroundGemItem("Icicle Plate", 105, 100, PokemonType.Ice));
        _items.Add(new UndergroundGemItem("Insect Plate", 106, 100, PokemonType.Bug));
        _items.Add(new UndergroundGemItem("Iron Plate", 107, 100, PokemonType.Steel));
        _items.Add(new UndergroundGemItem("Meadow Plate", 108, 100, PokemonType.Grass));
        _items.Add(new UndergroundGemItem("Mind Plate", 109, 100, PokemonType.Psychic));
        _items.Add(new UndergroundGemItem("Sky Plate", 110, 100, PokemonType.Flying));
        _items.Add(new UndergroundGemItem("Splash Plate", 111, 100, PokemonType.Water));
        _items.Add(new UndergroundGemItem("Spooky Plate", 112, 100, PokemonType.Ghost));
        _items.Add(new UndergroundGemItem("Stone Plate", 113, 100, PokemonType.Rock));
        _items.Add(new UndergroundGemItem("Toxic Plate", 114, 100, PokemonType.Poison));
        _items.Add(new UndergroundGemItem("Zap Plate", 115, 100, PokemonType.Electric));
        _items.Add(new UndergroundGemItem("Pixie Plate", 116, 100, PokemonType.Fairy));

        // Fossils/Fossil Pieces
        _items.Add(new UndergroundItem("Helix Fossil", 200, 0, UndergroundItem.ValueType.Fossil));
        _items.Add(new UndergroundItem("Dome Fossil", 201, 0, UndergroundItem.ValueType.Fossil));
        _items.Add(new UndergroundItem("Old Amber", 202, 0, UndergroundItem.ValueType.Fossil));
        _items.Add(new UndergroundItem("Root Fossil", 203, 0, UndergroundItem.ValueType.Fossil, new MaxRegionRequirement(Region.Hoenn)));
        _items.Add(new UndergroundItem("Claw Fossil", 204, 0, UndergroundItem.ValueType.Fossil, new MaxRegionRequirement(Region.Hoenn)));
        _items.Add(new UndergroundItem("Armor Fossil", 205, 0, UndergroundItem.ValueType.Fossil, new MaxRegionRequirement(Region.Sinnoh)));
        _items.Add(new UndergroundItem("Skull Fossil", 206, 0, UndergroundItem.ValueType.Fossil, new MaxRegionRequirement(Region.Sinnoh)));
        _items.Add(new UndergroundItem("Cover Fossil", 207, 0, UndergroundItem.ValueType.Fossil, new MaxRegionRequirement(Region.Unova)));
        _items.Add(new UndergroundItem("Plume Fossil", 208, 0, UndergroundItem.ValueType.Fossil, new MaxRegionRequirement(Region.Unova)));
        _items.Add(new UndergroundItem("Jaw Fossil", 209, 0, UndergroundItem.ValueType.Fossil, new MaxRegionRequirement(Region.Kalos)));
        _items.Add(new UndergroundItem("Sail Fossil", 210, 0, UndergroundItem.ValueType.Fossil, new MaxRegionRequirement(Region.Kalos)));
        _items.Add(new UndergroundItem("Fossilized Bird", 211, 0, UndergroundItem.ValueType.FossilPiece, new MaxRegionRequirement(Region.Galar)));
        _items.Add(new UndergroundItem("Fossilized Fish", 212, 0, UndergroundItem.ValueType.FossilPiece, new MaxRegionRequirement(Region.Galar)));
        _items.Add(new UndergroundItem("Fossilized Drake", 213, 0, UndergroundItem.ValueType.FossilPiece, new MaxRegionRequirement(Region.Galar)));
        _items.Add(new UndergroundItem("Fossilized Dino", 214, 0, UndergroundItem.ValueType.FossilPiece, new MaxRegionRequirement(Region.Galar)));

        // Evolution Stones
        _items.Add(new UndergroundEvolutionItem("Fire Stone", 300, 1, UndergroundItem.StoneType.Fire_stone));
        _items.Add(new UndergroundEvolutionItem("Water Stone", 301, 1, UndergroundItem.StoneType.Water_stone));
        _items.Add(new UndergroundEvolutionItem("Thunder Stone", 302, 1, UndergroundItem.StoneType.Thunder_stone));
        _items.Add(new UndergroundEvolutionItem("Leaf Stone", 303, 1, UndergroundItem.StoneType.Leaf_stone));
        _items.Add(new UndergroundEvolutionItem("Moon Stone", 304, 1, UndergroundItem.StoneType.Moon_stone));
        // TODO: Replace these requirements with StoneUnlockedRequirement once moved to modules
        _items.Add(new UndergroundEvolutionItem("Sun Stone", 305, 1, UndergroundItem.StoneType.Sun_stone, new MaxRegionRequirement(Region.Johto)));
        _items.Add(new UndergroundEvolutionItem("Shiny Stone", 306, 1, UndergroundItem.StoneType.Shiny_stone, new MaxRegionRequirement(Region.Sinnoh)));
        _items.Add(new UndergroundEvolutionItem("Dusk Stone", 307, 1, UndergroundItem.StoneType.Dusk_stone, new MaxRegionRequirement(Region.Sinnoh)));
        _items.Add(new UndergroundEvolutionItem("Dawn Stone", 308, 1, UndergroundItem.StoneType.Dawn_stone, new MaxRegionRequirement(Region.Sinnoh)));
        _items.Add(new UndergroundEvolutionItem("Ice Stone", 309, 1, UndergroundItem.StoneType.Ice_stone, new MaxRegionRequirement(Region.Alola)));
        // list.Add(new UndergroundEvolutionItem("Sun Stone", 305, 1, UndergroundItem.StoneType.Sun_stone, new StoneUnlockedRequirement(UndergroundItem.StoneType.Sun_stone)));
        // list.Add(new UndergroundEvolutionItem("Shiny Stone", 306, 1, UndergroundItem.StoneType.Shiny_stone, new StoneUnlockedRequirement(UndergroundItem.StoneType.Shiny_stone)));
        // list.Add(new UndergroundEvolutionItem("Dusk Stone", 307, 1, UndergroundItem.StoneType.Dusk_stone, new StoneUnlockedRequirement(UndergroundItem.StoneType.Dusk_stone)));
        // list.Add(new UndergroundEvolutionItem("Dawn Stone", 308, 1, UndergroundItem.StoneType.Dawn_stone, new StoneUnlockedRequirement(UndergroundItem.StoneType.Dawn_stone)));
        // list.Add(new UndergroundEvolutionItem("Ice Stone", 309, 1, UndergroundItem.StoneType.Ice_stone, new StoneUnlockedRequirement(UndergroundItem.StoneType.Ice_stone)));

        // Shards
        _items.Add(new UndergroundItem("Red Shard", 400, 0, UndergroundItem.ValueType.Shard));
        _items.Add(new UndergroundItem("Yellow Shard", 401, 0, UndergroundItem.ValueType.Shard));
        _items.Add(new UndergroundItem("Green Shard", 402, 0, UndergroundItem.ValueType.Shard));
        _items.Add(new UndergroundItem("Blue Shard", 403, 0, UndergroundItem.ValueType.Shard));
        _items.Add(new UndergroundItem("Grey Shard", 404, 0, UndergroundItem.ValueType.Shard, new MaxRegionRequirement(Region.Johto)));
        _items.Add(new UndergroundItem("Purple Shard", 405, 0, UndergroundItem.ValueType.Shard, new MaxRegionRequirement(Region.Johto)));
        _items.Add(new UndergroundItem("Ochre Shard", 406, 0, UndergroundItem.ValueType.Shard, new MaxRegionRequirement(Region.Hoenn)));
        _items.Add(new UndergroundItem("Black Shard", 407, 0, UndergroundItem.ValueType.Shard, new MaxRegionRequirement(Region.Sinnoh)));
        _items.Add(new UndergroundItem("Crimson Shard", 408, 0, UndergroundItem.ValueType.Shard, new MaxRegionRequirement(Region.Sinnoh)));
        _items.Add(new UndergroundItem("Lime Shard", 409, 0, UndergroundItem.ValueType.Shard, new MaxRegionRequirement(Region.Sinnoh)));
        _items.Add(new UndergroundItem("White Shard", 410, 0, UndergroundItem.ValueType.Shard, new MaxRegionRequirement(Region.Sinnoh)));
        _items.Add(new UndergroundItem("Pink Shard", 411, 0, UndergroundItem.ValueType.Shard, new MaxRegionRequirement(Region.Kalos)));
        _items.Add(new UndergroundItem("Cyan Shard", 412, 0, UndergroundItem.ValueType.Shard, new MaxRegionRequirement(Region.Alola)));
        _items.Add(new UndergroundItem("Rose Shard", 413, 0, UndergroundItem.ValueType.Shard, new MaxRegionRequirement(Region.Galar)));
        _items.Add(new UndergroundItem("Brown Shard", 414, 0, UndergroundItem.ValueType.Shard, new MaxRegionRequirement(Region.Galar)));
    }
}
