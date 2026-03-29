using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace TestTask.Editable
{
    public class ClientColors : MonoBehaviour
    {
        [SerializeField] private RectTransform colorItemContainer;
        [SerializeField] private Image colorItemTemplate;

        private List<Image> colorItems = new List<Image>();

        public void RequestNewColorSet()
        {
            ClientPacketsHandler.SendNewColorSetRequest();
        }

        public void SetColors(IList<Color32> colorSet)
        {
            ClearColors();

            foreach (Color32 color in colorSet)
            {
                var colorItem = GameObject.Instantiate(colorItemTemplate, colorItemContainer);
                colorItem.color = color;
                colorItem.gameObject.SetActive(true);
                colorItems.Add(colorItem);
            }

            LogColors(colorSet);
        }

        private void ClearColors()
        {
            foreach (Image colorItem in colorItems)
                Destroy(colorItem.gameObject);
            
            colorItems = new List<Image>();
        }

        private void LogColors(IList<Color32> colorSet)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("[Client] New Color List:");

            for (int i = 0; i < colorSet.Count; i++)
            {
                Color32 c = colorSet[i];
                sb.AppendLine($"[{i}] R:{c.r} G:{c.g} B:{c.b} A:{c.a}");
            }

            Debug.Log(sb.ToString());
        }
    }
}
