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

    private EntryModel current;

    public EntryModel Current
    {
        get { return current; }
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
        get => Current == null ? "N/A" : "[" + Current.Entry.PID + "] " + Current.Entry.Title + "(" + Current.Entry.Executable.Substring(Math.Max(0,
            Current.Entry.Executable.LastIndexOf("\\", StringComparison.CurrentCultureIgnoreCase))) + ")";
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class EntryModel : INotifyPropertyChanged
{
    private Entry entry;

    public Entry Entry
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

    public void NotifyChange()
    {
        OnPropertyChanged(nameof(Entry));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}