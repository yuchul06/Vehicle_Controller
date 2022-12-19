using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VCamSetting : MonoBehaviour
{
    public CinemachineVirtualCamera VCam;
    public ViewOption[] targets;

    public Views CurrentView = Views.FirstPerson;
    public KeyCode ChangeViewKey = KeyCode.C;
    private int ViewsCount
    {
        get => System.Enum.GetValues(typeof(Views)).Length;
    }

    private void OnEnable()
    {
        VCam = GetComponent<CinemachineVirtualCamera>();
        targets = GameObject.FindObjectsOfType<ViewOption>();
        
    }
    private void Start()
    {
        ChangeTarget(CurrentView);
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
        Debug.LogError("왜 없는데 시발련아");
    }
}
