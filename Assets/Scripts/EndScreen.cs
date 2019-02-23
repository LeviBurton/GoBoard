using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class EndScreen : MonoBehaviour
{
    public PostProcessProfile blurProfile;
    public PostProcessProfile normalProfile;
    public PostProcessVolume postProcessVolume;

    public void EnableCameraBlur(bool state)
    {
        if (state)
        {
            postProcessVolume.profile = blurProfile;
        }
        else
        {
            postProcessVolume.profile = normalProfile;
        }
    }
}
