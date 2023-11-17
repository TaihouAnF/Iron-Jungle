using System.Collections;
using UnityEngine;

/// <summary>
/// An Enumeration of platform types
/// </summary>
public enum PlatformType
{
    Normal,
    Other,
}

/// <summary>
/// <c>PlatformsBehavior</c> controls platforms behaviors and overall attributes.
/// </summary>
public class PlatformsBehavior : MonoBehaviour
{
    [Header("Attributes")]

    public PlatformType type;
    public bool isBreakable;
    public bool isMovable;
    public bool isRotatable;
    [Tooltip("Indicate whether this platform should activate its own script at the start or after being latched")]
    public bool shouldActive;
    public bool isLatched;
    public bool isValid;

    // Update is called once per frame
    void Update()
    {
        // If the platform should activate at the start of the game
        if (shouldActive) TypeHandler(true);
        else TypeHandler(isLatched);
    }

    /// <summary>
    /// Set platform to be valid or not
    /// </summary>
    /// <param name="valid">A boolean indicate the platform should be valid or not.</param>
    public void SetValid(bool valid)
    {
        isValid = valid;
    }

    /// <summary>
    /// Handles type of behaviors of the platform, if a platform belongs multiple
    /// types of behaviors, it can perform all of them.
    /// </summary>
    /// <param name="start"><c>start</c> is a boolean representing 
    /// whether the platform should start performing behaviors.</param>
    private void TypeHandler(bool start)
    {
        if (isValid)
        {
            if (isBreakable)
            {
                GetComponent<BreakingBehavior>().SetBreakingStart(start);
            }

            if (isMovable)
            {
                GetComponent<MovingBehavior>().SetMovingStart(start);
            }

            if (isRotatable)
            {
                GetComponent<RotatingBehavior>().SetRotatingStart(start);
            }
        }
    }
}
