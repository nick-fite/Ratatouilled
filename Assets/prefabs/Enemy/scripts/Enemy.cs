using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class Enemy : MonoBehaviour
{
   [SerializeField] GameObject _Hip;
   [SerializeField] int _Health = 100;
   List<Rigidbody> _RagDollRB = new List<Rigidbody>();
   Rigidbody _GeneralRB;
   [SerializeField] Rigidbody _ChestRB;
   Animator _Anim;

   public Rigidbody GeneralRB()
   {
      return _GeneralRB;
   }

   public Rigidbody GetChestRB()
   {
      return _ChestRB;
   }

   public List<Rigidbody> GetRagDollRB()
   {
      return _RagDollRB;
   }

   void Start()
   {
      _RagDollRB.AddRange(_Hip.GetComponentsInChildren<Rigidbody>());
      Debug.Log(_RagDollRB.Count);

      _GeneralRB = GetComponent<Rigidbody>();
      _Anim = GetComponent<Animator>();
      
      foreach(Rigidbody rb in _RagDollRB) {
         Debug.Log(rb.name);
         rb.GetComponent<Collider>().enabled = false;
         rb.useGravity = false;
         rb.isKinematic = true;
      }
   }

   public int AddHealth(int healthToAdd)
   {
      _Health += healthToAdd;
      Debug.Log(_Health);
      return _Health;
   }

   public void RagDoll()
   {
      _Anim.enabled = false;
      foreach(Rigidbody rb in _RagDollRB) {
         rb.isKinematic = false;
      }
   }
}
