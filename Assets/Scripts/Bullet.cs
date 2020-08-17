using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
   
    bool IsHitTarget = false;
    float destroy_timer = 0f;
    float destroy_timer2 = 0f;
    //public EnermyControl enermyControl;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag== "Enermy")
        {
            
            transform.Find("ImpactParticle").gameObject.GetComponent<Transform>().forward = transform.position - other.gameObject.GetComponent<Transform>().position;
            transform.Find("ImpactParticle").gameObject.SetActive(true);
            other.gameObject.GetComponent<EnermyControl>().EnermyGoDie();
            GameObject.Find("UI").GetComponent<HUD>().GetScore();
            //enermyControl.EnermyGoDie();
            //Destroy(gameObject);
            IsHitTarget = true;
        }
        if (other.tag == "Wall")
        {
            Destroy(gameObject);
        }
        
    }

    private void Update()
    {

        if (IsHitTarget)
        {
            destroy_timer += Time.deltaTime;
            if (destroy_timer >= 1)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            destroy_timer2 += Time.deltaTime;
            if (destroy_timer2 >= 2)
            {
                Destroy(gameObject);
            }
        }
    }
}
