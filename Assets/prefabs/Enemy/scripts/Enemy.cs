using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class Enemy : MonoBehaviour
{
   [SerializeField] GameObject _Hip;
   [SerializeField] int _Health = 100;
   List<Rigidbody> _RagDollRB = new List<Rigidbody>();
   void Start()
   {
      _RagDollRB.AddRange(_Hip.GetComponentsInChildren<Rigidbody>());
      Debug.Log(_RagDollRB.Count);

      foreach(Rigidbody rb in _RagDollRB)
      {
         rb.isKinematic = false;
      }
   }

   public void AddHealth(int healthToAdd)
   {
      _Health -= healthToAdd;
      Debug.Log(_Health);
      if(_Health == 0)
      {
         RagDoll();      
      }
   }

   public void RagDoll()
   {
      Rigidbody enemyRB = GetComponent<Rigidbody>(); 
      enemyRB.isKinematic = false;

      foreach(Rigidbody rb in _RagDollRB)
      {
         rb.isKinematic = true;
      }
   }
}
