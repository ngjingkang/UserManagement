using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Extensions
{
    public static class DataExtension
    {
        public static bool HasData<TSource>(this IEnumerable<TSource>? source)
            => source is not null && source.Any();
    }
}
