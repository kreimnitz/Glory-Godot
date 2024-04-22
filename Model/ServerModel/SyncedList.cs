using System;
using System.Collections;
using System.Collections.Generic;

public class SyncedList<T, U> : IEnumerable<T>
{
    private List<T> _items = new();
    private List<U> _syncedList;
    private Func<T, U> _map;

    public SyncedList(List<U> syncedList, Func<T, U> map)
    {
        _syncedList = syncedList;
        _map = map;
    }

    public int Count => _items.Count;

    public void Add(T item)
    {
        _items.Add(item);
        _syncedList.Add(_map(item));
    }

    public void RemoveAt(int index)
    {
        _items.RemoveAt(index);
        _syncedList.RemoveAt(index);
    }

    public void Remove(T item)
    {
        _items.Remove(item);
        _syncedList.Remove(_map(item));
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    public T this[int index]
    { 
        get => _items[index];
        set => _items[index] = value;
    }
}