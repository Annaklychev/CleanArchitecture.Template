using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Domain.Extensions;

public static class CollectionExtensions
{
    extension<T>(IEnumerable<T> collection)
    {
        public bool IsEmpty() => !collection.Any();

        public bool AllMatch(Func<T, bool> predicate) =>
            collection.All(predicate);
    }

    public static T? FindById<T>(this IEnumerable<T> collection, Guid id) where T : BaseEntity =>
        collection.FirstOrDefault(x => x.Id == id);
}