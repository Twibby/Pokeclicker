using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TypeBloc_Pick : MonoBehaviour
{
    public TMPro.TMP_Text TypeLabel;
    public Image TypeColorHolder;

    public GameObject ItemLinePrefab;
    public Transform ItemsParent;

    private PickPanel _parent;
        
    public void Init(UndergroundItem.ValueType type, PickPanel parent)
    {
        this.TypeLabel.text = type.ToString();
        this.TypeColorHolder.color = UndergroundItem.GetTypeColor(type);

        List<UndergroundItem> myItems = new List<UndergroundItem>(UndergroundItemsManager.Singleton.Items.FindAll(x => x.valueType == type));

        foreach (ItemButton line in ItemsParent.GetComponentsInChildren<ItemButton>()) { GameObject.Destroy(line.gameObject); }

        foreach (UndergroundItem item in myItems)
        {
            GameObject line = GameObject.Instantiate(ItemLinePrefab, ItemsParent);
            line.GetComponent<ItemButton>().Init(item, this);
        }

        this._parent = parent;
    }

    public void OnPick(int id) 
    {
        this._parent.OnPick(id);
    }
}
