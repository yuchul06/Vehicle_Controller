using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalType;
using System;
using UnityEngine.UI;
using TMPro;

public class CarController : MonoBehaviour
{
    [SerializeField] 
    private List<WheelCollider> wheelColliders = new List<WheelCollider>();

    [SerializeField]
    private GameObject[] wheelMesh;

    [Header("Property")]
    [SerializeField]
    private float _maxSteerAngle;
    [SerializeField]
    private float _minSteerAngle;
    [SerializeField]
    private float _currentMaxSteerAngle;
    [SerializeField]
    private float _maxMotorTorque;
    [SerializeField]
    private float _maxSpeed;
    [SerializeField]
    private float _maxBrakeTorque;
    [SerializeField]
    private DriveType _driveType;
    [SerializeField]
    private AnimationCurve TorqueCurve;
    [SerializeField]
    [Range(0.05f,1f)]
    private float _steerSpeed;


    [Header("\nStatus")]
    [SerializeField]
    private AutoGear _aGear;
    public AutoGear AGear
    {
        get => _aGear;
    }
    [SerializeField]
    private float _steerAngle;
    [SerializeField]
    private float _motorTorque;
    [SerializeField]
    private float _brakeTorque;
    [SerializeField]
    private float _speed;
    
    [Header("\nInterior Anim")]
    [SerializeField]
    private GameObject _steerModel;
    [SerializeField]
    private float _steerModelAngle = 900;

    [Header("\nUI")]
    [SerializeField]
    private TextMeshProUGUI _gearText = null;
    [SerializeField]
    private Image _ringImage = null;
    [SerializeField]
    private TextMeshProUGUI _speedText = null;
    [SerializeField]
    private bool _useRound = true;


    private bool canFlip = true;


