using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
public class TwoHandGrab : XRGrabInteractable
{
    public List<XRSimpleInteractable> secondHandGrabPoints = new List<XRSimpleInteractable>();

    private IXRInteractor _secondInteractor;

    private Quaternion _attachInitalRotation;
    
    public TwoHandRotationType twoHandRotationType;

    public bool snapToSecondHand = true;

    private Quaternion _initialRotationOffset;

    public Rigidbody cue;

    public bool isFrontPositionLocked;

    private Vector3 _cuePosition;
    private Vector3 _lockTransformForward;
    private float _lockOffset;

    public enum TwoHandRotationType
    {
        None,
        First,
        Second
    };

    // Start is called before the first frame update
    void Start()
    {
        _cuePosition = Vector3.forward;
        foreach ( var item in secondHandGrabPoints){
            item.selectEntered.AddListener(OnSecondHandGrab);   
            item.selectExited.AddListener(OnSecondHandRelease);  
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCuePosition();
    }

    private void UpdateCuePosition()
    {
        if (isFrontPositionLocked)
        {
            float currentOffset = (_secondInteractor.transform.position - firstInteractorSelecting.transform.position).magnitude;
            cue.MovePosition(_lockTransformForward + _cuePosition * (_lockOffset - currentOffset));
        }
        else if (_secondInteractor != null && firstInteractorSelecting != null)
        {
            _cuePosition = (0.75f * firstInteractorSelecting.transform.position) + (0.25f * _secondInteractor.transform.position);
            cue.MovePosition(_cuePosition);
        }
    }
    private Quaternion GetTwoHandRotation()
    {
        Quaternion targetRotation;
        if (isFrontPositionLocked)
        {
            targetRotation = Quaternion.LookRotation(_secondInteractor.transform.position - firstInteractorSelecting.transform.position, firstInteractorSelecting.transform.up);
        
        }
        else if (twoHandRotationType == TwoHandRotationType.None)
        {
            targetRotation = Quaternion.LookRotation(_secondInteractor.transform.position - firstInteractorSelecting.transform.position);
        }
        else if (twoHandRotationType == TwoHandRotationType.First)
        {
            targetRotation = Quaternion.LookRotation(_secondInteractor.transform.position - firstInteractorSelecting.transform.position, firstInteractorSelecting.transform.up);
        }
        else
        {
            targetRotation = Quaternion.LookRotation(_secondInteractor.transform.position - firstInteractorSelecting.transform.position, _secondInteractor.transform.up);

        }

        return targetRotation;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (_secondInteractor != null && firstInteractorSelecting != null)
        {
            if(snapToSecondHand)
                firstInteractorSelecting.transform.rotation = GetTwoHandRotation();
            else
                firstInteractorSelecting.transform.rotation = GetTwoHandRotation()* _initialRotationOffset;
        }
        base.ProcessInteractable(updatePhase);
    }

    public void OnSecondHandGrab(SelectEnterEventArgs args){
        Debug.Log("OnSecondHandGrab");
        _secondInteractor = args.interactorObject;
        _initialRotationOffset = Quaternion.Inverse(GetTwoHandRotation() * firstInteractorSelecting.transform.rotation);
        _lockTransformForward = _secondInteractor.transform.forward;
        _lockOffset = (_secondInteractor.transform.position - firstInteractorSelecting.transform.position).magnitude;
    }
    public void OnSecondHandRelease(SelectExitEventArgs args){
        Debug.Log("OnSecondHandRelease");
        _secondInteractor = null;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        Debug.Log("OnSelectExited");
        base.OnSelectExited(args);
        _secondInteractor = null;
        args.interactorObject.transform.localRotation = _attachInitalRotation;

    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        Debug.Log("OnSelectEntered");
        base.OnSelectEntered(args);
        _attachInitalRotation = args.interactorObject.transform.localRotation;
    }

    public override bool  IsSelectableBy(IXRSelectInteractor interactor){
        
        bool isalreadygrabbed = firstInteractorSelecting!= null && !interactor.Equals(firstInteractorSelecting);
        return base.IsSelectableBy(interactor) && !isalreadygrabbed;
    }
}
