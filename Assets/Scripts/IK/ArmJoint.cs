using UnityEngine;
/// <summary>
/// This is a data block for every joint in an arm
/// </summary>
public class ArmJoint : MonoBehaviour
{
    [Tooltip("Set one of rotation axis to more than 0. Default is z-axis")]
    public Vector3 RotationAxis;

    [Header("Limit joint angle")]
    public float MinAngle;
    public float MaxAngle;
    [HideInInspector]
    public Vector3 StartOffset;
    private Transform _transform;

    //Returns the axis for the IK controller to use
    [HideInInspector]
    public char _rotationAxis
    {
        get
        {
            if (RotationAxis.x > 0)
            {
                return 'x';
            }
            else if (RotationAxis.y > 0)
            {
                return 'y';
            }
            else
            {
                return 'z';
            }
        }
        private set { }
    }

    /// <summary>
    /// Initializes the data block
    /// </summary>
    private void Awake()
    {
        _transform = this.transform;
        StartOffset = _transform.localPosition;
    }
}