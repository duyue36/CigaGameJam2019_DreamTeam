using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HIV
{
    public class StickMan : MonoBehaviour
    {
        
        public Rigidbody2D Body;

        public Rigidbody2D LeftHandLow;
        public Rigidbody2D RightHandLow;
        
        public Rigidbody2D LeftFootLow;
        public Rigidbody2D RightFootLow;

        public KeyCode Q = KeyCode.Q;
        public KeyCode W = KeyCode.W;
        public KeyCode E = KeyCode.E;
        public KeyCode A = KeyCode.A;
        public KeyCode S = KeyCode.S;
        public KeyCode D = KeyCode.D;
        
        public HitCollider hit1;//left hand
        public HitCollider hit2;//right hand
       

        // Start is called before the first frame update
        void Start()
        {
            
        }

        public float handForce = 25f;

        public float CD = 0.5f;
        private float timer = 0;
        public bool hasCD = false;
        
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(W))
            {
                Body.AddForce(Vector3.up*handForce*2,ForceMode2D.Impulse);
            }
            
            if (hasCD)
            {
                timer += Time.deltaTime;
                if (timer >= CD)
                {
                    timer = 0;
                    hasCD = false;
                }
                return;
            }
            
            if (Input.GetKeyDown(Q))
            {
                SoundManager.Singleton.PlayFist();
                hasCD = true;
                if (hit1.hasBoss)
                {
                    var dir = transform.position - hit1.GetComponent<FixedJoint2D>().transform.position;
                    hit1.DestroyFixedJoint2DRigidbody();
                    LeftHandLow.AddForce(dir.normalized * handForce, ForceMode2D.Impulse);
                }
                else
                {
                    LeftHandLow.AddForce(Vector3.left * handForce, ForceMode2D.Impulse);
                }
            }

            if(Input.GetKeyDown(E))
            {
                SoundManager.Singleton.PlayFist();

                hasCD = true;

                if (hit2.hasBoss)
                {
                    var dir = transform.position - hit2.GetComponent<FixedJoint2D>().transform.position;

                    hit2.DestroyFixedJoint2DRigidbody();
                    RightHandLow.AddForce(dir.normalized*handForce,ForceMode2D.Impulse);
                }
                else
                {
                    RightHandLow.AddForce(Vector3.right*handForce,ForceMode2D.Impulse);
                }
            }
            
            if (Input.GetKeyDown(A))
            {
                SoundManager.Singleton.PlayFist2();

                hasCD = true;
                
               
                {
                    LeftFootLow.AddForce(Vector3.left*handForce,ForceMode2D.Impulse);                                    
                }
//                LeftFootLow.AddForce(Vector3.up*handForce*2,ForceMode2D.Impulse);                                    
            }
            
            if(Input.GetKeyDown(D))
            {
                SoundManager.Singleton.PlayFist2();

                hasCD = true;
                
              

                RightFootLow.AddForce(Vector3.right*handForce,ForceMode2D.Impulse);
//                RightFootLow.AddForce(Vector3.up*handForce*2,ForceMode2D.Impulse);
            }

            
            
        }
    }
}