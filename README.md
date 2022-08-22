# AdvancedViewModels

Helper types to reduce boilerplate code for WPF MVVM applications, inspired by Thomas Claudius Huber's course "WPF and MVVM: Advanced Model Treatment"

- INotifyPropertyChanged
- INotifyCollectionChanged
- Error Validation
- Change tracking

BaseViewModel Example:

```

public class SampleViewModel : BaseViewModel
{
    public SampleViewModel(SampleBackingObject wrapped) : base(wrapped)
    {
        SomeComplex = new SampleViewModel(wrapped.SomeComplex);
    }
    
    public int SomeInteger
    {
        get => GetProperty<int>();
        // fires INotifyPropertyChanged event when set
        set => SetProperty(value);
    }
    
    public SampleViewModel SomeComplex
    {
        get => GetComplexProperty<SampleViewModel>();
        // fires INotifyPropertyChanged event when set
        // listens for INotifyPropertyChanged events from Complex Property
        set => SetComplexProperty(value);
    }
    
    public SyncComplexCollection<SampleViewModel> ComplexCollection
    {
        get => GetComplexProperty<SyncComplexCollection<SampleViewModel>>();
        // fires INotifyPropertyChanged event when set
        // listens for INotifyPropertyChanged events from Complex Collection
        // listens for CollectionChanged events from Complex Collection
        set => SetComplexProperty(value);
    }
    
    // IsDirty { get; } == true when changes exist
    
    // AcceptChanges() accept changes, IsDirty false after operation
    
    // RejectChanges() revert changes, IsDirty false after operation
    
    // HasErrors { get; }
    
    // ErrorsChanged Event
}

```
