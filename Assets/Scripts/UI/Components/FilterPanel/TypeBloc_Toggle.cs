using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TypeBloc_Toggle : TypeBloc
{
    public TMPro.TMP_Text CountLabel;

    private int _selectedItemsCount = 0;
        
    public override void Init(UndergroundItem.ValueType type)
    {
        this.TypeLabel.text = type.ToString();
        this.TypeColorHolder.color = UndergroundItem.GetTypeColor(type);

        List<UndergroundItem> myItems = new List<UndergroundItem>(UndergroundItemsManager.Singleton.Items.FindAll(x => x.valueType == type));

        foreach (ItemToggle line in ItemsParent.GetComponentsInChildren<ItemToggle>()) { GameObject.Destroy(line.gameObject); }

        foreach (UndergroundItem item in myItems)
        {
            GameObject itemGO = GameObject.Instantiate(ItemPrefab, ItemsParent);
            itemGO.GetComponent<ItemToggle>().Init(item);
            itemGO.GetComponent<Toggle>().onValueChanged.AddListener(OnItemToggleClick);

            _items.Add(itemGO.GetComponent<ItemToggle>());
        }

        refreshCountLabel();
    }

    public void OnItemToggleClick(bool isOn)
    {
        refreshCountLabel();
    }

    void refreshCountLabel()
    { 
        _selectedItemsCount = _items.Where(x => x.isOn).Count();
        if (_selectedItemsCount == 0)
            CountLabel.text = "";
        else
            CountLabel.text = "(" + _selectedItemsCount + ")";
    }

    public override void Set(List<UndergroundItem> filters, bool setOnIfEmpty = true)
    {
        Debug.LogWarning("Setting with " + filters.Count + " filters and setonifempty is : " + setOnIfEmpty.ToString());
        if (filters == null || filters.Count == 0)
        {
            foreach (ItemToggle item in _items) { item.isOn = setOnIfEmpty; }
        }
        else
        {
            foreach (ItemToggle item in _items)
            {
                item.isOn = filters.Exists(x => x.Id == item.Item.Id);
            }
        }

        refreshCountLabel();
    }

    public override void OnReset(bool resetMustSetOn = true)
    {
        _items.ForEach(x => x.isOn = resetMustSetOn);
    }

    public override List<UndergroundItem> OnApply()
    {
        return base.OnApply();
    }

    public void OnSelectAll() { _items.ForEach(x => x.isOn = true); }
    public void OnSelectNone() { _items.ForEach(x => x.isOn = false); }
}
