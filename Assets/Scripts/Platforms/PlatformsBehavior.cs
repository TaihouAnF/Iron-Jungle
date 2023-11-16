using System.Collections;
using UnityEngine;

public enum PlatformType
{
    Normal,
    Other,
}

public class PlatformsBehavior : MonoBehaviour
{
    public PlatformType type;
    public bool isBreakable;
    public bool isMovable;
    public bool isRotatable;
    [Tooltip("Indicate whether this platform should activate its own script at the start or after being latched")]
    public bool shouldActive;
    public bool isLatched;
    public bool isValid;

    void Update()
    {
        if (shouldActive) TypeHandler(true);
        else TypeHandler(isLatched);
    }

    public void SetValid(bool valid)
    {
        isValid = valid;
    }

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
