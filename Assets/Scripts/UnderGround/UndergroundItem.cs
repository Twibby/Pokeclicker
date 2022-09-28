using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UndergroundItem
{
    private string itemName;
    private int id;
    private int value;
    public ValueType valueType { get; }
    private Requirement? requirement;
    protected int? type;

    public UndergroundItem( string p_name, int p_id, int p_val, ValueType p_valueType = ValueType.Diamond, Requirement p_requirement = null)
    {
        this.itemName = p_name;
        this.id = p_id;
        this.value = p_val;
        this.valueType = p_valueType;
        this.requirement = p_requirement;
    }

    public bool IsUnlocked()
    { 
        return this.requirement != null ? this.requirement.isCompleted() : true;
    }

    #region Accessors
    public int Id { get { return this.id; } }

    public string DisplayName { get { return this.itemName; } }

    public string ExtendedName { get { return this.itemName + (this.valueType == ValueType.Diamond ? " (" + this.value + ")" : ""); } }

    public string ImagePath
    {
        get
        {
            switch (this.valueType)
            {
                case ValueType.EvolutionItem:
                    return "Sprites/Items/Evolution/" + ((StoneType)this.type).ToString() + ".png";
                case ValueType.Fossil:
                    return "Sprites/Breeding/" + this.itemName + ".png";
                default:
                    return "Sprites/Items/Underground/" + this.itemName + ".png";
            }
        }
    }

    public Color GetItemColor()
    {
        return GetTypeColor(this.valueType);
    }
    #endregion

    public static Color GetTypeColor(ValueType type)
    {
        switch (type)
        {
            case ValueType.Diamond:
                return new Color(0, 160f / 255f, 200f / 255f);  // ~blue
            case ValueType.EvolutionItem:
                return new Color(200f / 255f, 0, 0);            // ~red
            case ValueType.Fossil:
            case ValueType.FossilPiece:
                return new Color(126f / 255f, 70f / 255f, 0);   // ~brown
            case ValueType.Gem:
                return new Color(0, 110f / 255f, 0);            // ~green
            case ValueType.Shard:
                return new Color(200f / 255f, 0, 200f / 255f);  // ~pink
            default:
                return Color.black;
        }
    }

        #region Enums for underground items
    public enum ValueType
    {
        Diamond,
        Gem,
        Shard,
        Fossil,
        FossilPiece,
        EvolutionItem,
    }

    public enum StoneType
    {
        None = -1,
        Leaf_stone,
        Fire_stone,
        Water_stone,
        Thunder_stone,
        Moon_stone,
        Linking_cord,
        Sun_stone,
        Soothe_bell,
        Metal_coat,
        Kings_rock,
        Upgrade,
        Dragon_scale,
        Prism_scale,
        Deepsea_tooth,
        Deepsea_scale,
        Shiny_stone,
        Dusk_stone,
        Dawn_stone,
        Razor_claw,
        Razor_fang,
        Electirizer,
        Magmarizer,
        Protector,
        Dubious_disc,
        Reaper_cloth,
        Black_DNA,
        White_DNA,
        Sachet,
        Whipped_dream,
        Ice_stone,
        Sweet_apple,
        Tart_apple,
        Cracked_pot,
        Galarica_cuff,
        Galarica_wreath,
    }

    public enum ShardType
    {
        None = -1,
        Red_Shard,
        Yellow_Shard,
        Green_Shard,
        Blue_Shard,
        Grey_Shard,
        Purple_Shard,
        Ochre_Shard,
        Black_Shard,
        Crimson_Shard,
        Lime_Shard,
        White_Shard,
        Pink_Shard,
        Cyan_Shard,
    }

    public enum FossilPieceType
    {
        None = -1,
        Fossilized_Bird,
        Fossilized_Fish,
        Fossilized_Drake,
        Fossilized_Dino,
    }
    #endregion

}


public class UndergroundGemItem : UndergroundItem
{
    public UndergroundGemItem(string p_name, int p_id, int p_val, PokemonType p_type, Requirement p_requirement = null)
        : base(p_name, p_id, p_val, ValueType.Gem, p_requirement)
    {
        this.type = (int)p_type;
    }
}

public class UndergroundEvolutionItem : UndergroundItem
{
    public UndergroundEvolutionItem(string p_name, int p_id, int p_val, StoneType p_type, Requirement p_requirement = null)
        : base(p_name, p_id, p_val, ValueType.EvolutionItem, p_requirement)
    {
        this.type = (int)p_type;
    }
}
