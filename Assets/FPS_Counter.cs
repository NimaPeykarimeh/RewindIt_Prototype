using TMPro;
using UnityEngine;

public class FPS_Counter : MonoBehaviour
{
    TextMeshProUGUI fpsText;
    [SerializeField] int listLength = 5;
    int listIndex = 0;
    [SerializeField] float[] fpsList;
    private void Awake()
    {
        fpsText = GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        fpsList = new float[listLength];
    }

    private void Update()
    {
        // Calculate the frame time for the current frame
        float frameTime = Time.unscaledDeltaTime;

        // Store the frame time in the array
        fpsList[listIndex] = frameTime;

        // Increment the index (loop back to 0 if it exceeds the array size)
        listIndex = (listIndex + 1) % fpsList.Length;

        // Calculate the average frame time
        float averageFrameTime = 0f;
        foreach (float time in fpsList)
        {
            averageFrameTime += time;
        }
        averageFrameTime /= fpsList.Length;

        // Calculate the average frame rate (FPS)
        float averageFPS = 1f / averageFrameTime;

        // Update the TextMeshPro component with the average FPS
        fpsText.SetText(averageFPS.ToString("0"));
    }
}
