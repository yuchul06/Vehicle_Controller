using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class FirstPersonCamera : MonoBehaviour
{
    Rigidbody2D asdf;
    private CinemachineVirtualCamera _vcam;
    private CinemachineComposer composer;

    public bool canRotate = false;
    public float CameraSpeed = 0.1f;
    public KeyCode ResetCameraKey = KeyCode.Alpha1;

    private VCamSetting _vCamSet;

    void Awake()
    {
        _vcam = GetComponent<CinemachineVirtualCamera>();
        _vCamSet = GetComponent<VCamSetting>();
        composer = _vcam.GetCinemachineComponent<CinemachineComposer>();
    }
    private void Start()
    {
    }

    void Update()
    {
        switch (_vCamSet.CurrentView)
        {
            case Views.FirstPerson:
                FirstPersonCam();
                break;
            case Views.ThirdPerson:
                ThirdPersonCam();
                break;
            case Views.Bonnet:
                BonnetCam();
                break;
            case Views.LeftFender:
                FenderCam(true);
                break;
            case Views.RightFender:
                FenderCam(false);
                break;
            case Views.Top:
                TopCam();
                break;
            default:
                break;
        }
        
        
    }

    private void TopCam()
    {

    }

    private void FenderCam(bool isLeft)
    {

    }

    private void BonnetCam()
    {

    }

    private void ThirdPersonCam()
    {
        if(GameManager.instance.CurrentCar != null)
        {
           
        }
    }

    private void FirstPersonCam()
    {
        if (Input.GetMouseButtonDown(1))
        {
            canRotate = canRotate ? false : true;
        }
        if (Input.GetKeyDown(ResetCameraKey) || !canRotate)
        {
            composer.m_TrackedObjectOffset = Vector3.zero;
        }

        if (canRotate)
        {
            composer.m_TrackedObjectOffset += new Vector3(Input.GetAxis("Mouse X") * CameraSpeed, Input.GetAxis("Mouse Y") * CameraSpeed, 0);
            composer.m_TrackedObjectOffset = new Vector3(Mathf.Clamp(composer.m_TrackedObjectOffset.x, -5, 5), Mathf.Clamp(composer.m_TrackedObjectOffset.y, -5, 5), 0);
        }
        
    }
}
