using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Node", menuName = "Scriptable Objects/Dialogue/Node")]
public class DialogueNode : ScriptableObject
{
    //[field: SerializeField] public DialogueNode Terminated { get; private set; }

    [field: SerializeField] public DialogueContext Context { get; set; }
    [field: SerializeField] private List<DialogueNode> _children { get; set; } = new List<DialogueNode>();
    [field: SerializeField] private List<string> _dialoguePool { get; set; } = new List<string>();

    public void AddChild(DialogueNode node)
    {
        Debug.Assert(node != null, "Trying to add null node!");
        _children.Add(node);
    }
    public void AddStringToPool(string str)
    {
        Debug.Assert(str != null, "Trying to add null to dialogue pool!");
        _dialoguePool.Add(str);
    }

    public string GetRandomStringFromPool()
    {
        Debug.Assert(_dialoguePool != null, "Dialogue pool for this node is uninitalised!");

        if (_dialoguePool.Count == 0)
        {
            return string.Empty;
        }
        return _dialoguePool[Random.Range(0, _dialoguePool.Count)];
    }

    public bool CheckContextSatisfied(DialogueContext context)
    {
        //Debug.Assert(context != null, "Dialogue context is null!");
        return true;
    }

    public bool TryTransition(DialogueContext context, out DialogueNode next)
    {
        Debug.Assert(_children != null, "Children list is uninitalised!");

        next = this;
        if (_children.Count == 0)
        {
            return false;
        }

        var possibleNodes = _children.FindAll(n => n.CheckContextSatisfied(context));

        if (possibleNodes.Count == 0)
        {
            return false;
        }
        next = _children[Random.Range(0, _children.Count)];
        return true;
    }
}
