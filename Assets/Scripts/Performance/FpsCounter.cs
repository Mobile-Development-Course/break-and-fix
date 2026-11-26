using UnityEngine;
using UnityEngine.UI;

// Frames per second and milliseconds per frame, in the corner of the screen.
// It makes no garbage: every possible label is built once, in Awake.
public class FpsCounter : MonoBehaviour
{
    const int MaxFps = 240;

    [SerializeField] Text label;

    string[] labels;
    float smoothedDelta = 1f / 60f;
    float nextUpdate;

    void Awake()
    {
        // Phones run at 30 fps unless a game asks for more. This scene asks for 60, so drops show.
        Application.targetFrameRate = 60;

        labels = new string[MaxFps + 1];
        labels[0] = "-- fps";
        for (int fps = 1; fps <= MaxFps; fps++)
            labels[fps] = $"{fps} fps   {1000f / fps:0.0} ms";
    }

    void Update()
    {
        smoothedDelta = Mathf.Lerp(smoothedDelta, Time.unscaledDeltaTime, 0.1f);

        if (Time.unscaledTime < nextUpdate)
            return;
        nextUpdate = Time.unscaledTime + 0.25f;

        int fps = Mathf.Clamp(Mathf.RoundToInt(1f / smoothedDelta), 0, MaxFps);
        label.text = labels[fps];
    }
}
