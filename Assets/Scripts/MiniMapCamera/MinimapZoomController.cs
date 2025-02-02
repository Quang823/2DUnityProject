using UnityEngine;
using UnityEngine.UI;

public class MinimapZoomController : MonoBehaviour
{
    public GameObject miniMapCanvas;    
    public GameObject fullMapCanvas;    
    public Camera miniMapCamera;       
    public Camera fullMapCamera;        
    public Button zoomButton;         
    public Button closeButton;      

    private bool isZoomed = false;

    void Start()
    {
        if (zoomButton != null)
            zoomButton.onClick.AddListener(OpenFullMap);

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseFullMap);

        SetMinimapState(false); 
    }

    void OpenFullMap()
    {
        isZoomed = true;
        SetMinimapState(isZoomed);
    }

    void CloseFullMap()
    {
        isZoomed = false;
        SetMinimapState(isZoomed);
    }

    void SetMinimapState(bool zoomed)
    {
        if (miniMapCanvas != null) miniMapCanvas.SetActive(!zoomed);
        if (fullMapCanvas != null) fullMapCanvas.SetActive(zoomed);
        if (miniMapCamera != null) miniMapCamera.enabled = !zoomed;
        if (fullMapCamera != null) fullMapCamera.enabled = zoomed;
    }
}
