using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace PCC.Datastructures.CSharp.WpfForm;

public class SyncComplexCollection<T> : ISyncComplexCollection<T>
{
    private List<T> _confirmedElements;
    private ObservableCollection<T> _unsavedElements;

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
        if (_unsavedElements != null)
            _unsavedElements.CollectionChanged -= OnCollectionChanged;
        _unsavedElements = new ObservableCollection<T>(source);
        _unsavedElements.CollectionChanged += OnCollectionChanged;
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
        if (_unsavedElements.Count != _confirmedElements.Count()) return true;
        for (int i = 0; i < _unsavedElements.Count; i++)
            if (!ReferenceEquals(_unsavedElements[i], _confirmedElements[i])) return true;
        return false;
    }}
    public void AcceptChanges()
    {
        _confirmedElements = new List<T>(_unsavedElements);
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
        _unsavedElements.Add(item);
    }

    public void Clear()
    {
        _unsavedElements.Clear();
    }

    public bool Contains(T item)
    {
        return _unsavedElements.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        _unsavedElements.CopyTo(array, arrayIndex);
    }

    public bool Remove(T item)
    {
        return _unsavedElements.Remove(item);
    }

    public int Count => _unsavedElements.Count;
    public bool IsReadOnly { get; }
    public int IndexOf(T item)
    {
        return _unsavedElements.IndexOf(item);
    }

    public void Insert(int index, T item)
    {
        _unsavedElements.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        _unsavedElements.RemoveAt(index);
    }

    public T this[int index]
    {
        get => _unsavedElements[index];
        set => _unsavedElements[index] = value;
    }
}