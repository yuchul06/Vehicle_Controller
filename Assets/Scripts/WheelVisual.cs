using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelVisual : MonoBehaviour
{
    [SerializeField]
    private GameObject[] wheelMesh;

    private CarController carController;

    // Start is called before the first frame update
    void Start()
    {
        carController = GetComponent<CarController>();

        wheelMesh = new GameObject[gameObject.transform.Find("wheel").childCount];
        for (int i = 0; i < gameObject.transform.Find("wheel").childCount; i++)
        {
            wheelMesh[i] = gameObject.transform.Find("wheel").GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            carController.wheelColliders[i].GetWorldPose(out Vector3 wheelPosition, out Quaternion wheelRotation);
            wheelMesh[i].transform.SetPositionAndRotation(wheelPosition, wheelRotation);
        }
    }
}
