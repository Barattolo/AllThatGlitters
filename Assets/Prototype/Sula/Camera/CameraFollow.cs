using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Vector3 maxCameraOffset;
    Vector3 minCameraOffset;
    Vector3 currentCameraOffset;

    [SerializeField]
    float smoothSpeed = 2f;    
    float zoomT;
    float scrollSpeed = 30f;

    private void OnEnable()
    {
        GameManager.singleton.LevelReset += DestroySelf;
    }

    private void OnDisable()
    {
        GameManager.singleton.LevelReset -= DestroySelf;
    }

    private void Start()
    {
        transform.parent = null;
        maxCameraOffset = transform.position - player.transform.position;
        minCameraOffset = Vector3.Lerp(player.transform.position, maxCameraOffset, .5f);
        currentCameraOffset = maxCameraOffset;
    }
    private void LateUpdate()
    {
        cameraZoom();
        Follow();
    }

    private void Follow()
    {
        if (!player)
            return;
        Vector3 desiredPosition = currentCameraOffset + player.transform.position; //al momento il player � sollevato di 1 su Y
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }

    void cameraZoom()
    {
        if (Input.mouseScrollDelta != Vector2.zero)
        {
            //mi serve solo la y 1, -1
            float yScrollValue = Input.mouseScrollDelta.y;
            //lo rendo frame indipendent, lo moltiplico per una velocit� e lo clampo per usarlo nel lerp
            zoomT = Mathf.Clamp(zoomT += yScrollValue * Time.deltaTime * scrollSpeed, 0, 1);
            //lo uso per lerpare fra max distance e min distance          
            currentCameraOffset = Vector3.Lerp(maxCameraOffset, minCameraOffset, zoomT);
            //si smootha da solo nella funzione di follow (che fortuna)
        }
    }

}
