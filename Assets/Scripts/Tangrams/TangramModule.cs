using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TangramModule : MonoBehaviour
{
    public TangramGrid[] GridPrefabs = null;
    public TangramPiece[] PiecePrefabs = null;

    [Range(1, 8)]
    public int RequiredInputCount = 3;

    public AudioClip TapSound = null;
    public AudioClip SmallPopSound = null;
    public AudioClip BigPopSound = null;
    public AudioClip ZapSound = null;

    private KMBombModule _bombModule = null;
    private KMAudio _audio = null;
    private KMSelectable _selectable = null;
    private Tangram _tangram = null;

    private TangramDisplayBar _displayBar = null;
    private ParticleSystem _smoke = null;

    private readonly List<TangramGridConnection> _previouslySelectedConnections = new List<TangramGridConnection>();
    private TangramGridConnection _selectedConnection = new TangramGridConnection() { PointA = null, PointB = null };

    private void Awake()
    {
        _bombModule = GetComponent<KMBombModule>();
        _audio = GetComponent<KMAudio>();
        _selectable = GetComponent<KMSelectable>();

        _displayBar = GetComponentInChildren<TangramDisplayBar>(true);
        _smoke = GetComponentInChildren<ParticleSystem>(true);

        _bombModule.GenerateLogFriendlyName();

        RandomizePuzzle();
    }

    private void RandomizePuzzle()
    {
        _tangram = GenerateTangram();
        _tangram.LogInfo(_bombModule);

        TangramChip chip = Instantiate(_tangram.Grid.Chip, transform);
        chip.UnderlayTexture = _tangram.Grid.UnderlayTexture;
        chip.Code = _tangram.ChipCode;

        KMSelectable chipSelectable = chip.Selectable;

        _selectable.ChildRowLength = chipSelectable.ChildRowLength;
        _selectable.Children = chipSelectable.Children;

        for (int contactPointIndex = 0; contactPointIndex < chipSelectable.transform.childCount; ++contactPointIndex)
        {
            KMSelectable contactPoint = chipSelectable.transform.GetChild(contactPointIndex).GetComponent<KMSelectable>();
            contactPoint.Parent = _selectable;
        }

        _selectable.DefaultSelectableIndex = chipSelectable.DefaultSelectableIndex;
        _selectable.UpdateChildren(_selectable.Children[_selectable.DefaultSelectableIndex]);

        chip.OnPinInteract += OnPinInteract;
    }

    private Tangram GenerateTangram()
    {
        while (true)
        { 
            TangramGrid grid = PickGrid();
            grid.Load();

            TangramPiece[] pieces = PickPieces(grid);

            Tangram tangram = new Tangram(grid, pieces);
            if (tangram.ValidInputCount >= RequiredInputCount)
            {
                return tangram;
            }
        }
    }

    private TangramGrid PickGrid()
    {
        return GridPrefabs.RandomPick();
    }

    private TangramPiece[] PickPieces(TangramGrid grid)
    {
        List<TangramPiece> chosenPieces = new List<TangramPiece>();
        List<TangramPiece> possiblePieces = new List<TangramPiece>();
        possiblePieces.AddRange(PiecePrefabs);

        foreach (TangramShape shape in grid.Cells)
        {
            TangramPiece piece = possiblePieces.Where(x => x.Shape == shape).RandomPick();
            possiblePieces.Remove(piece);
            chosenPieces.Add(piece);

            piece.Load();
        }

        return chosenPieces.ToArray();
    }

    private void OnPinInteract(int pinIndex)
    {
        _audio.PlaySoundAtTransform(TapSound.name, transform);

        if (_previouslySelectedConnections.Count >= RequiredInputCount)
        {
            return;
        }

        TangramGridConnectionPoint contactPoint = _tangram.Grid.ExternalConnections[pinIndex];

        if (_selectedConnection.PointA == null)
        {
            OnInteractInputContactPoint(contactPoint);
        }
        else if (_selectedConnection.PointB == null)
        {
            OnInteractOutputContactPoint(contactPoint);
        }
        else
        {
            //TODO!!
            _bombModule.LogFormat("Trying to select contact point {0}, but module is busy trying to overload the circuit. Strike.", _tangram.GetExternalConnectionIndex(contactPoint) + 1);
            _bombModule.HandleStrike();
        }
    }

    private void OnInteractInputContactPoint(TangramGridConnectionPoint contactPoint)
    {
        _bombModule.LogFormat("Selecting contact point {0} as input/positive.", _tangram.GetExternalConnectionIndex(contactPoint) + 1);

        if (_previouslySelectedConnections.Any((x) => x.PointA.Equals(contactPoint)))
        {
            _bombModule.LogFormat("Already used contact point {0} as a valid input/positive. Strike.", _tangram.GetExternalConnectionIndex(contactPoint) + 1);
            _bombModule.HandleStrike();
            return;
        }

        _selectedConnection.PointA = contactPoint;
    }

    private void OnInteractOutputContactPoint(TangramGridConnectionPoint contactPoint)
    {
        _bombModule.LogFormat("Selecting contact point {0} as output/negative.", _tangram.GetExternalConnectionIndex(contactPoint) + 1);

        _selectedConnection.PointB = contactPoint;
        if (_tangram.IsValidConnection(_selectedConnection))
        {
            ZapAndFinish(delegate()
            {
                _bombModule.LogFormat("{0}→{1} is a valid input/positive to output/negative.", _tangram.GetExternalConnectionIndex(_selectedConnection.PointA) + 1, _tangram.GetExternalConnectionIndex(_selectedConnection.PointB) + 1);

                _previouslySelectedConnections.Add(new TangramGridConnection() { PointA = _selectedConnection.PointA, PointB = _selectedConnection.PointB });
                if (_previouslySelectedConnections.Count >= RequiredInputCount)
                {
                    _bombModule.Log("Module defused!");
                    _audio.PlaySoundAtTransform(BigPopSound.name, transform);
                    _smoke.Play();
                    _bombModule.HandlePass();
                }
                else
                {
                    _audio.PlaySoundAtTransform(SmallPopSound.name, transform);
                }

                _selectedConnection.PointA = null;
                _selectedConnection.PointB = null;
            });
        }
        else
        {
            ZapAndFinish(delegate()
            {
                _bombModule.LogFormat("{0}→{1} is not a valid input/positive to output/negative. Strike.", _tangram.GetExternalConnectionIndex(_selectedConnection.PointA) + 1, _tangram.GetExternalConnectionIndex(_selectedConnection.PointB) + 1);
                _bombModule.HandleStrike();

                _selectedConnection.PointA = null;
                _selectedConnection.PointB = null;
            });            
        }
    }

    private void ZapAndFinish(Action toFinish)
    {
        StartCoroutine(DelayZapCoroutine(toFinish));
    }

    private IEnumerator DelayZapCoroutine(Action toFinish)
    {
        _audio.PlaySoundAtTransform(ZapSound.name, transform);

        float duration = 0.0f;
        while (duration < ZapSound.length)
        {
            yield return null;

            duration += Time.deltaTime;
            _displayBar.Progress = duration / ZapSound.length;
        }

        _displayBar.Progress = 0.0f;

        toFinish();
    }
}
