using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum Quadrant  // ordered clockwise from top left
{
    TopLeft,
    TopRight,
    BottomRight,
    BottomLeft
}

public class Quadtree
{
    public const int NodesPerLevel = 4;
    public static int BucketSize = 2;

    private QuadtreeNode _root { get; set; }
    private List<QuadtreeElement> QuadtreeElements { get; set; }

    public Quadtree(Vector2 centre, Vector2 extents)
    {
        _root = new QuadtreeNode(centre, extents);
        QuadtreeElements = new List<QuadtreeElement>();
    }

    public void Add(QuadtreeElement elem)
    {
        void TryAddAtNode(QuadtreeNode node, QuadtreeElement element)
        {
            if (node.Elements.Count < BucketSize)
            {
                node.Elements.Add(element);
                node.PropagateSubtreeElementCount(1);
                element.Node = node;
            }
            else
            {
                //Debug.Log("new noding, level of node: " + node.Depth);
                node.InstantiateNewNodes();
                // shift current elements in node down a level to respective nodes
                foreach (var e in node.Elements)
                {
                    var s = node.Search(e);
                    s.Elements.Add(e);
                    s.SubtreeElementCount++;
                    e.Node = s;
                }
                node.Elements.Clear();

                var nodeForNewElem = node.Search(element);
                TryAddAtNode(nodeForNewElem, element);
            }
        }

        //Debug.Assert(!QuadtreeElements.Exists(x => x.Transform == tr), "Transform already present in quadtree!");
        Debug.Assert(!QuadtreeElements.Contains(elem), "Transform already present in quadtree!");

        var node = _root.Search(elem);
        TryAddAtNode(node, elem);

        //var elem = new QuadtreeElement(tr, null);
        QuadtreeElements.Add(elem);
    }

    public void Remove(QuadtreeElement elem)
    {
        void RemoveChildren(QuadtreeNode node)
        {
            Array.Clear(node.Children, 0, Quadtree.NodesPerLevel);
            node.IsLeaf = true;
        }

        void CheckIfCollapse(QuadtreeNode node)
        {
            if (node.Parent == null)
            {
                return;
            }

            if (node.Parent.SubtreeElementCount <= Quadtree.BucketSize)
            {
                for (int i = 0; i < Quadtree.NodesPerLevel; i++)
                {
                    node.Parent.Elements.UnionWith(node.Parent.Children[i].Elements);
                }
                foreach (var e in node.Parent.Elements)
                {
                    e.Node = node.Parent;
                }
                RemoveChildren(node.Parent);

                CheckIfCollapse(node.Parent);
            }
        }

        //var elem = QuadtreeElements.Find(x => x.Element == elem);
        Debug.Assert(QuadtreeElements.Contains(elem), "Entity not present in quadtree!");
        Debug.Assert(elem.Node != null, "Element node not set!");

        var node = elem.Node;
        node.Elements.Remove(elem);
        node.PropagateSubtreeElementCount(-1);
        elem.Node = null;

        CheckIfCollapse(node);

        QuadtreeElements.Remove(elem);
    }

    public void ValidatePartitionOf(QuadtreeElement elem)
    {
        //var elem = QuadtreeElements.Find(x => x.Element == tr);
        //Debug.Assert(elem != null, "Transform not present in quadtree!");
        Debug.Assert(QuadtreeElements.Contains(elem), "Entity not present in quadtree!");

        //Debug.Log("Root stec: " + _root.SubtreeElementCount);

        if (!elem.Node.Bounds.Contains(elem.Transform.position))
        {
            Remove(elem);
            Add(elem);
        }
    }

    public void GetElementsAtPoint(Vector2 point, List<QuadtreeElement> elements, out QuadtreeNode containingNode)
    {
        Debug.Assert(_root.Bounds.Contains(point), "point outside of root bounds!");

        if (_root.IsLeaf)
        {
            elements.AddRange(_root.Elements);
            containingNode = _root;
            return;
        }

        var node = _root.GetDeeperEnclosingNode(point);
        while (!node.IsLeaf)
        {
            node = node.GetDeeperEnclosingNode(point);
        }

        elements.AddRange(node.Elements);
        containingNode = node;
    }

