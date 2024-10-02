using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] protected float _Strength = 10;
    [SerializeField] protected float _Jump = 3;
    [SerializeField] protected float _PushPower = 10;
    [SerializeField] private GameObject _Model;
    public float Strength
    {
        get{return _Strength;}
        protected set{_Strength = value;}
    } 
    public float Jump
    {
        get{return _Jump;}
        protected set{_Jump = value;}
    } 
    public float PushPower
    {
        get{return _PushPower;}
        protected set{_PushPower = value;}
    }

    public virtual void OnStart(){}

    public virtual void OnUpdate() 
    {
    }
    public virtual void Eject(Vector3 dir, float force, Vector3 pos)
    {
        GameObject model = Instantiate(_Model, pos, new Quaternion());
        Destroy(_Model);
        _Model = model;
        model.SetActive(true);
        model.GetComponent<Rigidbody>().AddForce(dir.normalized * force, ForceMode.Force);

    }

    public virtual void OnSpecialPressed(bool buttonPress){}
}
