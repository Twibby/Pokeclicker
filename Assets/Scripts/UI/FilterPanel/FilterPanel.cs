using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterPanel : MonoBehaviour
{
    public GameObject TypeBlocPrefab;
    public Transform TypeBlocParent;

    private DailyDealsGenerator _parent;
    private List<TypeBloc> _blocs = new List<TypeBloc>();

    void Start()
    {
        this._parent = this.GetComponentInParent<DailyDealsGenerator>();
        initItemList();
    }

    void initItemList()
    {
        foreach (TypeBloc bloc in TypeBlocParent.GetComponentsInChildren<TypeBloc>()) { GameObject.Destroy(bloc.gameObject); }
        _blocs = new List<TypeBloc>();

        foreach (UndergroundItem.ValueType type in System.Enum.GetValues(typeof(UndergroundItem.ValueType)))
        {
            GameObject bloc = GameObject.Instantiate(TypeBlocPrefab, TypeBlocParent);
            bloc.GetComponent<TypeBloc>().Init(type);

            _blocs.Add(bloc.GetComponent<TypeBloc>());
        }
    }

    public void Reset(List<UndergroundItem> filtersList)
    {
        foreach (TypeBloc bloc in _blocs)
        {
            bloc.Reset(filtersList);
        }
    }

    #region UI Functions
    public void OnCancel()
    {
        this.gameObject.SetActive(false);
    }

    public void OnApply()
    {
        List<UndergroundItem> filters = new List<UndergroundItem>();
        _blocs.ForEach(x => filters.AddRange(x.OnApply()));

        this._parent.ApplyFilters(filters);

        this.gameObject.SetActive(false);
    }
    #endregion
}
