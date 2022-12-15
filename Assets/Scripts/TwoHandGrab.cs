using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.XR;
// using UnityEngine.XR.Interaction.Toolkit;
public class TwoHandGrab : MonoBehaviour
{

    public GameObject frontController;
    public GameObject backController;
    private Vector3 frontPos;
    private Vector3 backPos;

    // public List<XRSimpleInteractable> secondHandGrabPoints = new List<XRSimpleInteractable>();
    //
    // private XRInteractor _secondInteractor;
    //
    // private Quaternion _attachInitalRotation;
    //
    // public TwoHandRotationType twoHandRotationType;

    public bool snapToSecondHand = true;

    private Quaternion _initialRotationOffset;

    public Rigidbody cue;

    public bool isFrontPositionLocked;

    private Vector3 _cuePosition;
    private Vector3 _lockTransformForward;
    private float _lockOffset;
    private bool firstPress = true;

    // public enum TwoHandRotationType
    // {
    //     None,
    //     First,
    //     Second
    // };

    // Start is called before the first frame update
    void Start()
    {
        cue = gameObject.GetComponent<Rigidbody>();
        // _cuePosition = Vector3.forward;
        // foreach ( var item in secondHandGrabPoints){
        //     item.selectEntered.AddListener(OnSecondHandGrab);   
        //     item.selectExited.AddListener(OnSecondHandRelease);  
        // }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCuePosition();
    }

    private void UpdateCuePosition()
    {
        var backTrigger = backController.activeSelf; // this needs to be whether or not the trigger is pushed 
        frontPos = frontController.transform.position;
        backPos = backController.transform.position;
        
        if (backTrigger)
        {
            if (firstPress)
            {
                firstPress = false; 
                _lockTransformForward = transform.forward;
                _lockOffset = (frontPos - backPos).magnitude;
                
            }
            float currentOffset = (frontPos - backPos).magnitude;
            cue.MovePosition(_lockTransformForward + _cuePosition * (_lockOffset - currentOffset));
        }
        else
        {
            firstPress = true;
            _cuePosition = (0.75f * backPos) + (0.25f * frontPos);
            cue.MovePosition(_cuePosition);
            cue.MoveRotation(Quaternion.LookRotation(frontPos - backPos));
        }
    }
    // private Quaternion GetTwoHandRotation()
    // {
    //     Quaternion targetRotation;
    //     if (isFrontPositionLocked)
    //     {
    //         targetRotation = Quaternion.LookRotation(frontPos - backPos, frontController.transform.up);
    //     
    //     }
    //     else if (twoHandRotationType == TwoHandRotationType.None)
    //     {
    //         
    //     }
    //     else if (twoHandRotationType == TwoHandRotationType.First)
    //     {
    //         targetRotation = Quaternion.LookRotation(frontPos - backPos, firstInteractorSelecting.transform.up);
    //     }
    //     else
    //     {
    //         targetRotation = Quaternion.LookRotation(frontPos - backPos, _secondInteractor.transform.up);
    //
    //     }
    //
    //     return targetRotation;
    // }
    //
    // public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    // {
    //     if (_secondInteractor != null && firstInteractorSelecting != null)
    //     {
    //         if(snapToSecondHand)
    //             firstInteractorSelecting.transform.rotation = GetTwoHandRotation();
    //         else
    //             firstInteractorSelecting.transform.rotation = GetTwoHandRotation()* _initialRotationOffset;
    //     }
    //     base.ProcessInteractable(updatePhase);
    // }
    //
    // public void OnSecondHandGrab(SelectEnterEventArgs args){
    //     Debug.Log("OnSecondHandGrab");
    //     _secondInteractor = args.interactorObject;
    //     _initialRotationOffset = Quaternion.Inverse(GetTwoHandRotation() * firstInteractorSelecting.transform.rotation);
    //     
    // }
    // public void OnSecondHandRelease(SelectExitEventArgs args){
    //     Debug.Log("OnSecondHandRelease");
    //     _secondInteractor = null;
    // }
    //
    // protected override void OnSelectExited(SelectExitEventArgs args)
    // {
    //     Debug.Log("OnSelectExited");
    //     base.OnSelectExited(args);
    //     _secondInteractor = null;
    //     args.interactorObject.transform.localRotation = _attachInitalRotation;
    //
    // }
    //
    // protected override void OnSelectEntered(SelectEnterEventArgs args)
    // {
    //     Debug.Log("OnSelectEntered");
    //     base.OnSelectEntered(args);
    //     _attachInitalRotation = args.interactorObject.transform.localRotation;
    // }
    //
    // public override bool  IsSelectableBy(IXRSelectInteractor interactor){
    //     
    //     bool isalreadygrabbed = firstInteractorSelecting!= null && !interactor.Equals(firstInteractorSelecting);
    //     return base.IsSelectableBy(interactor) && !isalreadygrabbed;
    // }
}
