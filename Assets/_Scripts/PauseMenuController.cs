using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject menuPanel;
    public void ResumeGame()
    {
        EventSystem.current.SetSelectedGameObject(null);
        CloseSettingsPanel();
        GameManager.instance.ResumeGame();
        menuPanel.SetActive(false);
    }

    public void SettingsPanel()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettingsPanel()
    {
        settingsPanel.SetActive(false);
    }

    public void PauseGame()
    {
        GameManager.instance.PauseGame();
        menuPanel.SetActive(true);
    }

    public void GoToMainMenu()
    {
        GameManager.instance.ResumeGame();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenu");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.instance.isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();

            }

        }
    }
}
