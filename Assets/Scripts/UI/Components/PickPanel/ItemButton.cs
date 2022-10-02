using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemButton : ItemObject
{
    private TypeBloc_Pick _parent;

    public void Init(UndergroundItem item, TypeBloc_Pick parent)
    {
        base.Init(item);
        this._parent = parent;
    }

    public void OnClick()
    {
        _parent.OnPick(_item.Id);
    }
}
