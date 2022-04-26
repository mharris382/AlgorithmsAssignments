using System.Collections.Generic;

namespace Assignment2
{
    public class DoubleKeyDictionary<TKey1, TKey2, TValue>
    {
        private Dictionary<TKey1, Dictionary<TKey2, TValue>> _nestedDictionary = new Dictionary<TKey1, Dictionary<TKey2, TValue>>();
        public TValue this[(TKey1 key1, TKey2 key2) keys]
        {
            get => _nestedDictionary[keys.key1][keys.key2];
            set => _nestedDictionary[keys.key1][keys.key2]=value;
        }

        public void Add(TKey1 key1, TKey2 key2, TValue value)
        {
            _nestedDictionary.Add(key1, new Dictionary<TKey2, TValue>());
            _nestedDictionary[key1].Add(key2, value);
            
            // if(_reverseLookup.ContainsKey(key2))
            //     _reverseLookup.Add(key2, new HashSet<TKey1>());
            //
            // _reverseLookup[key2].Add(key1);
        }

        public void Remove(TKey1 key1, TKey2 key2)
        {
            if (ContainsKeyPair(key1, key2))
            {
                // _reverseLookup[key2].Remove(key1);
                _nestedDictionary[key1].Remove(key2);
                if (_nestedDictionary[key1].Count == 0) 
                    _nestedDictionary.Remove(key1);
            }
        }
        
        public bool ContainsKeyPair(TKey1 key1, TKey2 key2)
        {
            return _nestedDictionary.ContainsKey(key1) && _nestedDictionary[key1].ContainsKey(key2);
        }
    }
}