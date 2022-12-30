using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class Skid : MonoBehaviour
{
    private CarController carController;
    private WheelCollider wCol;
    private TrailRenderer trailRenderer;

    public bool isSlip;

    public float minSlip = 0.2f;
    void Start()
    {
        carController = transform.parent.parent.parent.GetComponent<CarController>();
        wCol = transform.parent.GetComponent<WheelCollider>();
        trailRenderer = GetComponent<TrailRenderer>();

        wCol.GetWorldPose(out Vector3 pos, out Quaternion q);
        transform.rotation = new Quaternion(90, 0, 0, 90);
        transform.localPosition = new Vector3(0, -(wCol.radius+0.2f) ,0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log($"{transform.parent} |RPM : {wCol.rpm}, Speed : {carController.Speed}, radius * rpm * 0.1f : {(wCol.radius) * wCol.rpm * 0.1f}");
        
        wCol.GetGroundHit(out WheelHit hit);
        isSlip = wCol.isGrounded && carController.Speed > 1f && (Mathf.Abs(hit.sidewaysSlip) > minSlip || carController.Speed + 2.5f < Mathf.Abs((wCol.radius) * wCol.rpm * 0.1f));
        trailRenderer.emitting = isSlip;
    }
}
