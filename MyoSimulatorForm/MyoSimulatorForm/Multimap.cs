using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyoSimGUI
{
    class Multimap<T, V>
    {
        private Dictionary<T, List<V>> multimapDict;
 
        public Multimap()
        {
            multimapDict = new Dictionary<T, List<V>>();
        }

        public Multimap(Multimap<T, V> m)
        {
            multimapDict = new Dictionary<T, List<V>>(m.getUnderlyingDict());
        }

        public void Add(T key, V value)
        {
            List<V> values;

            if (multimapDict.ContainsKey(key))
            {
                values = multimapDict[key];
            }
            else
            {
                values = new List<V>();
                multimapDict.Add(key, values);
            }

            values.Add(value);
        }

        public Dictionary<T, List<V>> getUnderlyingDict()
        {
            return multimapDict;
        }

        private void Add(T key, List<V> value)
        {
            multimapDict.Add(key, value);
        }

        public List<V> GetValues(T key)
        {
            return multimapDict[key];
        }

        public List<V> this[T key]
        {
            get
            {
                return GetValues(key);
            }
            set
            {
                Add(key, value);
            }
        }
    }
}
