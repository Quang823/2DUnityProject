using UnityEngine;
using UnityEngine.UI;

public class ImageToggle : MonoBehaviour
{
    public Image image1;
    public Image image2;
    public Button buttonO;
    public Button buttonP;

    private bool isImage1Active = false;
    private bool isImage2Active = false;

    void Start()
    {
        // Ẩn hai hình ảnh khi bắt đầu
        image1.gameObject.SetActive(false);
        image2.gameObject.SetActive(false);

        // Thêm sự kiện bấm nút UI
        buttonO.onClick.AddListener(ToggleImage1);
        buttonP.onClick.AddListener(ToggleImage2);
    }

    void Update()
    {
        // Nhấn phím O để bật/tắt ảnh 1
        if (Input.GetKeyDown(KeyCode.O))
        {
            ToggleImage1();
        }

        // Nhấn phím P để bật/tắt ảnh 2
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleImage2();
        }
    }

    void ToggleImage1()
    {
        isImage1Active = !isImage1Active;
        image1.gameObject.SetActive(isImage1Active);
    }

    void ToggleImage2()
    {
        isImage2Active = !isImage2Active;
        image2.gameObject.SetActive(isImage2Active);
    }
}
    