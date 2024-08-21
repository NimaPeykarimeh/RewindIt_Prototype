using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static public  GameManager instance;
    public bool canMove;
    [SerializeField] GameObject levelCompletePanel;
    LevelPanelManager levelPanelManager;
    bool isComplete;

    [Header("NextLevelTimer")]
    [SerializeField] float nextLevelDelay = 3f;
    [SerializeField] float nextLevelTimer;

    [Space]
    public bool isPaused;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            
            instance = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (isComplete)
        {
            nextLevelTimer -= Time.deltaTime;
            if (nextLevelTimer < 0) 
            {
                isComplete = false;
                canMove = true;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                
            }
        }
        
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1.0f;
        canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void PauseGame()
    {
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        canMove = false;
        Time.timeScale = 0f;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        levelPanelManager = FindFirstObjectByType<LevelPanelManager>();

    }
    public void LevelComplete()
    {
        canMove = false;
        nextLevelTimer = nextLevelDelay;
        isComplete = true;

        
        levelPanelManager.PlayFinishAnimation();
    }
}
