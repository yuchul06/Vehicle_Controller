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


    [Header("Status")]
    [SerializeField]
    private AutoGear _aGear;
    [SerializeField]
    private float _steerAngle;
    [SerializeField]
    private float _motorTorque;
    [SerializeField]
    private float _brakeTorque;
    [SerializeField]
    private float _speed;
    [SerializeField]
    [Range(0.001f,1f)]
    private float _steerSpeed;

    [Header("Interior Anim")]
    [SerializeField]
    private GameObject _steerModel;
    [SerializeField]
    private float _maxSteerModelAngle;


    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI _speedText;
    [SerializeField]
    private bool _useRound;


    private bool canFlip = true;


    private Rigidbody _rigidbody;
    private void Awake()
    {
        wheelMesh = GameObject.FindGameObjectsWithTag("WheelMesh");
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = GameObject.Find("CenterOfMess").transform.localPosition;
        

        for(int i =0; i < wheelMesh.Length; i++)
        {
            if(wheelMesh[i].name.Contains("FR") || wheelMesh[i].name.Contains("RR"))
            {
                wheelMesh[i].transform.localScale = new Vector3
                    (-wheelMesh[i].transform.localScale.x, wheelMesh[i].transform.localScale.y, wheelMesh[i].transform.localScale.z);
            }
        }

        canFlip = true;
    }
    private void FixedUpdate()
    {

        for (int i = 0; i < 4; i++)
        {
            wheelColliders[i].GetWorldPose(out Vector3 wheelPosition, out Quaternion wheelRotation);
            wheelMesh[i].transform.SetPositionAndRotation(wheelPosition, wheelRotation);
        }
        //_speed = (Mathf.Abs((_rigidbody.velocity.x + _rigidbody.velocity.y + _rigidbody.velocity.z) / 3)) * 20;
        _speed = _rigidbody.velocity.magnitude * 3f;
        if(_speedText != null)
        {
            if (_useRound)
            {
                _speedText.text = ((int)_speed).ToString() + "KM/H";
            }
            else
            {
                _speedText.text = _speed.ToString() + "KM/H";
            }
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (_aGear != AutoGear.D)
            {
                _aGear++;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (_aGear != AutoGear.P)
            {
                _aGear--;
            }
        }
        if (Input.GetKeyDown(KeyCode.R) && canFlip)
        {
            Flip();
        }
        
        Accel();
        Steer();
        Move(_steerAngle, _motorTorque, _brakeTorque);
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
                Debug.Log(TorqueCurve.Evaluate((_speed) / _maxSpeed));
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
            _motorTorque = 0;
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
                    _steerAngle = _steerAngle + Mathf.Clamp(_speed / 35, 0, Mathf.Abs(_steerAngle) / _currentMaxSteerAngle*1.5f) > 0 ? 0 : _steerAngle + Mathf.Clamp(_speed / 35, 0, Mathf.Abs(_steerAngle) / _currentMaxSteerAngle*1.5f);
                }
                else
                {
                    _steerAngle = _steerAngle - Mathf.Clamp(_speed / 35, 0, Mathf.Abs(_steerAngle) / _currentMaxSteerAngle*1.5f) < 0 ? 0 : _steerAngle - Mathf.Clamp(_speed / 35, 0, Mathf.Abs(_steerAngle) / _currentMaxSteerAngle*1.5f);
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
                Mathf.Lerp(0, Mathf.Lerp(1, _maxSteerModelAngle/2, _currentMaxSteerAngle/_maxSteerAngle), Math.Abs(_steerAngle) / _currentMaxSteerAngle));
            }
            else if(_steerAngle > 0)
            {
                _steerModel.transform.localRotation = Quaternion.Euler(0, 0,
                Mathf.Lerp(0, -Mathf.Lerp(1, _maxSteerModelAngle/2, _currentMaxSteerAngle / _maxSteerAngle), Math.Abs(_steerAngle) / _currentMaxSteerAngle));
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
