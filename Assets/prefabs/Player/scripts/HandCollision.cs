using System.Collections.Generic;
using UnityEngine;

public class HandCollision : MonoBehaviour
{
    [SerializeField] int Damage = 10;
    void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.gameObject.name);
        if(col.gameObject.tag == "Enemy")
        {
            Enemy enemy = col.gameObject.GetComponent<Enemy>();
            int health = enemy.AddHealth(-Damage);

            if(health < 1)
            {
                enemy.RagDoll();
                
                Vector3 forceDir = enemy.gameObject.transform.position - transform.position;
                forceDir.y = 0;
                forceDir.Normalize();

                enemy.GeneralRB().AddExplosionForce(1000, transform.position, 100,0,ForceMode.Impulse);
            }
        }
    }
}
