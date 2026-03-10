using UnityEngine;

[ExecuteAlways]
public class BendControllerRadial : MonoBehaviour
{
    [Header("Bend Settings")]
    public bool bendOn = true;

    [Tooltip("How strong the world curvature is")]
    public float curvature = 40f;

    [Range(0.5f, 2f)]
    public float xScale = 1f;

    [Range(0.5f, 2f)]
    public float zScale = 1f;

    public float flatMargin = 0f;

    [Header("Horizon Waves")]
    public bool horizonWaves = false;

    [Range(0f, 10f)]
    public float horizonWaveFrequency = 0f;

    int curveOriginID;
    int referenceDirectionID;
    int curvatureID;
    int scaleID;
    int flatMarginID;
    int horizonWaveFrequencyID;

    Vector3 scale;

    void OnEnable()
    {
        curveOriginID = Shader.PropertyToID("_CurveOrigin");
        referenceDirectionID = Shader.PropertyToID("_ReferenceDirection");
        curvatureID = Shader.PropertyToID("_Curvature");
        scaleID = Shader.PropertyToID("_Scale");
        flatMarginID = Shader.PropertyToID("_FlatMargin");
        horizonWaveFrequencyID = Shader.PropertyToID("_HorizonWaveFrequency");
    }

    void Update()
    {
        scale.x = xScale;
        scale.z = zScale;

        if (horizonWaves)
            Shader.EnableKeyword("HORIZON_WAVES");
        else
            Shader.DisableKeyword("HORIZON_WAVES");

        if (bendOn)
            Shader.EnableKeyword("BEND_ON");
        else
            Shader.DisableKeyword("BEND_ON");

        Shader.SetGlobalVector(curveOriginID, transform.position);
        Shader.SetGlobalVector(referenceDirectionID, transform.forward);
        Shader.SetGlobalFloat(curvatureID, curvature * 0.00001f);
        Shader.SetGlobalVector(scaleID, scale);
        Shader.SetGlobalFloat(flatMarginID, flatMargin);
        Shader.SetGlobalFloat(horizonWaveFrequencyID, horizonWaveFrequency);
    }

    void OnDisable()
    {
        Shader.SetGlobalFloat(curvatureID, 0);
    }
}