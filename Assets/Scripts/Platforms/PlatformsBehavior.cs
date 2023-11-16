using System.Collections;
using UnityEngine;

public enum PlatformType
{
    Normal,
    Breakable,
    Uneven,
    Moving,
    Rotating
}

public class PlatformsBehavior : MonoBehaviour
{
    public PlatformType type;
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
        switch(type)
        {
            case PlatformType.Breakable:
                GetComponent<BreakingBehavior>().SetBreakingStart(start);
                break;
            case PlatformType.Moving:
                GetComponent<MovingBehavior>().SetMovingStart(start);
                break;
            case PlatformType.Rotating: 
                GetComponent<RotatingBehavior>().SetRotatingStart(start);
                break;
            default:
                break;
        }
    }
}
