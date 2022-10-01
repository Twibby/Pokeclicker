using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemButton : MonoBehaviour
{
    public TMPro.TMP_Text ItemLabel;

    private TypeBloc_Pick _parent;
    private UndergroundItem _item;
    public UndergroundItem Item { get { return _item; } }

    public void Init(UndergroundItem item, TypeBloc_Pick parent)
    {
        this._item = item;
        this._parent = parent;

        this.ItemLabel.text = item.DisplayName;
    }

    public void OnClick()
    {
        _parent.OnPick(_item.Id);
    }
}
