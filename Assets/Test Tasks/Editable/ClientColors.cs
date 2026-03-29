using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TestTask.Editable
{
    public class ClientColors : MonoBehaviour
    {
        private List<Color32> colors;

        public static event Action<IEnumerable<Color32>> ColorsChanged;

        public void RequestNewColorSet()
        {
            ClientPacketsHandler.SendNewColorSetRequest();
        }

        public void SetColors(IList<Color32> colorSet)
        {
            colors = colorSet.ToList();
            LogColors(colorSet);
            ColorsChanged?.Invoke(colors);
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
