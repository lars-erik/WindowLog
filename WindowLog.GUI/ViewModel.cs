using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using WindowLog.Core;

namespace WindowLog.GUI;

public class ViewModel : INotifyPropertyChanged
{
    public ObservableCollection<EntryModel> Entries { get; } = new ObservableCollection<EntryModel>();

    private EntryModel current = new EntryModel();

    public EntryModel Current
    {
        get => current;
        set
        {
            if (current != value)
            {
                current = value;
                OnPropertyChanged(nameof(Current));
                OnPropertyChanged(nameof(CurrentDescription));
            }
        }
    }

    public ViewModel()
    {
    }

    public string CurrentDescription
    {
        get
        {
            return Current.Entry == null
                ? "N/A"
                : Current.Entry.ToString();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class EntryModel : INotifyPropertyChanged
{
    private Entry? entry;

    public Entry? Entry
    {
        get => entry;
        set
        {
            if (entry != value)
            {
                entry = value;
                OnPropertyChanged(nameof(Entry));
            }
        }
    }

    public DateTime? End => Entry?.End;
    public TimeSpan? Duration => Entry?.Duration;

    public EntryModel()
    {
    }

    public EntryModel(Entry entry)
    {
        this.entry = entry;
    }

    public void NotifyChange()
    {
        OnPropertyChanged(nameof(Entry));
        OnPropertyChanged(nameof(End));
        OnPropertyChanged(nameof(Duration));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}