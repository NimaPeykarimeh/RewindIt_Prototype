using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPanelManager : MonoBehaviour
{
    public Animator animator;
    [SerializeField] TextMeshProUGUI levelText;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        levelText.SetText(SceneManager.GetActiveScene().name);
    }
    public void PlayFinishAnimation()
    {
        levelText.SetText("LEVEL");
        animator.SetTrigger("LevelComplete");
    }
}
