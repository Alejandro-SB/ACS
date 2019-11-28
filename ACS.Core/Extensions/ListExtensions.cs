using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ACS.Core.Extensions
{
    public static class ListExtensions
    {
        public static bool RemoveFirst<T>(this List<T> list, Func<T, bool> predicate)
        {
            var item = list.FirstOrDefault(predicate);

            if(item != null)
            {
                return list.Remove(item);
            }
            else
            {
                return false;
            }
        }
    }
}
