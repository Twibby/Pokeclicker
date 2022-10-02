using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TypeBloc_Checkboxes : TypeBloc
{
    public Toggle TypeToggle;

    public GameObject ShortcutButtonsPanel;
    
    private List<ItemLine> _lines = new List<ItemLine>();
    
    public override void Init(UndergroundItem.ValueType type)
    {
        this.TypeLabel.text = type.ToString();
        this.TypeColorHolder.color = UndergroundItem.GetTypeColor(type);

        List<UndergroundItem> myItems = new List<UndergroundItem>(UndergroundItemsManager.Singleton.Items.FindAll(x => x.valueType == type));

        foreach (ItemLine line in ItemsParent.GetComponentsInChildren<ItemLine>()) { GameObject.Destroy(line.gameObject); }
        _lines = new List<ItemLine>();

        foreach (UndergroundItem item in myItems)
        {
            GameObject line = GameObject.Instantiate(ItemPrefab, ItemsParent);
            line.GetComponent<ItemLine>().Init(item);

            _lines.Add(line.GetComponent<ItemLine>());
        }

        this.TypeToggle.onValueChanged.AddListener(ItemLinesActivation);
    }

    public override void Set(List<UndergroundItem> filters, bool setOnIfEmpty = true)
    {
        if (filters == null || filters.Count == 0)
        {
            this.TypeToggle.isOn = true;
            foreach (ItemLine line in _lines) { line.isOn = setOnIfEmpty; }
        }
        else
        {
            foreach (ItemLine line in _lines)
            {
                line.isOn = filters.Exists(x => x.Id == line.Item.Id);
            }
        }
    }

    public void ItemLinesActivation(bool isOn)
    {
        ItemsParent.gameObject.SetActive(isOn);
        ShortcutButtonsPanel.SetActive(isOn);
    }

    public override List<UndergroundItem> OnApply()
    {
        if (!this.TypeToggle.isOn)
            return new List<UndergroundItem>();

        return base.OnApply();
    }

    public override void OnReset(bool resetMustSetOn = true)
    {
        this.TypeToggle.isOn = false;
        this.TypeToggle.isOn = true;
        this.TypeToggle.isOn = resetMustSetOn;
        _lines.ForEach(x => x.isOn = resetMustSetOn);
    }

    public void OnSelectAll()   { _lines.ForEach(x => x.isOn = true);  }
    public void OnSelectNone()  { _lines.ForEach(x => x.isOn = false); }

}
