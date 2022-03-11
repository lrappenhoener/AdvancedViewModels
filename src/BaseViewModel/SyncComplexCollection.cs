using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace PCC.Datastructures.CSharp.BaseViewModel;

public class SyncComplexCollection<T> : ISyncComplexCollection<T> where T : IComplexProperty
{
    private List<T> _confirmedElements;
    private ObservableCollection<T> _currentElements = new ObservableCollection<T>();

    public SyncComplexCollection() : this(new List<T>())
    {
        
    }
    public SyncComplexCollection(List<T> source)
    {
        _confirmedElements = new List<T>(source);
        UpdateCurrentElements(source);
    }

    private void UpdateCurrentElements(IEnumerable<T> source)
    {
        _currentElements.CollectionChanged -= OnCollectionChanged;
        _currentElements = new ObservableCollection<T>(source);
        _currentElements.CollectionChanged += OnCollectionChanged;
        foreach (var currentElement in _currentElements)
            currentElement.PropertyChanged += OnElementPropertyChanged;
    }

    private void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(IsDirty))
            OnPropertyChanged(nameof(IsDirty));
    }

    private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        var oldComplexProperties =
            e.OldItems != null ? e.OldItems.OfType<IComplexProperty>() : new List<IComplexProperty>();
        Unhook(oldComplexProperties);

        var newComplexProperties =
            e.NewItems != null ? e.NewItems.OfType<IComplexProperty>() : new List<IComplexProperty>();
        Hook(newComplexProperties);
        
        CollectionChanged?.Invoke(this, e);
        OnPropertyChanged(nameof(IsDirty));
    }

    private void Unhook(IEnumerable<IComplexProperty> complexProperties)
    {
        foreach (var complexProperty in complexProperties)
            complexProperty.PropertyChanged -= OnElementPropertyChanged;
    }
    
    private void Hook(IEnumerable<IComplexProperty> complexProperties)
    {
        foreach (var complexProperty in complexProperties)
            complexProperty.PropertyChanged += OnElementPropertyChanged;
    }

    private void OnPropertyChanged(string propertyName)
    {
        var args = new PropertyChangedEventArgs(propertyName);
        PropertyChanged?.Invoke(this, args);        
    }

    public event NotifyCollectionChangedEventHandler? CollectionChanged;
    public event PropertyChangedEventHandler? PropertyChanged;
    public bool IsDirty { get
    {
        if (_currentElements.Count != _confirmedElements.Count()) return true;
        for (int i = 0; i < _currentElements.Count; i++)
            if (!ReferenceEquals(_currentElements[i], _confirmedElements[i]) || _currentElements[i].IsDirty) 
                return true;
        return false;
    }}
    public void AcceptChanges()
    {
        foreach (var dirtyElement in _currentElements.Where(e => e.IsDirty))
            dirtyElement.AcceptChanges();
        _confirmedElements = new List<T>(_currentElements);
    }

    public void RejectChanges()
    {
        foreach (var currentElement in _currentElements)
            currentElement.PropertyChanged -= OnElementPropertyChanged;
        foreach (var dirtyElement in _confirmedElements.Where(e => e.IsDirty))
            dirtyElement.RejectChanges();
        UpdateCurrentElements(_confirmedElements);
        OnCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _currentElements.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(T item)
    {
        _currentElements.Add(item);
    }

    public void Clear()
    {
        _currentElements.Clear();
    }

    public bool Contains(T item)
    {
        return _currentElements.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        _currentElements.CopyTo(array, arrayIndex);
    }

    public bool Remove(T item)
    {
        return _currentElements.Remove(item);
    }

    public int Count => _currentElements.Count;
    public bool IsReadOnly => false;
    public int IndexOf(T item)
    {
        return _currentElements.IndexOf(item);
    }

    public void Insert(int index, T item)
    {
        _currentElements.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        _currentElements.RemoveAt(index);
    }

    public T this[int index]
    {
        get => _currentElements[index];
        set => _currentElements[index] = value;
    }
}