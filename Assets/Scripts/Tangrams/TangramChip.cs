using System;
using System.Linq;
using UnityEngine;

public class TangramChip : MonoBehaviour
{
    public string Type = null;

    public Renderer Underlay = null;

    [HideInInspector]
    public string Code = null;

    [HideInInspector]
    public Texture UnderlayTexture = null;

    public KMSelectable Selectable
    {
        get
        {
            return GetComponentInChildren<KMSelectable>();
        }
    }

    public event Action<int> OnPinInteract = null;

    private TextMesh _codeText = null;

    private void Awake()
    {
        _codeText = GetComponentInChildren<TextMesh>();

        KMSelectable[] pinSelectables = GetComponentsInChildren<KMSelectable>().Where((x) => x != Selectable).ToArray();

        for (int pinIndex = 0; pinIndex < pinSelectables.Length; ++pinIndex)
        {
            int closurePinIndex = pinIndex;
            pinSelectables[closurePinIndex].OnInteract += delegate()
            {
                pinSelectables[closurePinIndex].AddInteractionPunch(0.4f);
                OnPinInteract(closurePinIndex);
                return false;
            };
        }
    }

    private void Start()
    {
        _codeText.text = string.Format("{0}\n{1}", Type, Code);
        name = string.Format("{0}: {1}", Type, Code);

        Underlay.material.mainTexture = UnderlayTexture;
    }
}
