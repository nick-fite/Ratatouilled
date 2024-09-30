using System.Collections;
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
                
                Vector3 forceDir = enemy.gameObject.transform.position - transform.position;
                forceDir.Normalize();
                StartCoroutine(WaitThenExplode(enemy));
            }
        }
    }

    private IEnumerator WaitThenExplode(Enemy target)
    {
        target.RagDoll();
        Debug.Log("exploding");
        target.GetChestRB().AddExplosionForce(500f, transform.position,3f,0f, ForceMode.Impulse);
        yield return null;
    }
}
