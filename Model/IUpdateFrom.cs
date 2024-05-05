using System;
using System.Collections.Generic;
using System.Linq;

public interface IUpdateFrom<T, U>
{
    public Guid Id { get; }
    public U UpdateFrom(T other);
}

public static class UpdateUtilites
{
    public static ListUpdateInfo<T, U> UpdateMany<T, U>(List<T> items, List<T> otherItems) where T : IUpdateFrom<T, U>
    {
        var dict = items.ToDictionary(i => i.Id);
        var otherDict = otherItems.ToDictionary(i => i.Id);
        var updateInfo = UpdateMany<T, U>(dict, otherDict);
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

    public static ListUpdateInfo<T, U> UpdateMany<T, U>(Dictionary<Guid, T> items, Dictionary<Guid, T> otherItems) where T : IUpdateFrom<T, U>
    {
        var updateInfos = new ListUpdateInfo<T, U>();
        foreach (var otherKvp in otherItems)
        {
            if (items.TryGetValue(otherKvp.Key, out T matchingItem))
            {
                var updateInfo = matchingItem.UpdateFrom(otherKvp.Value);
                updateInfos.Updated.Add(matchingItem, updateInfo);
            }
            else
            {
                updateInfos.Added.Add(otherKvp.Value);
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
            updateInfos.Removed.Add(item);
            items.Remove(item.Id);
        }
        return updateInfos;
    }

    public static HashSet<string> UpdateProperties<T>(T obj, T other, HashSet<string> propertyNames)
    {
        var type = typeof(T);
        var updated = new HashSet<string>();
        foreach (var propName in propertyNames)
        {
            var property = type.GetProperty(propName);
            var value1 = property.GetValue(obj);
            var value2 = property.GetValue(other);
            if (value1 != value2)
            {
                property.SetValue(obj, value2);
                updated.Add(propName);
            }
        }

        return updated;
    }
}

public class ListUpdateInfo<T, U> where T : IUpdateFrom<T, U>
{
    public List<T> Added { get; } = new();
    public List<T> Removed { get; } = new();
    public Dictionary<T, U> Updated { get; } = new();
}

public class DummyUpdateInfo
{
    public static DummyUpdateInfo Instance { get; } = new();
}

public class PropertyUpdateInfo : HashSet<string>
{
}
