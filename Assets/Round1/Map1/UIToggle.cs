using UnityEngine;
using UnityEngine.UI;

public class UIToggle : MonoBehaviour
{
    public GameObject[] uiElements; // Danh sách UI cần tắt

    private bool isUIActive = true;

    void Update()
    {
        // Khi nhấn phím I, bật/tắt các UI đã chọn
        if (Input.GetKeyDown(KeyCode.I))
        {
            isUIActive = !isUIActive;

            foreach (GameObject uiElement in uiElements)
            {
                uiElement.SetActive(isUIActive);
            }
        }
    }
}
