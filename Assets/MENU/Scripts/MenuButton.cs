using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    [SerializeField] MenuButtonController menuButtonController;
    [SerializeField] Animator animator;
    [SerializeField] AnimatorFunctions animatorFunctions;
    [SerializeField] int thisIndex;
    [SerializeField] private GameObject mapSelectionPanel;
    [SerializeField] private GameObject mainMenuPanel;

    private bool isSelectingMap = false; // Kiểm soát trạng thái chọn map

    //void Update()
    //{
    //    if (menuButtonController.index == thisIndex)
    //    {
    //        animator.SetBool("selected", true);
    //        if (Input.GetAxis("Submit") == 1)
    //        {
    //            animator.SetBool("pressed", true);
    //            HandleButtonClick();
    //        }
    //        else if (animator.GetBool("pressed"))
    //        {
    //            animator.SetBool("pressed", false);
    //            animatorFunctions.disableOnce = true;
    //        }
    //    }
    //    else
    //    {
    //        animator.SetBool("selected", false);
    //    }
    //}
    void Update()
    {
        if (menuButtonController.currentState == MenuState.MainMenu && menuButtonController.index == thisIndex)
        {
            animator.SetBool("selected", true);
            if (Input.GetButtonDown("Submit"))
            {
                animator.SetBool("pressed", true);
                HandleButtonClick();
            }
        }
        else if (menuButtonController.currentState == MenuState.MapSelection && gameObject.name.StartsWith("Map"))
        {
            if (menuButtonController.index == thisIndex && Input.GetButtonDown("Submit"))
            {
                LoadLevel(gameObject.name);
            }
        }
        else
        {
            animator.SetBool("selected", false);
        }
    }


    private void HandleButtonClick()
    {
        if (gameObject.name == "Play")
        {
            ShowMapSelection();
        }
        else if (isSelectingMap && gameObject.name.StartsWith("Round"))  // Kiểm tra "Round5" thay vì "Map5"
        {
            LoadLevel(gameObject.name);
        }
    }


    private void ShowMapSelection()
    {
        if (mapSelectionPanel != null && mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(false);  // Ẩn menu chính
            mapSelectionPanel.SetActive(true); // Hiển thị menu chọn bản đồ
            isSelectingMap = true;  // Chuyển trạng thái sang chọn map
        }
    }

    //private void ShowMapSelection()
    //{
    //    if (mapSelectionPanel != null && mainMenuPanel != null)
    //    {
    //        mainMenuPanel.SetActive(false);
    //        mapSelectionPanel.SetActive(true);
    //        menuButtonController.ChangeState(MenuState.MapSelection);
    //    }
    //}


    //private void LoadLevel(string mapButtonName)
    //{
    //    string levelId = mapButtonName.Replace("Round", ""); // Lấy số từ tên button (Map5 -> 5)

    //    string levelName = "Round" + levelId; // Tạo tên scene (Round5)
    //    SceneManager.LoadScene(levelName);
    //}
    private void LoadLevel(string mapButtonName)
    {
        string levelId = mapButtonName.Replace("Round", "").Trim(); // Lấy số từ tên button
        string levelName = "Round" + levelId; // Tạo tên scene (Round5)

        Debug.Log("Đang load scene: " + levelName);

        if (Application.CanStreamedLevelBeLoaded(levelName))
        {
            SceneManager.LoadScene(levelName);
        }
        else
        {
            Debug.LogError("Scene " + levelName + " không tồn tại! Kiểm tra Build Settings.");
        }
    }

}
