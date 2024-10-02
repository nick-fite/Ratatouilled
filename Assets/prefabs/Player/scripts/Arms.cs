using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public enum ArmActions
{
    Punching,
    Grabbing
}

public class Arms : MonoBehaviour
{
    public delegate void OnHandCollisionDelegate(GameObject obj, ArmActions action);
    public event OnHandCollisionDelegate OnHandCollision;
    AudioSource _LeftArmAudio;
    AudioSource _RightArmAudio;
    [SerializeField] AudioClip _WhiffClip;
    [SerializeField] GameObject _LeftHand;
    [SerializeField] GameObject _RightHand;
    [SerializeField] GameObject _PushPoint;
    [SerializeField] GameObject _Chest;
    CapsuleCollider _LeftCollider;
    CapsuleCollider _RightCollider;
    CapsuleCollider _CenterCollider; 
    InteractCollision _LeftHandCollision;
    InteractCollision _RightHandCollision;
    InteractCollision _CenterCollision;

    void Start()
    {
        AudioSource[] sources = GetComponents<AudioSource>();
        _LeftArmAudio = sources[0];
        _RightArmAudio = sources[1];

        _LeftArmAudio.clip = _WhiffClip;
        _RightArmAudio.clip = _WhiffClip;

        _LeftCollider = _LeftHand.GetComponent<CapsuleCollider>();
        _RightCollider = _RightHand.GetComponent<CapsuleCollider>();
        _CenterCollider = _Chest.GetComponent<CapsuleCollider>();

        _LeftCollider.enabled = false;
        _RightCollider.enabled = false;
        _CenterCollider.enabled = false;

        _LeftHandCollision = _LeftHand.GetComponent<InteractCollision>();
        _RightHandCollision = _RightHand.GetComponent<InteractCollision>();
        _CenterCollision = _CenterCollider.GetComponent<InteractCollision>();
    }

    void RightPunch()
    {
        _RightCollider.enabled = true;
        _CenterCollider.enabled = true;
        _RightArmAudio.Play();
    }
    void LeftPunch()
    {
        
        _LeftCollider.enabled = true;
        _CenterCollider.enabled = true;
        _LeftArmAudio.Play();
    }

    void LeftPunchEnd()
    {
        _LeftCollider.enabled = false;
        _CenterCollider.enabled = false;
        if(_LeftHandCollision.CollisionObject != null)
        {
            OnHandCollision.Invoke(_LeftHandCollision.CollisionObject, ArmActions.Punching);
        }
        if(_CenterCollision.CollisionObject != null)
        {
            OnHandCollision.Invoke(_CenterCollision.CollisionObject, ArmActions.Punching);
        }
    }
    
    void RightPunchEnd()
    {
        _RightCollider.enabled = false;
        _CenterCollider.enabled = false;
        if(_RightHandCollision.CollisionObject is not null)
        {
            OnHandCollision.Invoke(_LeftHandCollision.CollisionObject, ArmActions.Punching);
        }
        if(_CenterCollision.CollisionObject is not null)
        {
            OnHandCollision.Invoke(_CenterCollision.CollisionObject, ArmActions.Punching);
        }
    }

    void GrabStart()
    {
        _LeftCollider.enabled = true;
        _CenterCollider.enabled = true;
    }

    void GrabEnd()
    {
        _LeftCollider.enabled = false;
        _CenterCollider.enabled = false;
        if(_LeftHandCollision.CollisionObject is not null)
        {
            OnHandCollision.Invoke(_LeftHandCollision.CollisionObject, ArmActions.Grabbing);
        }
        if(_CenterCollision.CollisionObject is not null)
        {
            OnHandCollision.Invoke(_CenterCollision.CollisionObject, ArmActions.Grabbing);
        }
    }

}
