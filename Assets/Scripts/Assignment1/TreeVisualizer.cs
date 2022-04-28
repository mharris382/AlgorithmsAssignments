using System;
using UnityEngine;

namespace Assignment1
{
    public class TreeVisualizer : MonoBehaviour
    {
        public TreeNodeVisual prefab;
        public Transform treeParent;
        public Vector2 offset = new Vector2(1, -1);


        public Vector2 GetLeftPosition(TreeNodeVisual parent)
        {
            var pos = parent.transform.position;
            return ((Vector2) pos) + new Vector2(-offset.x, offset.y);
        }

        public Vector2 GetLeftOffset(int currentDepth, int maxDepth) => new Vector2(-offset.x * (maxDepth - currentDepth), offset.y);
        public Vector2 GetRightOffset(int currentDepth, int maxDepth) => new Vector2(offset.x* (maxDepth - currentDepth), offset.y);

        public Vector2 GetRightPosition(TreeNodeVisual parent)
        {
            var pos = parent.transform.position;
            return (Vector2)pos + offset;
        }

        public void BuildTree<T>(BinaryHeap<T> heap)
        {
            if (treeParent.childCount > 0)
            {
                Destroy(treeParent.GetChild(0).gameObject);
            }
            var root = heap.Root();
            int d = 0;
            var visual = BuildTreeFrom<T>(root, heap, ref d);
            visual.transform.SetParent(treeParent);
        }

        TreeNodeVisual BuildTreeFrom<T>(T node, BinaryHeap<T> heap, ref int depth)
        {
            var self = CreateNode(node);
            self.SetInfo(node.ToString());
            int leftDepth = depth;
            var left = GetLeftChild(node, heap, ref leftDepth);
            if (left != null)
            {
                left.transform.SetParent(self.transform);
                left.transform.localPosition = GetLeftOffset(depth, leftDepth);
            }

            int rightDepth = depth;
            var right = GetRightChild(node, heap, ref rightDepth);
            if (right != null)
            {
                right.transform.SetParent(self.transform);
                right.transform.localPosition = GetRightOffset(depth, rightDepth);
            }
            depth = Mathf.Max(rightDepth, leftDepth);
            return self;
        }

        TreeNodeVisual GetLeftChild<T>(T parent, BinaryHeap<T> heap, ref int depth)
        {
            depth += 1;
            if (heap.GetLeftChild(parent, out T left))
            {
                return BuildTreeFrom(left, heap ,ref depth);
            }

            return null;
        }
        
        TreeNodeVisual GetRightChild<T>(T parent, BinaryHeap<T> heap, ref int depth)
        {
            depth += 1;
            if (heap.GetRightChild(parent, out T right))
            {
                return BuildTreeFrom(right, heap, ref depth);
            }
            return null;
        }

        TreeNodeVisual CreateNode()
        {
            return Instantiate(prefab, treeParent);
        }
        TreeNodeVisual CreateNode<T>(T node)
        {
            var instance = Instantiate(prefab, treeParent);
            instance.name = node.ToString();
            return instance;
        }
        TreeNodeVisual CreateNodeVisual(TreeNodeVisual parent, bool isLeft)
        {
            if (parent == null)
            {
                return Instantiate(prefab, treeParent);
            }

            var instance = Instantiate(prefab, isLeft ? GetLeftPosition(parent) : GetRightPosition(parent), Quaternion.identity, parent.transform);
            if(!isLeft)instance.transform.SetAsFirstSibling();
            else instance.transform.SetAsLastSibling();
            return instance;
        }
    }
}