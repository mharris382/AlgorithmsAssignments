using System;
using System.Collections.Generic;
using System.Text;

using UnityEngine;

namespace Assignment1
{
    public class BinaryHeap<T>
    {
        private (T item,int key)[] _items;
        private int _capacity;
        private int _count; //stores the number of items in the heap, not the size of the array
        private Dictionary<T, int> _indexLookup; //lookup table so the index of any given element can be found in O(1) time
        public int Capacity => _capacity;
        public int Count => _count;

        

        /// <summary>
        /// moves an element located at the specified index upwards in the heap to correctly reposition an element
        /// whose value is less than the value of its parent.
        /// This condition may result from removing an element or from changing an element’s value.
        /// This method is described on pages 60-61 of the text, and pseudocode is provided on page 61.
        /// </summary>
        /// <param name="index"></param>
        void HeapifyUp(int index)
        {
            
            int curIndex = index;
            int key = _items[curIndex].key;
            
           
            while (curIndex != 0 && key < ParentKey())
            {
                SwapCurrentWithParent();
                void SwapCurrentWithParent()
                {
                    int parentIndex = GetParentIndex(curIndex);
                    Swap(curIndex, parentIndex);
                    curIndex = parentIndex;
                }
            }
            int ParentKey() => _items[GetParentIndex(curIndex)].key;
        }
        
        /// <summary>
        /// moves an element located at the specified index downwards in the heap to correctly reposition an element whose value is
        /// greater than the value of either of its children. This condition may result from removing an element or from changing an
        /// element’s value.
        /// </summary>
        /// <param name="index"></param>
        void HeapifyDown(int index)
        {
            
            int n = _count;
            int j, lKey, rKey, lIndex, rIndex;

            lIndex =  GetLeftChild(index);
            rIndex = GetRightChild(index);
            
            
            int key = GetKey(index);
            int smallestIndex = index;
            int smallestKey = key;
            if (lIndex < _count &&  GetKey(lIndex) < smallestKey)
            {
                smallestKey = GetKey(lIndex);
                smallestIndex = lIndex;
            }
            if (rIndex < _count &&  GetKey(rIndex) < smallestKey)
            {
                smallestIndex = rIndex;
            }

            if (index != smallestIndex)
            {
                Swap(index, smallestIndex);
                HeapifyDown(smallestIndex);
            }
        }
    
        
        /// <summary>
        /// initializes an empty heap that is set up to store at most N elements.
        /// This operation takes O(N) time, as it involves initializing the array that will hold the heap
        /// </summary>
        /// <param name="maxItems"></param>
        public void StartHeap(int maxItems)
        {
            _capacity = maxItems;
            _items = new (T, int)[maxItems];
            _count = 0;
            _indexLookup = new Dictionary<T, int>();
        }

        /// <summary>
        /// inserts the item, item, with an ordering value, value, into the heap at index 0,
        /// then uses HeapifyDown to position the item so as to maintain the heap order.
        /// If the heap currently has n elements, this takes O(log n) time.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="value">ordering value</param>
        public bool Insert(T item, int key)
        {
            if (IsAtCapacity())
            {
                return false;
            }
            
            int index = _count;
            _items[index] =  (item, key);
            _indexLookup.Add(item, index);
            _count += 1;
            
            int i = index;
            while (i != 0 && GetKey(i) < GetParentKey(i))
            {
                int parent = GetParentIndex(i);
                Swap(i, parent);
                i = parent;
            }
            
            return true;
        }

        /// <summary>
        /// identifies the minimum element in the heap, which is located at index 1, but does not remove it. This takes O(1) time.
        /// </summary>
        /// <returns></returns>
        public T FindMin()
        {
            return _items[0].item;
        }

        /// <summary>
        /// deletes the element in the specified heap position by moving the item in the last array position to index,
        /// then using Heapify_Down to reposition that item. This is implemented in O(log n) time for heaps that have n elements.
        /// </summary>
        /// <param name="index"></param>
        public void Delete(int index)
        {
            PrintDictionary();
            ChangeKey(_items[index].item, int.MinValue);
            ExtractMin();
        }

