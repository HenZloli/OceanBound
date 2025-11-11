using Unity.Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public CinemachineCamera playerCamera;
    private float normalSize = 4.5f;

    private float targetSize;
    private float zoomVelocity = 0;
    public float smoothTime = 0.3f;

    void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = FindAnyObjectByType<CinemachineCamera>();
        }

        targetSize = normalSize;
        playerCamera.Lens.OrthographicSize = normalSize;
    }

    void Update()
    {
        float currentSize = playerCamera.Lens.OrthographicSize;
        currentSize = Mathf.SmoothDamp(currentSize, targetSize, ref zoomVelocity, smoothTime);
        playerCamera.Lens.OrthographicSize = currentSize;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            targetSize = normalSize - 2f; // zoom in
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            targetSize = normalSize; // zoom out
        }
    }
}
