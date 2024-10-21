using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;
    public int xSenValue;
    public int ySenValue;

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
}
