using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraHandle : MonoBehaviour
{
    public Transform targetTransform;
    public Transform cameraTransform;
    public Transform cameraPivotTransform;
    private Transform mytransform;
    
    private Vector3 cameraTransformPosition;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    private LayerMask ignoreLayers;
    private Cinemachine.CinemachineImpulseSource impulse;

    public static CameraHandle Instance;


    public float lookSpeed = 0.1f;
    public float followSpeed = 0.1f;
    public float pivotSpeed = 0.03f;

    private float defaultPosition;
    private float targetPosition;
    private float lookAngle;
    private float pivotAngel;
    public float minimumPivot = -35;
    public float maximumPivot = 35;

    public float cameraSphereRadius = 0.2f;
    public float cameraCollisionOffSet = 0.2f;
    public float minimumCollisionOffSet = 0.2f;

    
    private void Awake()
    {
        Instance = this;
        mytransform = this.transform;
        defaultPosition = cameraTransform.localPosition.z;
        ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
        impulse = GetComponent<Cinemachine.CinemachineImpulseSource>();
    }

    public void FollowTarget(float delta)
    {
        Vector3 targetPosition = Vector3.SmoothDamp(mytransform.position, targetTransform.position, ref cameraFollowVelocity, delta / followSpeed);
        mytransform.position = targetPosition + new Vector3(0,0.1f,0);

        //HandleCameraCollisions(delta);
    }

    public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
    {
        lookAngle += (mouseXInput * lookSpeed) / delta;
        pivotAngel -= (mouseYInput * pivotSpeed) / delta;
        pivotAngel = Mathf.Clamp(pivotAngel, minimumPivot, maximumPivot);

        Vector3 rotation = Vector3.zero;
        rotation.y = lookAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        mytransform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngel;

        targetRotation = Quaternion.Euler(rotation);
        cameraPivotTransform.localRotation = targetRotation;
    }

    private void HandleCameraCollisions(float delta)
    {
        targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetPosition), ignoreLayers))
        {
            float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
            targetPosition = -(dis - cameraCollisionOffSet);
            if (Mathf.Abs(targetPosition) < minimumCollisionOffSet)
            {
                targetPosition = -minimumCollisionOffSet;
            }

            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
            cameraTransform.localPosition = cameraTransformPosition;
        }

        

    }

    public void CameraShake()
    {
        impulse.GenerateImpulse();
    }

    
}


