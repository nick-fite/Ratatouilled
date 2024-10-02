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
        if(col.gameObject != null)
            CollisionObject = col.gameObject;
        /*if(col.gameObject.tag == "Enemy")
        {
            Enemy enemy = col.gameObject.GetComponent<Enemy>();
            enemy.AddHealth(-Damage, _PushPoint.forward, PushingForce);
        }

        if(col.gameObject.tag == "Pickup")
        {

        }*/
    }
}
