using UnityEngine;

public class LitText : MonoBehaviour
{
    public TextMesh Text = null;
    private KMGameInfo _gameInfo = null;

    private static bool _lightState = false;

    private void Awake()
    {
        _gameInfo = GetComponentInParent<KMGameInfo>();
        _gameInfo.OnLightsChange += OnLightsChange;
        _gameInfo.OnStateChange += OnStateChange;
        OnLightsChange(_lightState);
    }

    private void OnDestroy()
    {
        _gameInfo.OnLightsChange -= OnLightsChange;
        _gameInfo.OnStateChange -= OnStateChange;
    }

    private void OnLightsChange(bool on)
    {
        Text.GetComponent<MeshRenderer>().enabled = on;
        _lightState = on;
    }


    private void OnStateChange(KMGameInfo.State state)
    {
        switch (state)
        {
            case KMGameInfo.State.Gameplay:
                break;

            default:
                _lightState = false;
                break;
        }
    }
}
