using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ItemStackCounterDisplay : MonoBehaviour
{
    private TextMeshProUGUI _textDisp;
    private ItemStackContainer _stackContainer;

    private void Awake()
    {
        _textDisp = GetComponent<TextMeshProUGUI>();
        _stackContainer = GetComponentInParent<ItemStackContainer>();
    }

    private void Update()
    {
        _textDisp.enabled = _stackContainer.ItemStack.ShowStackIndicator;

        _textDisp.text = _stackContainer.ItemStack.Count.ToString();
    }
}
