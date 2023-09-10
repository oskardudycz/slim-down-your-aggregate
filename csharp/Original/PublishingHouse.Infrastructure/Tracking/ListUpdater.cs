namespace PublishingHouse.Core.Tracking;

public static class ListUpdater
{
    public static void Update<TLTo, TLFrom>(
        this List<TLTo> to,
        List<TLFrom> from,
        Func<TLFrom, Func<TLTo, bool>> matches,
        Action<TLTo, TLFrom> onUpdate,
        Func<TLFrom, TLTo> onAdd
    )
    {
        var toUpdate = from.Where(f => to.Any(matches(f))).ToList();
        var toAdd = from.Except(toUpdate);

        foreach (var updated in toUpdate)
        {
            var current = to.Single(matches(updated));
            onUpdate(current, updated);
        }

        to.AddRange(toAdd.Select(onAdd));
    }
}
