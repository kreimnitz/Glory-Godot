using System;
using System.Collections.Generic;
using System.Linq;

public interface IUpdateFrom<T>
{
    public Guid Id { get; }
    public void UpdateFrom(T other);
}

public static class UpdateUtilites
{
    public static ListUpdateInfo<T> UpdateMany<T>(List<T> items, List<T> otherItems) where T : IUpdateFrom<T>
    {
        var dict = items.ToDictionary(i => i.Id);
        var otherDict = otherItems.ToDictionary(i => i.Id);
        var updateInfo = UpdateMany(dict, otherDict);
        foreach (var added in updateInfo.Added)
        {
            items.Add(added);
        }
        foreach (var removed in updateInfo.Removed)
        {
            items.Remove(removed);
        }
        return updateInfo;
    }

    public static ListUpdateInfo<T> UpdateMany<T>(Dictionary<Guid, T> items, Dictionary<Guid, T> otherItems) where T : IUpdateFrom<T>
    {
        var updateInfo = new ListUpdateInfo<T>();
        foreach (var otherKvp in otherItems)
        {
            if (items.TryGetValue(otherKvp.Key, out T matchingItem))
            {
                matchingItem.UpdateFrom(otherKvp.Value);
                updateInfo.Updated.Add(matchingItem);
            }
            else
            {
                updateInfo.Added.Add(otherKvp.Value);
                items.Add(otherKvp.Key, otherKvp.Value);
            }
        }

        var otherIds = otherItems.Keys.ToHashSet();
        foreach (var item in items.Values.ToList())
        {
            if (otherIds.Contains(item.Id))
            {
                continue;
            }
            updateInfo.Removed.Add(item);
            items.Remove(item.Id);
        }
        return updateInfo;
    }
}

public class ListUpdateInfo<T> where T : IUpdateFrom<T>
{
    public List<T> Added { get; } = new();
    public List<T> Removed { get; } = new();
    public List<T> Updated { get; } = new();
}