using DG.Tweening;
using UnityEngine;

public class CameraShakeUtility : MonoBehaviour
{
    private Camera mainCamera;

    public static CameraShakeUtility singleton;
    public static CameraShakeUtility GetCameraShake()
    {
        if (singleton == null)
        {
            singleton = FindObjectOfType<CameraShakeUtility>();

            if (singleton == null)
            {
                Debug.LogError("Yike couldn't find Battle Manager :(");
            }
        }

        return singleton;
    }

    public void ShakeCamera(float duration, float strength = 90, int vibrato = 10, float randomness = 90)
    {
        if (mainCamera == null)
        {
            mainCamera = gameObject.GetComponent<Camera>();
        }

        mainCamera.DOShakeRotation(duration, strength, vibrato, randomness);
    }


}
