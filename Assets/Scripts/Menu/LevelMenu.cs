using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class LevelMenu : MonoBehaviour
{
    private void Update()
    {
        // Ki?m tra n?u nh?n Spacebar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject selectedButton = EventSystem.current.currentSelectedGameObject;

            if (selectedButton != null && selectedButton.name.StartsWith("Round"))
            {
                int levelId = int.Parse(selectedButton.name.Replace("Round", ""));
                OpenLevel(levelId);
            }
        }
    }

    public void OpenLevel(int levelId)
    {
        string levelName = "Round" + levelId;
        SceneManager.LoadScene(levelName);
    }
}
