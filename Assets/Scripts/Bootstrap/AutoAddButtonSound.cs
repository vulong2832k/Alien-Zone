using UnityEngine;
using UnityEngine.UI;

public class AutoAddButtonSound : MonoBehaviour
{
    private void Awake()
    {
        Button[] buttons = GetComponentsInChildren<Button>(true);

        foreach (var btn in buttons)
        {
            if (btn.GetComponent<UIButtonSound>() == null)
            {
                btn.gameObject.AddComponent<UIButtonSound>();
            }
        }
    }
}
