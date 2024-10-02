using UnityEngine;

public class InteractCollision : MonoBehaviour
{
    GameObject _CollisionObject;

    public GameObject CollisionObject
    {
        get{return _CollisionObject;}
        private set{_CollisionObject = value;}
    }
    
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject != null && col.gameObject.layer != LayerMask.NameToLayer("Player"))
            CollisionObject = col.gameObject;
    }
}
