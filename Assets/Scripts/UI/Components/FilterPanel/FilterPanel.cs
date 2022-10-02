using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterPanel : MonoBehaviour
{
    public GameObject TypeBlocPrefab;
    public Transform TypeBlocParent;

    private DailyDealsGenerator _parent;
    private List<TypeBloc> _blocs = new List<TypeBloc>();

    public bool ResetMustSetOn = true;

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
            Debug.Log(bloc.gameObject.name);
        }
        Debug.LogWarning("*" + _blocs.Count);
        Set(new List<UndergroundItem>());
    }

    public void Set(List<UndergroundItem> filtersList)
    {
        Debug.Log("settings");
        foreach (TypeBloc bloc in _blocs)
        {
            Debug.Log(bloc.gameObject.name);
            bloc.Set(filtersList, ResetMustSetOn);
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

        if (filters.Count == 0)
        {
            UIManager.Instance.SetMessage("No item selected, you need to have at least one selected item", UIManager.MessageLevel.ERROR);
            return;
        }

        this._parent.ApplyFilters(filters);

        this.gameObject.SetActive(false);
    }

    public void OnReset()
    {
        _blocs.ForEach(x => x.OnReset(ResetMustSetOn));
    }
    #endregion
}
