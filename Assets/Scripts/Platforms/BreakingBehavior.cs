using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class BreakingBehavior : MonoBehaviour
{
    [Header("Attributes")]

    [ReadOnly] public bool breaking;
    [ReadOnly] public bool startBreaking;

    [Tooltip("How long would this object break")]
    public float breakingTime;
    public float recoverTime;

    [Header("Shake Attributes")]

    [Tooltip("Speed of a shake, will start 0.1% of its maxShakeFrequency will increase over time until the max is reached")]
    public float maxShakeFrequency;
    [Tooltip("Max distance to oscillate")]
    public float shakeAmplitude;
    public AnimationCurve frequencyScale;

    [Header("Fade Attributes")]

    public bool doFades;
    public float fadeOutTime;
    public float fadeInTime;
    public AnimationCurve fadeOutCurve;
    public AnimationCurve fadeInCurve;


    [Header("Reference")]

    [Tooltip("Object reference that we are going to break")]
    [SerializeField] GameObject _breakingObject;
    [SerializeField] GameObject _mesh;

    Material _material;

    /// <summary>
    /// Method <c>Awake</c> is called when the object is initialized.
    /// </summary>
    void Awake()
    {
        _material = _mesh.GetComponent<MeshRenderer>().material;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Tests. Make sure we have the correct hierarchy.
        Debug.Assert(transform.GetChildsWhere((childT) => (childT == _breakingObject.transform)).Count == 1,
        $"{_breakingObject.name} must be a transfom child of {this.gameObject.name}");

        Debug.Assert(_breakingObject.transform.GetChildsWhere((childT) => (childT == _mesh.transform)).Count == 1,
        $"{_mesh.name} must be a transfom child of {_breakingObject.name}");
    }

    #region Breaking Methods

    public void SetBreakingStart(bool start)
    {
        startBreaking = start;
        if (startBreaking && !breaking)
        {
            StartBreaking();
        }

    }

    /// <summary>
    /// This method enables the performing of breaking behavior.
    /// </summary>
    public void StartBreaking()
    {
        startBreaking = true;
        StartCoroutine(Breaking());
    }

    /// <summary>
    /// This method performs breaking behavior.
    /// </summary>
    /// <returns>null, it is used for fetch the current status and continue on the next call</returns>
    private IEnumerator Breaking()
    {
        breaking = true;
        float normTime = 0f;

        //shake
        float initalY = _mesh.transform.position.y;
        while (normTime < 1.0f)
        {
            float frequency = frequencyScale.Evaluate(normTime) * maxShakeFrequency;
            float y = initalY + shakeAmplitude *
            Mathf.Sin(frequency * normTime * 10);
            Vector3 newPos = new Vector3(_mesh.transform.position.x, y, _mesh.transform.position.z);

            _mesh.transform.position = newPos;

            normTime += Time.deltaTime / breakingTime;
            yield return null;
        }

        if (doFades)
        {
            //Fade out
            normTime = 0;
            GetComponent<PlatformsBehavior>().SetValid(false);
            while (normTime < 1.0f)
            {
                SetTransparency(1 - fadeOutCurve.Evaluate(normTime));

                normTime += Time.deltaTime / fadeOutTime;
                yield return null;
            }
            _breakingObject.SetActive(false);

            yield return new WaitForSeconds(recoverTime);


            //Fade in
            normTime = 0;
            SetTransparency(0);

            GetComponent<PlatformsBehavior>().SetValid(true);
            _breakingObject.SetActive(true);

            while (normTime < 1.0f)
            {
                SetTransparency(fadeInCurve.Evaluate(normTime));

                normTime += Time.deltaTime / fadeInTime;
                yield return null;
            }
        }
        else
        {
            GetComponent<PlatformsBehavior>().SetValid(false);
            _breakingObject.SetActive(false);

            yield return new WaitForSeconds(recoverTime);

            GetComponent<PlatformsBehavior>().SetValid(true);
            _breakingObject.SetActive(true);
        }

        breaking = false;
        startBreaking = false;
    }

    /// <summary>
    /// This method set the transparency of the mesh
    /// </summary>
    /// <param name="alpha"><c>alpha</c> is a float representing the current alpha value of the mesh.</param>
    public void SetTransparency(float alpha)
    {
        if (_material.color != null)
        {
            Color newColor = _material.color;
            newColor.a = Mathf.Clamp01(alpha);
            _material.color = newColor;
        }
    }

    #endregion
}
