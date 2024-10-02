using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
   [SerializeField] GameObject _Hip;
   [SerializeField] float _Health = 100;
   List<Rigidbody> _RagDollRB = new List<Rigidbody>();
   [SerializeField] Rigidbody _ChestRB;
   Animator _Anim;

   void Start()
   {
      _RagDollRB.AddRange(_Hip.GetComponentsInChildren<Rigidbody>());
      Debug.Log(_RagDollRB.Count);

      _Anim = GetComponent<Animator>();
      
      foreach(Rigidbody rb in _RagDollRB) {
         rb.GetComponent<Collider>().enabled = false;
         rb.useGravity = false;
         rb.isKinematic = true;
      }
   }

   public void AddHealth(float healthToAdd, Transform pointOfImpact, float force)
   {
      _Health += healthToAdd;
      Debug.Log(_Health);
      
      if (_Health < 1)
         Die(pointOfImpact, force);

   }

   public void RagDoll()
   {
      _Anim.enabled = false;
      foreach(Rigidbody rb in _RagDollRB) {
         rb.isKinematic = false;
         rb.useGravity = true;
         rb.GetComponent<Collider>().enabled = true;
      }
   }

   public void Die(Transform pointOfImpact, float force)
   {
      RagDoll();
      /*Vector3 dir = (pointOfImpact - transform.position).normalized;
      _ChestRB.AddForce(dir * force, ForceMode.Impulse);*/
      /*foreach(Rigidbody rb in _RagDollRB) {
         Vector3 dir = pointOfImpact.forward.normalized;
         rb.AddForce(dir * force, ForceMode.Impulse);
      }*/
   }
}
