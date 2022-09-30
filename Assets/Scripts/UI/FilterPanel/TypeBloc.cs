using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TypeBloc : MonoBehaviour
{
    public Toggle TypeToggle;
    public TMPro.TMP_Text TypeLabel;
    public Image TypeColorHolder;

    public GameObject ItemLinePrefab;
    public Transform ItemsParent;

    public GameObject ShortcutButtonsPanel;
    
    private List<ItemLine> _lines = new List<ItemLine>();
    
    public void Init(UndergroundItem.ValueType type)
    {
        this.TypeLabel.text = type.ToString();
        this.TypeColorHolder.color = UndergroundItem.GetTypeColor(type);

        List<UndergroundItem> myItems = new List<UndergroundItem>(UndergroundItemsManager.Singleton.Items.FindAll(x => x.valueType == type));

        foreach (ItemLine line in ItemsParent.GetComponentsInChildren<ItemLine>()) { GameObject.Destroy(line.gameObject); }
        _lines = new List<ItemLine>();

        foreach (UndergroundItem item in myItems)
        {
            GameObject line = GameObject.Instantiate(ItemLinePrefab, ItemsParent);
            line.GetComponent<ItemLine>().Init(item);

            _lines.Add(line.GetComponent<ItemLine>());
        }

        this.TypeToggle.onValueChanged.AddListener(ItemLinesActivation);
    }

    public void Reset(List<UndergroundItem> filters)
    {
        if (filters == null || filters.Count == 0)
        {
            this.TypeToggle.isOn = true;
            foreach (ItemLine line in _lines) { line.isOn = true; }
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

    public List<UndergroundItem> OnApply()
    {
        if (!this.TypeToggle.isOn)
            return new List<UndergroundItem>();

        return _lines.Where(x => x.isOn).Select(x => x.Item).ToList();
    }

    public void OnReset()
    {
        this.TypeToggle.isOn = false;
        this.TypeToggle.isOn = true;
        _lines.ForEach(x => x.isOn = true);
    }

    public void OnSelectAll()   { _lines.ForEach(x => x.isOn = true);  }
    public void OnSelectNone()  { _lines.ForEach(x => x.isOn = false); }

}
