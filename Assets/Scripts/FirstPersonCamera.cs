using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FirstPersonCamera : MonoBehaviour
{
    private CinemachineVirtualCamera _vcam;
    private CinemachineComposer composer;

    public bool canRotate = false;
    public float CameraSpeed = 0.1f;
    public KeyCode ResetCameraKey = KeyCode.Alpha1;

    void Awake()
    {
        _vcam = GetComponent<CinemachineVirtualCamera>();
        composer = _vcam.GetCinemachineComponent<CinemachineComposer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            canRotate = canRotate ? false : true;
        }

        if (Input.GetKeyDown(ResetCameraKey) || !canRotate)
        {
            composer.m_TrackedObjectOffset = Vector3.zero;
        }
        else
        {
            composer.m_TrackedObjectOffset += new Vector3(Input.GetAxis("Mouse X") * CameraSpeed, Input.GetAxis("Mouse Y") * CameraSpeed, 0);
            composer.m_TrackedObjectOffset = new Vector3(Mathf.Clamp(composer.m_TrackedObjectOffset.x, -5, 5), Mathf.Clamp(composer.m_TrackedObjectOffset.y, -5, 5), 0);
        }
    }
}
