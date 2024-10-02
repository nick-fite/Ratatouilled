using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
   [SerializeField] GameObject _Hip;
   [SerializeField] float _Health = 100;
   List<Rigidbody> _RagDollRB = new List<Rigidbody>();
   [SerializeField] Rigidbody _ChestRB;
   Animator _Anim;
   ExplodeOnImpact Explode;
   [SerializeField] CapsuleCollider ExplodeCollider;

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

      Explode = _Hip.GetComponent<ExplodeOnImpact>();
      Explode.Explode = true;
      Explode.enemyObj = gameObject.transform.parent.gameObject;

      ExplodeCollider = _Hip.GetComponent<CapsuleCollider>();
      ExplodeCollider.enabled = false;
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
      GetComponent<Collider>().isTrigger = true;
      foreach(Rigidbody rb in _RagDollRB) {
         rb.isKinematic = false;
         rb.useGravity = true;
         rb.GetComponent<Collider>().enabled = true;
      }
   }

   public void Die(Transform pointOfImpact, float force)
   {
      if(force >= 100)
      {
         ExplodeCollider.enabled = true;
         Explode.Explode = true;
         Explode.ExplosiveRadius = 20f;
         Explode.ExplosiveForce = force;
      }

      RagDoll();
      foreach(Rigidbody rb in _RagDollRB) {
         rb.AddExplosionForce(force, pointOfImpact.position, 20f, 0f, ForceMode.Impulse);
      }
   }
}
