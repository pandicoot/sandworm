using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelToggleBlockRaycast : MonoBehaviour
{
    private CanvasGroup _group { get; set; }

    public bool IsOpen
    {
        get
        {
            if (_group != null)
            {
                return _group.interactable;
            }
            else
            {
                return false;
            }
        }
    }

    public void AllowRaycastTarget()
    {
        if (_group != null)
        {
            _group.blocksRaycasts = true;
        }
    }

    public void DisallowRaycastTarget()
    {
        if (_group != null)
        {
            _group.blocksRaycasts = false;
        }
    }
}