    public void GetElementsInBounds(Bounds bounds, List<QuadtreeElement> elements, List<QuadtreeNode> containingNodes)
    {
        //elements = new List<QuadtreeElement>();
        //Debug.Log("Getting elements in bounds...");
        //return;

        Debug.Assert(elements != null, "elements list is null!");
        Debug.Assert(containingNodes != null, "containing nodes list is null!");

        Debug.Assert(_root.Bounds.Contains(bounds.min) && _root.Bounds.Contains(bounds.max), "min and max outside of root bounds!");
        //var prev = _root;
        //var curr = _root;
        // FIXME
        //while (!curr.IsLeaf && curr.Bounds.Contains(bounds.min) && curr.Bounds.Contains(bounds.max))
        //{
        //    prev = curr;
        //    curr = curr.GetDeeperEnclosingNode(bounds.min);
        //    //if (curr == _root)
        //    //{
        //    //    break;
        //    //}
        //}

        if (_root.IsLeaf)
        {
            elements.AddRange(_root.Elements);
            return;
        }

        var minNode = _root;
        var maxNode = _root;
        

        while (minNode == maxNode)
        {
            if (minNode.IsLeaf)
            {
                elements.AddRange(minNode.Elements);
                return;
            }

            minNode = minNode.GetDeeperEnclosingNode(bounds.min);
            maxNode = maxNode.GetDeeperEnclosingNode(bounds.max);
        }

        containingNodes.Add(minNode);
        containingNodes.Add(maxNode);

        var lowestCommonAncestor = minNode.Parent;

        //if (prev.IsLeaf)
        //{
        //    elements.AddRange(prev.Elements);
        //    return;
        //}

        // from prev, do BFS
        //var currentLayer = new List<QuadtreeNode>();
        //var nextLayer = new List<QuadtreeNode>();
        //currentLayer.Add(prev);
        //while (currentLayer.Count > 0)
        //{
        //    foreach (var node in currentLayer)
        //    {
        //        foreach (var child in node.Children)
        //        {
        //            if (child.IsLeaf)
        //            {
        //                elements.AddRange(child.Elements);
        //                continue;
        //            }
        //            if (child.Bounds.Intersects(bounds))
        //            {
        //                nextLayer.Add(child);
        //            }
        //        }
        //    }
        //    currentLayer.Clear();
        //    currentLayer.AddRange(nextLayer);
        //    nextLayer.Clear();
        //}

        // from prev, traverse subtree
        AddToElements(lowestCommonAncestor, elements);

        void AddToElements(QuadtreeNode node, List<QuadtreeElement> elements)
        {
            if (node.IsLeaf)
            {
                elements.AddRange(node.Elements);
                return;
            }

            foreach (var child in node.Children)
            {
                AddToElements(child, elements);
            }
        }
    }

    public void DrawBoundsInScene()
    {
        _root.DrawBoundsInScene();
    }

    public static Quadrant GetQuadrant(Vector3 pos, Bounds bounds)
    {
        Quadrant res;
        if (pos.x < bounds.center.x && pos.y > bounds.center.y)
        {
            res = Quadrant.TopLeft;
        }
        else if (pos.x > bounds.center.x && pos.y > bounds.center.y)
        {
            res = Quadrant.TopRight;
        }
        else if (pos.x > bounds.center.x && pos.y < bounds.center.y)
        {
            res = Quadrant.BottomRight;
        }
        else
        {
            res = Quadrant.BottomLeft;
        }
        return res;
    }

    
}

public class QuadtreeNode
{
    public QuadtreeNode[] Children { get; set; }
    public QuadtreeNode Parent { get; }

    public int SubtreeElementCount { get; set; }
    public int MaxSubtreeDepth { get; set; }
    public int Depth { get; }

