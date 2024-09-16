using System.Collections;
using UnityEngine;

public class ArmsAnim : MonoBehaviour
{
    [SerializeField] private Transform _TopWalkingPos;
    [SerializeField] private Transform _DefaultPos;
    [SerializeField] private GameObject _Arms;
    [SerializeField] private float _WalkCycleSpeed = 1f;
    private bool _IsWalking = false;

    private IEnumerator CurrentCycle;
    void Start()
    {
        
    }

    public void StartWalkCycle()
    {
        if(!_IsWalking)
        {
            _IsWalking = true;
            CurrentCycle = WalkCycle();
            StartCoroutine(CurrentCycle);
        }
    }

    public void StopWalkCycle()
    {
        _IsWalking = false;
        StopCoroutine(CurrentCycle);
        StartCoroutine(ReturnToDefault());
    }

    public IEnumerator WalkCycle()
    {
        float upRate = 1.0f/Vector3.Distance(_DefaultPos.position, _TopWalkingPos.position) * _WalkCycleSpeed;
        float downRate = 1.0f/Vector3.Distance(_TopWalkingPos.position, _DefaultPos.position) * _WalkCycleSpeed;

        while(_IsWalking)
        {
            float tUp = 0.0f;
            float tDown = 0.0f;
            while(tUp < 1.0f)
            {
                tUp += Time.deltaTime * upRate;
                _Arms.transform.position = Vector3.Slerp(_Arms.transform.position, _TopWalkingPos.position, tUp);
                yield return new WaitForEndOfFrame();
            }
            
            while(tDown < 1.0f)
            {
                tDown += Time.deltaTime * downRate;
                _Arms.transform.position = Vector3.Slerp(_Arms.transform.position, _DefaultPos.position, tDown);
                yield return new WaitForEndOfFrame();
            }
            yield return null;
        }
    }

    public IEnumerator ReturnToDefault()
    {
        float returnRate = 1.0f/Vector3.Distance(_Arms.transform.position, _DefaultPos.position);

        float tRate = 0.0f;
        while(tRate < 1.0f)
        {
            tRate += Time.deltaTime * returnRate;
            _Arms.transform.position = Vector3.Slerp(_Arms.transform.position, _DefaultPos.position, tRate);
            yield return null;
        }
    }
}
