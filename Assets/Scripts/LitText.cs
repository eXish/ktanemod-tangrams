using UnityEngine;

public class LitText : MonoBehaviour
{
    public TextMesh Text = null;
    private KMGameInfo _gameInfo = null;

    private bool _lightState = true;

    private void Awake()
    {
        _gameInfo = GetComponentInParent<KMGameInfo>();
        _gameInfo.OnLightsChange += OnLightsChange;
        OnLightsChange(_lightState);
    }

    private void OnDestroy()
    {
        _gameInfo.OnLightsChange -= OnLightsChange;
    }

    private void OnLightsChange(bool on)
    {
        Text.GetComponent<MeshRenderer>().enabled = on;
        _lightState = on;
    }
}
