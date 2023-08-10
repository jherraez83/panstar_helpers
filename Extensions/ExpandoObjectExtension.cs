using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace panstar_helpers.Extensions
{
    public static class ExpandoObjectExtension
    {
        public static void AddProperties(this System.Dynamic.ExpandoObject item, IEnumerable<string> keys, IEnumerable<object> values)
        {
            IEnumerator<object> _ieValues = values.GetEnumerator();

            var dictionary = (IDictionary<string, object>)item;

            foreach ( var key in keys)
            {
                _ieValues.MoveNext();
                dictionary.Add(key, _ieValues.Current);
            }
        }
    }
}


