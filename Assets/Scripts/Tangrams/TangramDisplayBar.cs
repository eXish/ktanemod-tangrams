using UnityEngine;

public class TangramDisplayBar : MonoBehaviour
{
    public float Progress
    {
        get;
        set;
    }

    private MeshRenderer _renderer = null;
    private MaterialPropertyBlock _propertyBlock = null;
    private int _progressPropertyID = 0;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _progressPropertyID = Shader.PropertyToID("_Progress");
        _propertyBlock = new MaterialPropertyBlock();
    }

    private void Update()
    {
        _propertyBlock.SetFloat(_progressPropertyID, Progress);
        _renderer.SetPropertyBlock(_propertyBlock);
    }
}
