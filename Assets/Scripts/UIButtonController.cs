using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIButtonController : MonoBehaviour
{
    public void LoadScene(string _SceneName)
    {
        SceneManager.LoadScene(_SceneName);
    }

    public void OpenPanel(GameObject _Panel)
    {
        _Panel.SetActive(true);
    }

    public void PauseGame()
    {
        
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ClosePanel(GameObject _Panel)
    {
        _Panel.SetActive(false);
    }
}
