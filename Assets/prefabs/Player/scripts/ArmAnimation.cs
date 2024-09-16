using UnityEngine;

public class ArmAnimation : MonoBehaviour
{
    AudioSource _LeftArmAudio;
    AudioSource _RightArmAudio;
    [SerializeField] AudioClip _WhiffClip;
    [SerializeField] GameObject _LeftHand;
    [SerializeField] GameObject _RightHand;
    CapsuleCollider _LeftCollider;
    CapsuleCollider _RightCollider;

    void Start()
    {
        AudioSource[] sources = GetComponents<AudioSource>();
        _LeftArmAudio = sources[0];
        _RightArmAudio = sources[1];

        _LeftArmAudio.clip = _WhiffClip;
        _RightArmAudio.clip = _WhiffClip;

        _LeftCollider = _LeftHand.GetComponent<CapsuleCollider>();
        _RightCollider = _RightHand.GetComponent<CapsuleCollider>();

        _LeftCollider.enabled = false;
        _RightCollider.enabled = false;
    }

    void RightPunchSound()
    {
        _RightCollider.enabled = true;
        _RightArmAudio.Play();
    }
    void LeftPunchSound()
    {
        _LeftCollider.enabled = true;
        _LeftArmAudio.Play();
    }

    void LeftPunchEnd()
    {
        _LeftCollider.enabled = false;
    }
    
    void RightPunchEnd()
    {
        _RightCollider.enabled = false;
    }
}
