using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HIV
{
    public class StickMan2 : MonoBehaviour
    {
        
        public Rigidbody2D Body;

     
        public Rigidbody2D LeftHandLow;
        public Rigidbody2D RightHandLow;
        
        public Rigidbody2D LeftFootLow;
        public Rigidbody2D RightFootLow;

        public KeyCode Q = KeyCode.Insert;
        public KeyCode W = KeyCode.Home;
        public KeyCode E = KeyCode.PageUp;
        public KeyCode A = KeyCode.Delete;
        public KeyCode S = KeyCode.End;
        public KeyCode D = KeyCode.PageDown;
        
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
            if (Input.GetKeyDown(W) || Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Keypad8))
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
            
            if (Input.GetKeyDown(Q) || Input.GetKeyDown(KeyCode.U) || Input.GetKeyDown(KeyCode.Keypad7))
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

            if(Input.GetKeyDown(E) || Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.Keypad9))
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
            
            if (Input.GetKeyDown(A) || Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Keypad4))
            {
                SoundManager.Singleton.PlayFist2();

                hasCD = true;

                LeftFootLow.AddForce(Vector3.left*handForce,ForceMode2D.Impulse);                                    
//                LeftFootLow.AddForce(Vector3.up*handForce*2,ForceMode2D.Impulse);                                    
            }
            
            if(Input.GetKeyDown(D) || Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.Keypad6))
            {
                SoundManager.Singleton.PlayFist2();

                hasCD = true;

                RightFootLow.AddForce(Vector3.right*handForce,ForceMode2D.Impulse);
//                RightFootLow.AddForce(Vector3.up*handForce*2,ForceMode2D.Impulse);
            }

            
            
        }
    }
}