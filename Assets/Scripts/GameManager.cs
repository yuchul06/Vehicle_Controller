using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;
    public CarController CurrentCar = null;
    public CarController[] Cars;
    public KeyCode ChangeCarKey = KeyCode.Tab;
    private int _currentCarIndex = 0;
    public int FrameLimit = 60;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("이미 있음");
            Destroy(gameObject);
        }
        
    }
    private void Start()
    {
        Application.targetFrameRate = FrameLimit;
        Cars = FindObjectsOfType<CarController>();
        _currentCarIndex = -1;
        ChangeCar();
    }

    private void Update()
    {
        if (Input.GetKeyDown(ChangeCarKey))
        {
            ChangeCar();
        }
    }
    [ContextMenu("차량 변경")]
    public void ChangeCar()
    {
        if (_currentCarIndex == Cars.Length - 1)
        {
            CurrentCar = Cars[0];
            _currentCarIndex = 0;
        }
        else
        {
            CurrentCar = Cars[_currentCarIndex + 1];
            _currentCarIndex++;
        }
        
        for(int i = 0; i < Cars.Length; i++)
        {
            if (i != _currentCarIndex)
            {
                Cars[i].enabled = false;
            }
            else
            {
                Cars[i].enabled = true;
            }
        }

        CurrentCar.UpdateGearText();
        CurrentCar.UpdateRingFilled();
        CurrentCar.UpdateSpeedText();
        VCamSetting.instance.ChangeCar(CurrentCar);

    }
}