    private Rigidbody _rigidbody;
    private void Awake()
    {
        wheelMesh = new GameObject[gameObject.transform.Find("wheel").childCount];
        for(int i = 0; i < gameObject.transform.Find("wheel").childCount; i++)
        {
            wheelMesh[i] = gameObject.transform.Find("wheel").GetChild(i).gameObject;
        }

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = GameObject.Find("CenterOfMess").transform.localPosition;

        

        //for(int i =0; i < wheelMesh.Length; i++)
        //{
        //    if(wheelMesh[i].name.Contains("FR") || wheelMesh[i].name.Contains("RR"))
        //    {
        //        wheelMesh[i].transform.localScale = new Vector3
        //            (-wheelMesh[i].transform.localScale.x, wheelMesh[i].transform.localScale.y, wheelMesh[i].transform.localScale.z);
        //    }
        //}

        canFlip = true;

        _speedText = GameObject.FindGameObjectWithTag("SpeedUI").GetComponent<TextMeshProUGUI>();
        _gearText = GameObject.FindGameObjectWithTag("GearUI").GetComponent<TextMeshProUGUI>();
        _ringImage = GameObject.FindGameObjectWithTag("GaugeRingUI").GetComponent<Image>();

        UpdateGearText();
    }
    private void FixedUpdate()
    {

        for (int i = 0; i < 4; i++)
        {
            wheelColliders[i].GetWorldPose(out Vector3 wheelPosition, out Quaternion wheelRotation);
            wheelMesh[i].transform.SetPositionAndRotation(wheelPosition, wheelRotation);
        }

        _speed = _rigidbody.velocity.magnitude * 4f;

        UpdateSpeedText();
        UpdateRingFilled();
    }
    #region UI
    private void UpdateSpeedText()
    {
        if (_speedText != null)
        {
            if (_useRound)
            {
                _speedText.text = ((int)_speed).ToString();
            }
            else
            {
                _speedText.text = _speed.ToString();
            }
        }
    }
    private void UpdateRingFilled()
    {
        if(_ringImage != null)
        {
            _ringImage.fillAmount = _speed / _maxSpeed;
        }
    }
    private void UpdateGearText()
    {
        if(_gearText != null)
        {
            _gearText.text = _aGear.ToString();
        }
    }
    #endregion
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && canFlip)
        {
            Flip();
        }

        Gear();
        Accel();
        Steer();
        Move(_steerAngle, _motorTorque, _brakeTorque);
    }

    private void Gear()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.JoystickButton5))
        {
            if (_aGear != AutoGear.D)
            {
                _aGear++;
                UpdateGearText();
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.JoystickButton4))
        {
            if (_aGear != AutoGear.P)
            {
                _aGear--;
                UpdateGearText();
            }
        }
    }

    private void Accel()
    {
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            //for(int i = 0; i < wheelColliders.Count; i++)
            //{
            //    wheelColliders[i].motorTorque = _motorTorque * Input.GetAxis("Vertical");
            //}
            if (_speed < _maxSpeed)
            {
                _motorTorque = _maxMotorTorque * Input.GetAxis("Vertical") * Mathf.Lerp(1, 0.1f, TorqueCurve.Evaluate((_speed)/_maxSpeed));
            }
            else
            {
                _motorTorque = 0;
            }
        }
        else if (Input.GetAxisRaw("Vertical") < 0)
        {
            _brakeTorque = Mathf.Abs(_maxBrakeTorque * Input.GetAxis("Vertical"));
        }
        else
        {
            _motorTorque = _maxMotorTorque / 850;
            _brakeTorque = 0;
        }
    }

    private void Steer()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            _steerAngle += _steerSpeed * Input.GetAxis("Horizontal");
            _steerAngle = Mathf.Clamp(_steerAngle, -_currentMaxSteerAngle, _currentMaxSteerAngle);
        }
        else
        {
            if (_steerAngle != 0)
            {
                if (_steerAngle < 0)
                {
                    _steerAngle = _steerAngle + Mathf.Clamp(_speed / 50, 0, Mathf.Abs(_steerAngle) / _currentMaxSteerAngle*1.5f) > 0 ? 0 : _steerAngle + Mathf.Clamp(_speed / 50, 0, Mathf.Abs(_steerAngle) / _currentMaxSteerAngle*1.5f);
                }
                else
                {
                    _steerAngle = _steerAngle - Mathf.Clamp(_speed / 50, 0, Mathf.Abs(_steerAngle) / _currentMaxSteerAngle*1.5f) < 0 ? 0 : _steerAngle - Mathf.Clamp(_speed / 50, 0, Mathf.Abs(_steerAngle) / _currentMaxSteerAngle*1.5f);
                }
            }
        }
        SteerAnim();
        _currentMaxSteerAngle = Mathf.Lerp(_minSteerAngle, _maxSteerAngle, (_maxSpeed - _speed) / _maxSpeed);
    }

    private void SteerAnim()
    {
        if(_steerModel != null)
        {
            if(_steerAngle < 0)
            {
                _steerModel.transform.localRotation = Quaternion.Euler(0, 0,
                Mathf.Lerp(0, Mathf.Lerp(1, _steerModelAngle/2, _currentMaxSteerAngle/_maxSteerAngle), Math.Abs(_steerAngle) / _currentMaxSteerAngle));
            }
            else if(_steerAngle > 0)
            {
                _steerModel.transform.localRotation = Quaternion.Euler(0, 0,
                Mathf.Lerp(0, -Mathf.Lerp(1, _steerModelAngle/2, _currentMaxSteerAngle / _maxSteerAngle), Math.Abs(_steerAngle) / _currentMaxSteerAngle));
            }
            else
            {
                _steerModel.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            

            

        }
        else
        {
            return;
        }
    }

    public void Move(float steerAngle, float motorTorque, float brakeTorque)
    {
        for(int i = 0; i < 2; i++)
        {
            wheelColliders[i].steerAngle = steerAngle;
        }

        for (int i = 0; i < wheelColliders.Count; i++) //일반 브레이크 : 앞바퀴 제동 + 뒷바퀴 반만, 사이드 브레이크 : 뒷바퀴 제동
        {
            if (wheelColliders[i].name.Contains("FR")||wheelColliders[i].name.Contains("FL"))
            {
                wheelColliders[i].brakeTorque = brakeTorque;
            }
            else
            {
                wheelColliders[i].brakeTorque = brakeTorque / 2;
            }
        }

            
        if(_aGear == AutoGear.P)
        {
            for(int i = 0; i< wheelColliders.Count; i++)
            {
                wheelColliders[i].brakeTorque = _maxBrakeTorque / 10;
            }
        }

        int motorCount = 0;
        switch (_driveType)
        {
            case DriveType.FWD:
                motorCount = 2;
                break;
            case DriveType.RWD:
            case DriveType.AWD:
                motorCount = 4;
                break;
            default:
                break;
        }

        for(int i = 0; i < motorCount; i++)
        {
            if(_driveType == DriveType.RWD)
            {
                if(i < 2)
                {
                    continue;
                }
            }
            if (_aGear == AutoGear.R)
            {
                if (_driveType == DriveType.AWD)
                {
                    wheelColliders[i].motorTorque = -motorTorque/4;
                }
                else
                {
                    wheelColliders[i].motorTorque = -motorTorque;
                }
                
            }
            else if(_aGear == AutoGear.D)
            {
                if(_driveType == DriveType.AWD)
                {
                    wheelColliders[i].motorTorque = motorTorque/4;
                }
                else
                {
                    wheelColliders[i].motorTorque = motorTorque;
                }
            }
            else
            {
                wheelColliders[i].motorTorque = 0;
            }
            }
    }

    private void Flip()
    {
        StartCoroutine(FlipDelay());
        transform.position += new Vector3(0, 2.5f, 0);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

     IEnumerator FlipDelay()
    {
        canFlip = false;
        yield return new WaitForSeconds(2f);
        canFlip = true;
    }
    

  

    
}
