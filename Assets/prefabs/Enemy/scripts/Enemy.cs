using UnityEngine;

public class Enemy : MonoBehaviour
{
   void OnTriggerEnter(Collider collision)
   {
    Debug.Log(collision.gameObject.name);
   } 
}
