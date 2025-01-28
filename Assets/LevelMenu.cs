using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    //public Button[] buttons;


    // ch? m? nh?ng level ?ã ch?i r?i
    // m? level 1, sau khi ch?i xong level 1 thì level 2 ???c m?.
    // ch?m ?i?m k?t thúc c?a m?i màn ch?i

    //private void Awake()
    //{
    //    int unlocketLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
    //    for (int i = 0; i < buttons.Length; i++)
    //    {
    //        buttons[i].interactable = false;
    //    }

    //    for(int i = 0;i< unlocketLevel; i++)
    //    {
    //        buttons[i].interactable = true;
    //    }

    //}
    public void OpenLevel(int levelId)
    {
        string levelName = "Round" + levelId;
        SceneManager.LoadScene(levelName);
    }
   
}
