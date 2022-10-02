using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TypeBloc : MonoBehaviour
{
    public TMPro.TMP_Text TypeLabel;
    public Image TypeColorHolder;

    public GameObject ItemPrefab;
    public Transform ItemsParent;

    public List<ItemObject> _items = new List<ItemObject>();

    public virtual void Init(UndergroundItem.ValueType type) { Debug.LogWarning("Function not implemented yet"); }

    public virtual void Set(List<UndergroundItem> filters, bool setOnIfEmpty = true) { Debug.LogWarning("Function not implemented yet"); }

    public virtual List<UndergroundItem> OnApply() 
    {
        return _items.Where(x => x.isOn).Select(x => x.Item).ToList();
    }

    public virtual void OnReset(bool resetMustSetOn = true) { Debug.LogWarning("Function not implemented yet"); }


}
