using System.Collections;
using System.Collections.Generic;
using TestTask.Editable;
using TestTask.NonEditable;
using UnityEngine;
using UnityEngine.UI;

public class ColorsUI : MonoBehaviour
{
    [SerializeField] private RectTransform colorItemContainer;
    [SerializeField] private Image colorItemTemplate;
    
    private List<Image> colorItems = new List<Image>();

    private void OnEnable()
    {
        ClientColors.ColorsChanged += SetColors;
    }

    private void OnDisable()
    {
        ClientColors.ColorsChanged -= SetColors;
    }

    private void SetColors(IEnumerable<Color32> colorSet)
    {
        ClearColors();

        foreach (Color32 color in colorSet)
        {
            var colorItem = GameObject.Instantiate(colorItemTemplate, colorItemContainer);
            colorItem.color = color;
            colorItem.gameObject.SetActive(true);
            colorItems.Add(colorItem);
        }
    }

    private void ClearColors()
    {
        foreach (Image colorItem in colorItems)
            Destroy(colorItem.gameObject);
        
        colorItems = new List<Image>();
    }
}
