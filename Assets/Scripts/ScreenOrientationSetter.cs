using UnityEngine;

public class ScreenOrientationSetter : MonoBehaviour
{
    void Awake()
    {
        // lock to landscape only
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.orientation = ScreenOrientation.AutoRotation;
    }
}
