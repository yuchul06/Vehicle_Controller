using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VCamSetting : MonoBehaviour
{
    public CinemachineVirtualCamera VCam;
    static public VCamSetting instance;
    public ViewOption[] targets;
    public Views CurrentView = Views.FirstPerson;
    public KeyCode ChangeViewKey = KeyCode.C;
    private int ViewsCount
    {
        get => System.Enum.GetValues(typeof(Views)).Length;
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    
   
        VCam = GetComponent<CinemachineVirtualCamera>();
        //targets = GameObject.FindObjectsOfType<ViewOption>();
    }
    private void Start()
    {
        //ChangeTarget(CurrentView);
    }

    public void ChangeCar(CarController car)
    {
        targets =car.gameObject.transform.Find("CameraPos").GetComponentsInChildren<ViewOption>();
        CarCameraSystem.instance.thirdCamPos = car.transform.Find("CameraPos").Find("Third").gameObject;

        ChangeTarget(Views.FirstPerson);
        CurrentView = Views.FirstPerson;
    }

    private void Update()
    {
        if (Input.GetKeyDown(ChangeViewKey))
        {
            if((int)CurrentView == ViewsCount-1)
            {
                CurrentView = 0;
            }
            else
            {
                CurrentView++;
            }
            ChangeTarget(CurrentView);
        }
    }
    private void ChangeTarget(Views option)
    {
        for(int i = 0; i < targets.Length; i++)
        {
            if (targets[i].option == option)
            {
                VCam.Follow = targets[i].transform;
                VCam.LookAt = targets[i].transform;
                return;
            }
        }
        Debug.LogError($"Can't find '{option}' ");
    }
}
