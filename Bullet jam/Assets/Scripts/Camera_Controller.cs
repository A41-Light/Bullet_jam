using UnityEngine;
using System.Collections;

public class Camera_Controller : MonoBehaviour
{
    public static Camera_Controller instance; // Singleton reference

    private void Awake()
    {
        instance = this;
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0f);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }

    public void TriggerShake(float duration = 0.2f, float magnitude = 0.1f)
    {
        StartCoroutine(Shake(duration, magnitude));
    }
}
