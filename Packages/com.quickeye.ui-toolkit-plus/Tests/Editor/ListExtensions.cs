using System.Collections;

internal static class ListExtensions
{
    public static void Move(this IList list, int oldIndex, int newIndex)
    {
        var item = list[oldIndex];

        list.RemoveAt(oldIndex);
        list.Insert(newIndex, item);
    }
}