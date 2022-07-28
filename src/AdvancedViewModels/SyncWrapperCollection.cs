using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

namespace PCC.Libraries.AdvancedViewModels;

public class SyncWrapperCollection<TWrapper, TModel> : ISyncComplexCollection<TWrapper> where TWrapper : IComplexProperty<TModel>
{
    private readonly List<TModel> _models;
    private readonly ISyncComplexCollection<TWrapper> _syncComplexCollection;

    public SyncWrapperCollection(List<TModel> models, List<TWrapper> wrappers)
    {
        _models = models;
        _syncComplexCollection = new SyncComplexCollection<TWrapper>(wrappers);
    }

    #region ISyncComplexCollection<TWrapper> Members

    public IEnumerator<TWrapper> GetEnumerator()
    {
        return _syncComplexCollection.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(TWrapper item)
    {
        _syncComplexCollection.Add(item);
    }

    public void Clear()
    {
        _syncComplexCollection.Clear();
    }

    public bool Contains(TWrapper item)
    {
        return _syncComplexCollection.Contains(item);
    }

    public void CopyTo(TWrapper[] array, int arrayIndex)
    {
        _syncComplexCollection.CopyTo(array, arrayIndex);
    }

    public bool Remove(TWrapper item)
    {
        return _syncComplexCollection.Remove(item);
    }

    public int Count => _syncComplexCollection.Count;
    public bool IsReadOnly => _syncComplexCollection.IsReadOnly;

    public int IndexOf(TWrapper item)
    {
        return _syncComplexCollection.IndexOf(item);
    }

    public void Insert(int index, TWrapper item)
    {
        _syncComplexCollection.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        _syncComplexCollection.RemoveAt(index);
    }

    public TWrapper this[int index]
    {
        get => _syncComplexCollection[index];

        set => _syncComplexCollection[index] = value;
    }

    public event NotifyCollectionChangedEventHandler? CollectionChanged
    {
        add => _syncComplexCollection.CollectionChanged += value;

        remove => _syncComplexCollection.CollectionChanged -= value;
    }

    public event PropertyChangedEventHandler? PropertyChanged
    {
        add => _syncComplexCollection.PropertyChanged += value;
        remove => _syncComplexCollection.PropertyChanged -= value;
    }

    public bool IsDirty => _syncComplexCollection.IsDirty;

    public void AcceptChanges()
    {
        _syncComplexCollection.AcceptChanges();
        SyncModels();
    }

    public void RejectChanges()
    {
        _syncComplexCollection.RejectChanges();
    }

    public IEnumerable GetErrors(string? propertyName)
    {
        return _syncComplexCollection.GetErrors(propertyName);
    }

    public bool HasErrors => _syncComplexCollection.HasErrors;

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged
    {
        add => _syncComplexCollection.ErrorsChanged += value;
        remove => _syncComplexCollection.ErrorsChanged -= value;
    }

    public bool CanSave => _syncComplexCollection.CanSave;

    #endregion

    private void SyncModels()
    {
        _models.Clear();
        _models.AddRange(_syncComplexCollection.Select(w => w.Model).ToList());
    }
}