        /// <summary>
        /// deletes the element item form the heap. This can be implemented as a call to Delete(Position[item]), which operates in O(log n) time for heaps
        /// that have n elements provided Position allows the index of v to be returned in O(1) time     
        /// </summary>
        /// <param name="item"></param>
        public void Delete(T item)
        {
            Delete(_indexLookup[item]);
        }
        
        /// <summary>
        /// identifies and deletes the element with the minimum key value, located at index 1, from the heap. This is a combination of the preceding two
        /// operations, and so it takes O(log n) time
        /// </summary>
        public T ExtractMin()
        {
            if (_count <= 0)
            {
                _count = 0;
                return default(T);
            }

            if (_count == 1)
            {
                _count--;
                return _items[0].item;
            }
            
            T minItem = _items[0].item;
            
            
            _items[0] = _items[_count - 1];
            _count--;
            HeapifyDown(0);
            
            return minItem;
        }

        /// <summary>
        /// which changes the key value of element v to newValue. To implement this operation in O(log n) time, we first need to be able to identify the position
        /// of element v in the array, which we do by using the Position structure. Once we have identified the position of element v, we change the key and
        /// then apply Heapify-up or Heapify-down as appropriate
        /// </summary>
        /// <param name="item"></param>
        /// <param name="key"></param>
        public void ChangeKey(T item, int key)
        {
           
            if (!_indexLookup.ContainsKey(item))
                throw new NodeNotFoundException($"the item: ({item}) was not found in the heap");
            var curIndex = _indexLookup[item];
            Debug.Log($"Changing Key of {_items[curIndex].item} from {_items[curIndex].key} to {key}");
            var oldKey = _items[curIndex].key;
            _items[curIndex] = (item, key);
           
            if(oldKey == key)  //key did not change, so position is same
                return;
            if (oldKey < key) //key was increased
            {
                HeapifyDown(curIndex);
              
            }
            else // key was decreased
            {
                HeapifyUp(curIndex);
            }
            Debug.Log(ToString());
        }

        public void PrintDictionary()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var kvp in _indexLookup)
            {
                sb.Append($"[{kvp.Key}, {kvp.Value}]");
                sb.Append(", ");
            }
            Debug.Log(sb.ToString());
        }
        void Swap(int i, int j)
        {
            var temp = _items[i];
            _items[i] = _items[j];
            _items[j] = temp;

            _indexLookup[_items[i].item] = i;
            _indexLookup[_items[j].item] = j;
        }

        void Swap(T a, T b)
        {
            int index1 = _indexLookup[a];
            int index2 = _indexLookup[b];
            Swap(index1, index2);
        }

             int GetParentIndex(int index)
             {
                 if (index < 0 || index > _count - 1)
                     throw new IndexOutOfRangeException($"Must index between 0,{_count}");
                 else if (index == 0)
                     throw new InvalidTreeOperationException("root node has no parent");
        
                 return (index - 1) / 2;
             }
             int GetLeftChild(int index)
             {
                 if (index < 0 || index > _count - 1)
                     throw new IndexOutOfRangeException($"Must index between 0,{_count}");
                 return (2 * index) + 1;
             }
             int GetRightChild(int index)
             {
                 if (index < 0 || index > _count - 1)
                     throw new IndexOutOfRangeException($"Must index between 0,{_count}");
                 return (2 * index) + 2;
             }

             int GetKey(int index) => _items[index].key;
             int GetParentKey(int index) => GetKey(GetParentIndex(index));
             int GetLeftKey(int index) => GetKey(GetLeftChild(index));
             int GetRightKey(int index) => GetKey(GetRightChild(index));
             
        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < _count; i++)
            {
                sb.Append(i);
                sb.Append(":[");
                var value = _items[i].Item1;
                var key =_items[i].Item2;
                sb.Append(value.ToString());
                sb.Append(',');
                sb.Append(key);
                sb.Append("], ");
            }

            return sb.ToString();
        }
        
        
        

        public bool IsAtCapacity() => _count >= _capacity;
    }
}