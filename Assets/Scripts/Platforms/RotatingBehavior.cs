using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class RotatingBehavior : MonoBehaviour
{
    [Header("Attribute")]

    public float angleSpeed;
    public bool shouldRotate;
    [SerializeField] bool shouldTurnBack;
    [SerializeField] float timeToWait;
    [SerializeField] float timeOnRotation;
    float timeWaitCoolDown;
    float timeRotaCoolDown;

    [Header("Reference")]

    [Tooltip("Object reference that we are going to rotate")]
    public GameObject rotatingObject;
    [SerializeField] Quaternion desiredAngle;
    [SerializeField] Quaternion startingAngle;
    Rigidbody rotatingRb;

    // Start is called before the first frame update
    void Start()
    {
        timeWaitCoolDown = timeToWait;
        timeRotaCoolDown = 0;
        shouldTurnBack = false;
        startingAngle = rotatingObject.transform.rotation;
        rotatingRb = rotatingObject.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        if (shouldRotate)
        {
            if (timeRotaCoolDown > 0)
            {
                timeRotaCoolDown -= Time.deltaTime;
            }
            else
            {
                timeRotaCoolDown = 0;
                Rotate();
            }
        }
        else if (!shouldRotate && rotatingObject.transform.rotation != startingAngle)
        {
            timeRotaCoolDown = timeRotaCoolDown != 0 ? 0 : timeRotaCoolDown; 
            if (timeWaitCoolDown > 0)
            {
                timeWaitCoolDown -= Time.deltaTime;
            }
            else
            {
                ResetRotation();
            }
        }
        else
        {
            if (timeRotaCoolDown != 0) timeRotaCoolDown = 0;
            if (timeWaitCoolDown != timeToWait) timeWaitCoolDown = timeToWait;
            shouldTurnBack = false;
        }
    }

    #region Rotating Methods

    /// <summary>
    /// This method rotates the platform.
    /// </summary>
    private void Rotate()
    {
        if (timeWaitCoolDown != timeToWait) timeWaitCoolDown = timeToWait;
        Quaternion realAngle = shouldTurnBack ? startingAngle : desiredAngle;
        var actualSpeed = angleSpeed * Time.deltaTime;
        rotatingRb.MoveRotation(Quaternion.RotateTowards(rotatingObject.transform.rotation, realAngle, actualSpeed));

        if (rotatingObject.transform.rotation == desiredAngle && !shouldTurnBack)
        {
            shouldTurnBack = true;
            timeRotaCoolDown = timeOnRotation;
        }
        else if (rotatingObject.transform.rotation == startingAngle && shouldTurnBack)
        {
            shouldTurnBack = false;
            timeRotaCoolDown = timeOnRotation;
        }
    }

    /// <summary>
    /// Reset the platform to its default orientation
    /// </summary>
    private void ResetRotation()
    {
        var actualSpeed = angleSpeed * Time.deltaTime;
        rotatingRb.MoveRotation(Quaternion.RotateTowards(rotatingObject.transform.rotation, startingAngle, actualSpeed));
    }

    #endregion

    #region Utils

    public void SetRotatingStart(bool start)
    {
        shouldRotate = start;
    }

    #endregion
}
