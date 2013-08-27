using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

namespace System.Dynamic.Extensions
{
    public class ExpandoObjectComparer : IEqualityComparer<object>
    {
        private ExpandoObjectComparer() { }

        public static ExpandoObjectComparer Default()
        {
            return new ExpandoObjectComparer();
        }


        public bool Equals(object x, object y)
        {

            if (object.ReferenceEquals(x, y)) return true;

            if (x.GetType().Equals(y.GetType()) && x.GetType().Equals(typeof(ExpandoObject)))
            {
                var xKeyValues = new Dictionary<string, object>(x as IDictionary<string, object>);
                var yKeyValues = new Dictionary<string, object>(y as IDictionary<string, object>);

                var xFieldsCount = xKeyValues.Count();
                var yFieldsCount = yKeyValues.Count();

                if (xFieldsCount != yFieldsCount) return false;
                var missingKey = xKeyValues.Keys.Where(k => !yKeyValues.ContainsKey(k)).FirstOrDefault();
                if (missingKey != null) return false;

                foreach (var keyValue in xKeyValues)
                {

                    var key = keyValue.Key;
                    var xValueItem = keyValue.Value;
                    var yValueItem = yKeyValues[key];

                    if (xValueItem == null & yValueItem != null) return false;
                    if (xValueItem != null & yValueItem == null) return false;

                    if (xValueItem != null & yValueItem != null)
                    {
                        if (!xValueItem.Equals(yValueItem)) return false;
                    }
                }
                return true;
            }
            return false;
        }


        public int GetHashCode(object obj)
        {

            int hashCode = 0;

            Func<object, int> getHash = item =>
            {
                if (item == null) return 0;
                return item.GetHashCode();
            };


            if (obj.GetType().Equals(typeof(ExpandoObject)))
            {
                unchecked
                {
                    var fieldValues = new Dictionary<string, object>(obj as IDictionary<string, object>);
                    fieldValues.Values.ToList().ForEach(v => hashCode ^= getHash(v));
                }
                return hashCode;
            }

            else
            {
                return obj.GetHashCode();
            }

        }
    }

}