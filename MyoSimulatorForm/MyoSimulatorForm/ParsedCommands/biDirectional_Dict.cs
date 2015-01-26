using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyoSimGUI.ParsedCommands
{
    /**
     * A bidirectional dictionary class that allows for looking up the key via
     * another dictionary that has the key value pairs reversed.
     */
    public class biDirectional_Dict<type1, type2> : IDictionary<type1, type2>
    {
        private IDictionary<type1, type2> _firstToSecond =
            new Dictionary<type1, type2>();
        private IDictionary<type2, type1> _secondToFirst =
            new Dictionary<type2, type1>();

        public biDirectional_Dict()
        {
        }

        #region Overload methods for forward Dictionary

        public type2 this[type1 key]
        {
            get { return _firstToSecond[key]; }
            set { _firstToSecond[key] = (type2)value;
                  _secondToFirst[(type2)value] = key;
                }
        }

        public ICollection<type1> Keys
        {
            get { return _firstToSecond.Keys; }
        }

        public ICollection<type2> Values
        {
            get { return _firstToSecond.Values; }
        }

        public void Add(type1 key, type2 value)
        {
            _firstToSecond.Add(key, value);
            _secondToFirst.Add(value, key);
        }

        public bool ContainsKey(type1 key)
        {
            return _firstToSecond.ContainsKey(key);
        }

        public bool Remove(type1 key)
        {
            return _firstToSecond.Remove(key);
        }

        public bool TryGetValue(type1 key, out type2 value)
        {
            return _firstToSecond.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<type1, type2> kvp)
        {
            _firstToSecond.Add(kvp);
        }

        public void Clear()
        {
            _firstToSecond.Clear();
        }

        public bool Contains(KeyValuePair<type1, type2> kvp)
        {
            return _firstToSecond.Contains(kvp);
        }

        public void CopyTo(KeyValuePair<type1, type2>[] array, int index)
        {
            _firstToSecond.CopyTo(array, index);
        }

        public bool Remove(KeyValuePair<type1, type2> kvp)
        {
            return _firstToSecond.Remove(kvp);
        }

        public int Count
        {
            get { return _firstToSecond.Count; }
        }

        public bool IsReadOnly
        {
            get { return _firstToSecond.IsReadOnly; }
        }

        public IEnumerator<KeyValuePair<type1, type2>> GetEnumerator()
        {
            return _firstToSecond.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region class for reverse lookup
        /**
         * Class to get use the reverse dictionary
         * This contains all the same methods as the biDirectional_Dict class,
         * but it gets the values from the reverse dictionary instead
         */
        public class reverse : IDictionary<type2, type1>
        {
            private biDirectional_Dict<type1, type2> _dict;

            public reverse(biDirectional_Dict<type1,type2> biDict)
            {
                _dict = biDict;   
            }

            public type1 this[type2 key]
            {
                get { return _dict._secondToFirst[key]; }
                set { _dict._firstToSecond[value] = key;
                      _dict._secondToFirst[key] = value;
                }
            }

            public ICollection<type2> Keys
            {
                get { return _dict._secondToFirst.Keys; }
            }

            public ICollection<type1> Values
            {
                get { return _dict._secondToFirst.Values; }
            }

            public void Add(type2 key, type1 value)
            {
                _dict._secondToFirst.Add(key, value);
                _dict._firstToSecond.Add(value, key);
            }

            public bool ContainsKey(type2 key)
            {
                return _dict._secondToFirst.ContainsKey(key);
            }

            public bool Remove(type2 key)
            {
                return _dict._secondToFirst.Remove(key);
            }

            public bool TryGetValue(type2 key, out type1 value)
            {
                return _dict._secondToFirst.TryGetValue(key, out value);
            }

            public void Add(KeyValuePair<type2, type1> kvp)
            {
                _dict._secondToFirst.Add(kvp);
            }

            public void Clear()
            {
                _dict._secondToFirst.Clear();
            }

            public bool Contains(KeyValuePair<type2, type1> kvp)
            {
                return _dict._secondToFirst.Contains(kvp);
            }

            public void CopyTo(KeyValuePair<type2, type1>[] array, int index)
            {
                _dict._secondToFirst.CopyTo(array, index);
            }

            public bool Remove(KeyValuePair<type2, type1> kvp)
            {
                return _dict._secondToFirst.Remove(kvp);
            }

            public int Count
            {
                get { return _dict._secondToFirst.Count; }
            }

            public bool IsReadOnly
            {
                get { return _dict._secondToFirst.IsReadOnly; }
            }

            public IEnumerator<KeyValuePair<type2, type1>> GetEnumerator()
            {
                return _dict._secondToFirst.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        } /* public class reverse */

        #endregion reverse class
    } /* public class biDirectional_Dict<type1, type2> */
} /* namespace MyoSimGUI.ParsedCommands */
