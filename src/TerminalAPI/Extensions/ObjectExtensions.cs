using System.Collections.Generic;
using System.Linq;

namespace TerminalAPI.Extensions;

internal static partial class ObjectExtensions
{
    internal static bool IsNull(this object obj)
        => obj is null;

    internal static bool IsNullOrEmpty(this ICollection<object> collection)
        => collection is null || !collection.Any();
}
