using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace PCC.Datastructures.CSharp.WpfForm;

public class SyncComplexCollection<T> : ISyncComplexCollection<T>
{
    private List<T> _elements;
    private ObservableCollection<T> _unsavedElements = new ObservableCollection<T>();

    public SyncComplexCollection() : this(new List<T>())
    {
        
    }
    public SyncComplexCollection(IEnumerable<T> source)
    {
        _elements = new List<T>(source);
        _unsavedElements = new ObservableCollection<T>(source);
    }

    public event NotifyCollectionChangedEventHandler? CollectionChanged;
    public event PropertyChangedEventHandler? PropertyChanged;
    public bool IsDirty { get
    {
        if (_unsavedElements.Count != _elements.Count()) return true;
        for (int i = 0; i < _unsavedElements.Count; i++)
            if (!ReferenceEquals(_unsavedElements[i], _elements[i])) return true;
        return false;
    }}
    public void AcceptChanges()
    {
        _elements = new List<T>(_unsavedElements);
    }

    public void RejectChanges()
    {
        _unsavedElements = new ObservableCollection<T>(_elements);
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
        throw new System.NotImplementedException();
    }

    public bool Contains(T item)
    {
        throw new System.NotImplementedException();
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        throw new System.NotImplementedException();
    }

    public bool Remove(T item)
    {
        return _unsavedElements.Remove(item);
    }

    public int Count { get; }
    public bool IsReadOnly { get; }
    public int IndexOf(T item)
    {
        throw new System.NotImplementedException();
    }

    public void Insert(int index, T item)
    {
        throw new System.NotImplementedException();
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