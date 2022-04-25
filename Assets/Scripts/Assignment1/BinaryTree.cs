using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Assignment1
{
    // public class BinaryTree<T>
    // {
    //     private List<T> _items = new List<T>();
    //     private Dictionary<T, int> _indexLookup = new Dictionary<T, int>();
    //     private int _count;
    //     
    //     public BinaryTree()
    //     {
    //         _count = 0;
    //         _items = new List<T>();
    //         _indexLookup = new Dictionary<T, int>();
    //     }
    //
    //     
    //     /// <summary>
    //     /// if given node is root node, returns the default value
    //     /// </summary>
    //     /// <param name="node"></param>
    //     /// <returns></returns>
    //     /// <exception cref="NodeNotFoundException">thrown if the given node is not in the binary tree</exception>
    //     public T GetParent([NotNull]T node)
    //     {
    //         if (!_indexLookup.ContainsKey(node))
    //             throw new NodeNotFoundException(node);
    //         int nodeIndex = _indexLookup[node];
    //         try
    //         {
    //             return _items[GetParentIndex(nodeIndex)];
    //         }
    //         catch (InvalidTreeOperationException e)
    //         {
    //             Console.WriteLine(e);
    //             return default;
    //         }
    //     }
    //     
    //     public T GetLeftChild([NotNull]T node)
    //     {
    //         if (!_indexLookup.ContainsKey(node))
    //             throw new NodeNotFoundException(node);
    //         int nodeIndex = _indexLookup[node];
    //         try
    //         {
    //             return _items[GetLeftChild(nodeIndex)];
    //         }
    //         catch (InvalidTreeOperationException e)
    //         {
    //             Console.WriteLine(e);
    //             return default;
    //         }
    //     }
    //     
    //      
    //     public T GetRightChild([NotNull]T node)
    //     {
    //         if (!_indexLookup.ContainsKey(node))
    //             throw new NodeNotFoundException(node);
    //         int nodeIndex = _indexLookup[node];
    //         try
    //         {
    //             return _items[GetLeftChild(nodeIndex)];
    //         }
    //         catch (InvalidTreeOperationException e)
    //         {
    //             Console.WriteLine(e);
    //             return default;
    //         }
    //     }
    //
    //     public bool HasRightChild(T node)
    //     {
    //         if (!_indexLookup.ContainsKey(node))
    //             throw new NodeNotFoundException(node);
    //         int index = _indexLookup[node];
    //     }
    //
    //     public bool HasLeftChild(T node)
    //     {
    //         if (!_indexLookup.ContainsKey(node))
    //             throw new NodeNotFoundException(node);
    //         int index = _indexLookup[node];
    //     }
    //     
    //     int GetParentIndex(int index)
    //     {
    //         if (index < 0 || index > _count - 1)
    //             throw new IndexOutOfRangeException($"Must index between 0,{_count}");
    //         else if (index == 0)
    //             throw new InvalidTreeOperationException("root node has no parent");
    //
    //         return (index - 1) / 2;
    //     }
    //
    //     int GetLeftChild(int index)
    //     {
    //         if (index < 0 || index > _count - 1)
    //             throw new IndexOutOfRangeException($"Must index between 0,{_count}");
    //         return (2 * index) + 1;
    //     }
    //
    //     int GetRightChild(int index)
    //     {
    //         if (index < 0 || index > _count - 1)
    //             throw new IndexOutOfRangeException($"Must index between 0,{_count}");
    //         return (2 * index) + 2;
    //     }
    // }
}