using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public abstract class ItemObject : MonoBehaviour
{
    protected UndergroundItem _item;
    public UndergroundItem Item {  get { return _item; } }

    public TMPro.TMP_Text ItemLabel;

    public virtual bool isOn { get; set; }

    public virtual void Init(UndergroundItem item)
    {
        this._item = item;
        this.ItemLabel.text = item.DisplayName;
    }

}
