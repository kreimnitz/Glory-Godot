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
    public static UpdateInfo<T> UpdateMany<T>(List<T> items, List<T> otherItems) where T : IUpdateFrom<T>
    {
        var dict = items.ToDictionary(i => i.Id);
        var updateInfo = UpdateMany(dict, otherItems);
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

    public static UpdateInfo<T> UpdateMany<T>(Dictionary<Guid, T> items, List<T> otherItems) where T : IUpdateFrom<T>
    {
        var updateInfo = new UpdateInfo<T>();
        foreach (var otherItem in otherItems)
        {
            if (items.TryGetValue(otherItem.Id, out T matchingItem))
            {
                matchingItem.UpdateFrom(otherItem);
                updateInfo.Updated.Add(matchingItem);
            }
            else
            {
                updateInfo.Added.Add(otherItem);
                items.Add(otherItem.Id, otherItem);
            }
        }

        var otherIds = otherItems.Select(e => e.Id).ToHashSet();
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

public class UpdateInfo<T> where T : IUpdateFrom<T>
{
    public List<T> Added { get; } = new();
    public List<T> Removed { get; } = new();
    public List<T> Updated { get; } = new();
}