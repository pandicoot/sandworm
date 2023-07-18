using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInteractionComponent : MonoBehaviour, IInteractionComponent
{
    [field: SerializeField] public List<DialogueNode> TreeRoots { get; private set; }
    private DialogueNode _root { get; set; }
    [field: SerializeField] private DialogPanel _dialogPanel { get; set; }

    [field: SerializeField] public string EntityName { get; set; }

    public Action<string, string> UpdatedText;

    private DialogueNode _curr;

    private void Awake()
    {
        _root = ScriptableObject.CreateInstance<DialogueNode>();
        TreeRoots.ForEach(x => _root.AddChild(x));
        _curr = _root;
    }

    public void ReceiveInteraction(Interaction interaction)
    {
        Debug.Log($"{interaction.Interactor} interacted with me, {interaction.Interactable}!");
        if (_root.TryTransition(null, out _curr))
        {
            _dialogPanel.RegisterController(this);
            UpdatedText?.Invoke(EntityName, _curr.GetRandomStringFromPool());
        }
    }

    public void Next()
    {
        bool res = _curr.TryTransition(null, out _curr);
        if (!res)
        {
            // terminated
            _dialogPanel.Deregister();
            Reset();
            return;
        }

        UpdatedText?.Invoke(EntityName, _curr.GetRandomStringFromPool());
    }

    private void Reset()
    {
        _curr = _root;
    }

    
}
