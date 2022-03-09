using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace PCC.Datastructures.CSharp.WpfForm;

public class SyncComplexCollection<T> : ISyncComplexCollection<T> where T : IComplexProperty
{
    private List<T> _confirmedElements;
    private ObservableCollection<T> _currentElements;

    public SyncComplexCollection() : this(new List<T>())
    {
        
    }
    public SyncComplexCollection(IEnumerable<T> source)
    {
        _confirmedElements = new List<T>(source);
        UpdateUnsavedElements(source);
    }

    private void UpdateUnsavedElements(IEnumerable<T> source)
    {
        if (_currentElements != null)
            _currentElements.CollectionChanged -= OnCollectionChanged;
        _currentElements = new ObservableCollection<T>(source);
        _currentElements.CollectionChanged += OnCollectionChanged;
    }

    private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        CollectionChanged?.Invoke(this, e);
        OnPropertyChanged(nameof(IsDirty));
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
            if (!ReferenceEquals(_currentElements[i], _confirmedElements[i])) return true;
        return false;
    }}
    public void AcceptChanges()
    {
        _confirmedElements = new List<T>(_currentElements);
    }

    public void RejectChanges()
    {
        UpdateUnsavedElements(_confirmedElements);
        OnCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public IEnumerator<T> GetEnumerator()
    {
        throw new System.NotImplementedException();
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
    public bool IsReadOnly { get; }
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