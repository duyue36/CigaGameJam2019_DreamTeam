using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    //hasBoss == true let boss go
    public bool hasBoss = false;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground")) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("Bed")) return;
        
        if (other.gameObject.tag != this.tag)
        {
            if(GetComponent<Rigidbody2D>()==null) return;

            var vec = GetComponent<Rigidbody2D>().velocity;
            
            Debug.Log("hit: "+ other.transform.parent.name +" speed= "+vec.normalized);
                
             //speed 1  4

            if (vec.magnitude > 2f)
            {
                other.rigidbody.AddForce(vec*3f,ForceMode2D.Impulse);
            }
            
           
            
    //            if (other.rigidbody.CompareTag("Boss") && other.rigidbody.name =="Head")
            if(other.rigidbody != null)
            {
                if (other.rigidbody.name == "Head")
                {
                    //var vec = GetComponent<Rigidbody2D>().velocity;

                    if (vec.magnitude > 5)
                    {
                        GameObject.Find("BoxSound").GetComponent<AudioSource>().Play();
                    }

                    if (vec.magnitude > 2f)
                        //            Debug.Break();
                        if (!hasBoss)
                        {
                            hasBoss = true;
                            //gameObject.GetComponent<FixedJoint2D>().connectedBody = other.gameObject.GetComponent<Rigidbody2D>();
                            AddFixedJoint2DRigidbody(other.gameObject.GetComponent<Rigidbody2D>());
                        }
                }
            }
            
        }

       
    }

    public void AddFixedJoint2DRigidbody(Rigidbody2D rb2d)
    {
        var joint = gameObject.GetComponent<FixedJoint2D>();
        if (joint == null)
        {
            joint = gameObject.AddComponent<FixedJoint2D>();
        }

        joint.connectedBody = rb2d;
    }

    public void DestroyFixedJoint2DRigidbody()
    {
        //if(gameObject.GetComponent<FixedJoint2D>())
            GameObject.Destroy(gameObject.GetComponent<FixedJoint2D>());
        hasBoss = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
