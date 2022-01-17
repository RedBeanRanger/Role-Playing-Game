using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    //public variables
    public CinemachineBrain Brain;
    public CinemachineVirtualCamera MainVC;

    private bool OnActiveZoom;
    private bool zoomActive; //are we currently in a zoom

    //private variables
    private CinemachineFramingTransposer mainFramingTransposer;

    private LensSettings mainLens;

    private float maxZoomSize = 10f; //farthest distances away from the screen
    private float minZoomSize = 5f; //closest distance away from the screen
    private float enemyInRangeZoomSize = 7.7f;
    private float zoomSpeed = 2f;

    public void OnStart(Transform transform, bool onActiveZoom)
    {
        Brain = GameObject.Find("Main Camera").GetComponent<CinemachineBrain>();
        MainVC = GameObject.Find("MainVC").GetComponent<CinemachineVirtualCamera>();

        MainVC.m_Follow = transform;
        OnActiveZoom = onActiveZoom;
        ConfigureMainFramingTransposer();
        mainLens = MainVC.m_Lens;
        if (OnActiveZoom)
        {
            mainLens.OrthographicSize = maxZoomSize;
        }
        else
        {
            mainLens.OrthographicSize = minZoomSize;
        }
        MainVC.m_Lens = mainLens;
    }

    public void OnUpdate(bool enemyInRange)
    {
        if (OnActiveZoom)
        {
            Debug.Log("Active Zoom Code Executing");
            if (Input.anyKeyDown)
            {
                zoomActive = true;
                OnActiveZoom = false;
            }
        }

        if (zoomActive)
        {
            ZoomToSize(minZoomSize);
        }
        if (enemyInRange)
        {
            Debug.Log("Should be zooming");
            ZoomToSize(enemyInRangeZoomSize);
            Debug.Assert(mainLens.OrthographicSize == MainVC.m_Lens.OrthographicSize);
        }
        if (!enemyInRange)
        {
            ZoomToSize(minZoomSize);
        }
    }

    public void ZoomToSize(float zoomSize)
    {
        mainLens.OrthographicSize = Mathf.Lerp(mainLens.OrthographicSize, zoomSize, zoomSpeed * Time.deltaTime);
        if (mainLens.OrthographicSize == zoomSize || mainLens.OrthographicSize == minZoomSize || mainLens.OrthographicSize == maxZoomSize)
        {
            zoomActive = false;
        }
        MainVC.m_Lens = mainLens;
    }

    private void ConfigureMainFramingTransposer()
    {
        mainFramingTransposer = MainVC.GetCinemachineComponent<CinemachineFramingTransposer>();
        mainFramingTransposer.m_LookaheadTime = 0.5f;
        mainFramingTransposer.m_LookaheadSmoothing = 1.5f;
        mainFramingTransposer.m_DeadZoneWidth = 0.02f;
        mainFramingTransposer.m_DeadZoneHeight = 0.02f;

        Debug.Log("Confirm Camera Reconfiguered");
    }
}
