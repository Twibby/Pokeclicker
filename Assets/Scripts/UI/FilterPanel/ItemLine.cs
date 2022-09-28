using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]

public class ItemLine : MonoBehaviour
{
    public UndergroundItem Item;
    public TMPro.TMP_Text ItemLabel;
    
    private Toggle _myToggle;

    public void Init(UndergroundItem item)
    {
        this._myToggle = this.GetComponent<Toggle>();

        this.Item = item;
        this.ItemLabel.text = item.DisplayName;
    }

    public bool isOn
    {
        get { return this._myToggle.isOn; }
        set { this._myToggle.isOn = value; }
    }
}
