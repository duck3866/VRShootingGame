using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grab : MonoBehaviour
{
    private bool _isGrabbing = false;
    private GameObject _grabbedObject;
    public LayerMask grabbedLayer;
    public float grabRange = 0.2f;
    private Vector3 prevPos;
    public float throwPower = 10f;
    private Quaternion prevRot;
    public float rotPower = 5f;
    public bool isRemoteGrab = true;
    public float remoteGrabDistance = 20f;
    
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (_isGrabbing == false)
        {
            TryGrab();
        }
        else
        {
            TryUngrab();
        }
    }

    private void TryGrab()
    {
        if (ARAVRInput.GetDown(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.RTouch))
        {
            if (isRemoteGrab)
            {
                Ray ray = new Ray(ARAVRInput.RHandPosition, ARAVRInput.RHandDirection);
                RaycastHit hitInfo;
                if (Physics.SphereCast(ray, 0.5f, out hitInfo, remoteGrabDistance))
                {
                    _isGrabbing = true;
                    _grabbedObject = hitInfo.transform.gameObject;
                    StartCoroutine(GrabbingAnimation());
                }

                return;
            }
            Collider[] hitObjects = Physics.OverlapSphere(ARAVRInput.RHandPosition, grabRange, grabbedLayer);
            int closest = 0;
            for (int i = 0; i < hitObjects.Length; i++)
            {
                Vector3 closestPos = hitObjects[closest].transform.position;
                float closestDistance = Vector3.Distance(closestPos, hitObjects[i].transform.position);
                Vector3 nextPos = hitObjects[closest].transform.position;
                float nextDistance = Vector3.Distance(nextPos, hitObjects[i].transform.position);

                if (nextDistance < closestDistance)
                {
                    closest = i;
                }
            }

            if (hitObjects.Length > 0)
            {
                _isGrabbing = true;
                _grabbedObject = hitObjects[closest].gameObject;
                _grabbedObject.transform.parent = ARAVRInput.RHand;
                
                _grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                
                prevPos = ARAVRInput.RHandPosition;
                prevRot = ARAVRInput.RHand.rotation;
            }
        }
    }

    void TryUngrab()
    {
        Vector3 throwDirection = (ARAVRInput.RHandPosition - prevPos);
        prevPos = ARAVRInput.RHandPosition;
        
        Quaternion deltaRotation = ARAVRInput.RHand.rotation * Quaternion.Inverse(prevRot);
        prevRot = ARAVRInput.RHand.rotation;
        
        if (ARAVRInput.GetUp(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.RTouch))
        {
            _isGrabbing = false;
            _grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
            _grabbedObject.transform.parent = null;
            _grabbedObject.GetComponent<Rigidbody>().velocity = throwDirection * throwPower;
            float angle;
            Vector3 axis;
            deltaRotation.ToAngleAxis(out angle, out axis);
            Vector3 angularVelocity = (1.0f / Time.deltaTime) * angle * axis;
            _grabbedObject.GetComponent<Rigidbody>().angularVelocity = angularVelocity;

            _grabbedObject = null;
        }
    }

    IEnumerator GrabbingAnimation()
    {
        float currentTime = 0;
        float finishTime = 0.2f;
        _grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
        prevPos = ARAVRInput.RHandPosition;
        prevRot = ARAVRInput.RHand.rotation;
        Vector3 startLocation = _grabbedObject.transform.position;
        Vector3 targetLocation = ARAVRInput.RHandPosition + ARAVRInput.RHandDirection * 0.1f;

        float elapsedRate = currentTime / finishTime;
        while (elapsedRate < 1)
        {
            currentTime += Time.deltaTime;
            elapsedRate = currentTime / finishTime;
            _grabbedObject.transform.position = Vector3.Lerp(startLocation, targetLocation, elapsedRate);
            yield return null;
        }

        _grabbedObject.transform.position = targetLocation;
        _grabbedObject.transform.parent = ARAVRInput.RHand;
    }
}
