using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

[RequireComponent(typeof(VCamSetting))]
public class CarCameraSystem : MonoBehaviour
{
    public static CarCameraSystem instance;
    private CinemachineVirtualCamera _vcam;
    private CinemachineComposer composer;
    public GameObject thirdCamPos;

    public bool canRotate = false;
    [SerializeField]
    private float firstPersonCamSpeed = 1;
    public float FirstPersonCameraSpeed
    {
        get => firstPersonCamSpeed / 10;
        set => firstPersonCamSpeed = value;
    }
    public float ThirdPersonCameraSpeed = 1.5f;
    public KeyCode ResetCameraKey = KeyCode.Alpha1;


    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        _vcam = GetComponent<CinemachineVirtualCamera>();
        composer = _vcam.GetCinemachineComponent<CinemachineComposer>();
    }
    private void Start()
    {
    }

    void Update()
    {
        switch (VCamSetting.instance.CurrentView)
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
            if(GameManager.instance.CurrentCar.AGear == GlobalType.AutoGear.R)
            {
                thirdCamPos.transform.rotation = new Quaternion(0, 180, 0, 0);
            }
            else
            {
                thirdCamPos.transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            //float x = Input.GetAxis("Mouse X") * ThirdPersonCameraSpeed;
            //float y = Input.GetAxis("Mouse Y") * ThirdPersonCameraSpeed;
            //Quaternion q = thirdCamPos.transform.rotation;
            //q.eulerAngles = new Vector3(q.eulerAngles.x + y, q.eulerAngles.y + x, q.eulerAngles.z);
            //thirdCamPos.transform.rotation = q;

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
            composer.m_TrackedObjectOffset += new Vector3(Input.GetAxis("Mouse X") * FirstPersonCameraSpeed, Input.GetAxis("Mouse Y") * FirstPersonCameraSpeed, 0);
            composer.m_TrackedObjectOffset = new Vector3(Mathf.Clamp(composer.m_TrackedObjectOffset.x, -5, 5), Mathf.Clamp(composer.m_TrackedObjectOffset.y, -5, 5), 0);
        }
        
    }
}
