using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickPanel : MonoBehaviour
{
    public GameObject TypeBlocPrefab;
    public Transform TypeBlocParent;

    private DailyDealsGenerator _parent;

    void Start()
    {
        this._parent = this.GetComponentInParent<DailyDealsGenerator>();
        initItemList();
    }
    void initItemList()
    {
        foreach (TypeBloc_Pick bloc in TypeBlocParent.GetComponentsInChildren<TypeBloc_Pick>()) { GameObject.Destroy(bloc.gameObject); }

        foreach (UndergroundItem.ValueType type in System.Enum.GetValues(typeof(UndergroundItem.ValueType)))
        {
            GameObject bloc = GameObject.Instantiate(TypeBlocPrefab, TypeBlocParent);
            bloc.GetComponent<TypeBloc_Pick>().Init(type, this);
        }
    }

    public void OnPick(int itemId)
    {
        this._parent.OnPickItem(itemId);
    }
}
