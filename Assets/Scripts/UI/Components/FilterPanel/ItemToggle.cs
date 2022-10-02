using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ItemToggle : ItemObject
{
    private Toggle _myToggle;
    public override bool isOn
    {
        get { return this._myToggle.isOn; }
        set { this._myToggle.isOn = value; }
    }

    public override void Init(UndergroundItem item)
    {
        base.Init(item);

        this._myToggle = this.GetComponent<Toggle>();
    }
}