    public Bounds Bounds { get; }

    public HashSet<QuadtreeElement> Elements { get; set; }

    public bool IsLeaf { get; set; }

    public QuadtreeNode(Vector2 centre, Vector2 extents)
    {
        Bounds = new Bounds(centre, extents * 2);
        Children = new QuadtreeNode[Quadtree.NodesPerLevel];
        Elements = new HashSet<QuadtreeElement>();
        IsLeaf = true;
        SubtreeElementCount = 0;
        Depth = 0;
    }

    private QuadtreeNode(Vector2 centre, Vector2 extents, QuadtreeNode parent)
    {
        Bounds = new Bounds(centre, extents * 2);
        Children = new QuadtreeNode[Quadtree.NodesPerLevel];
        Elements = new HashSet<QuadtreeElement>();
        IsLeaf = true;
        SubtreeElementCount = 0;
        Parent = parent;
        Depth = parent.Depth + 1;
    }

    public QuadtreeNode GetDeeperEnclosingNode(Vector2 pos)
    {
        if (IsLeaf)
        {
            return this;
        }

        var quadrant = (int)Quadtree.GetQuadrant(pos, Bounds);
        Debug.Assert(Children[quadrant] != null, "Uninitialised child node encountered in search");
        return Children[quadrant];
    }

    public QuadtreeNode Search(QuadtreeElement elem)
    {
        if (IsLeaf)
        {
            return this;
        }

        var quadrant = (int)Quadtree.GetQuadrant(elem.Transform.position, Bounds);
        Debug.Assert(Children[quadrant] != null, "Uninitialised child node encountered in search");
        return Children[quadrant].Search(elem);
    }

    public void PropagateSubtreeElementCount(int increment)
    {
        SubtreeElementCount += increment;
        if (Parent != null)
        {
            Parent.PropagateSubtreeElementCount(increment);
        }
    }

    public void DrawBoundsInScene()
    {
        var tl = (Vector2)Bounds.center + new Vector2(-Bounds.extents.x, Bounds.extents.y);
        var tr = (Vector2)Bounds.center + new Vector2(Bounds.extents.x, Bounds.extents.y);
        var br = (Vector2)Bounds.center + new Vector2(Bounds.extents.x, -Bounds.extents.y);
        var bl = (Vector2)Bounds.center + new Vector2(-Bounds.extents.x, -Bounds.extents.y);

        Debug.DrawLine(tl, tr);
        Debug.DrawLine(tr, br);
        Debug.DrawLine(br, bl);
        Debug.DrawLine(bl, tl);

        foreach (var c in Children)
        {
            if (c != null)
            {
                c.DrawBoundsInScene();
            }
        }
    }

    //public void RemoveChildren()
    //{
    //    Debug.Log("Level: " + Depth);
    //    Debug.Log("stec: " + SubtreeElementCount);
    //    Array.Clear(Children, 0, Quadtree.NodesPerLevel);
    //    IsLeaf = true;
    //}


    public void InstantiateNewNodes()
    {
        Children[(int)Quadrant.TopLeft] = new QuadtreeNode((Vector2)Bounds.center + new Vector2(-Bounds.extents.x, Bounds.extents.y) / 2f, Bounds.extents / 2f, this);
        Children[(int)Quadrant.TopRight] = new QuadtreeNode((Vector2)Bounds.center + new Vector2(Bounds.extents.x, Bounds.extents.y) / 2f, Bounds.extents / 2f, this);
        Children[(int)Quadrant.BottomRight] = new QuadtreeNode((Vector2)Bounds.center + new Vector2(Bounds.extents.x, -Bounds.extents.y) / 2f, Bounds.extents / 2f, this);
        Children[(int)Quadrant.BottomLeft] = new QuadtreeNode((Vector2)Bounds.center + new Vector2(-Bounds.extents.x, -Bounds.extents.y) / 2f, Bounds.extents / 2f, this);
        IsLeaf = false;
    }
}