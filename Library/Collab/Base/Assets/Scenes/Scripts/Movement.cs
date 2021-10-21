using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    float walkSpeed = 10.0f;
    [SerializeField]
    float distToGround;
    int fraps = 0;
    public float deltaTime;


     // Use this for initialization
     void Start () {
         distToGround = GetComponent<Collider>().bounds.extents.y;
     }
     
     // Update is called once per frame
     void Update () {
         if(Input.GetKey(KeyCode.W)) {
             transform.Translate(Vector3.forward * Time.deltaTime * walkSpeed);
         }
         if(Input.GetKeyDown(KeyCode.W)) {
             this.gameObject.GetComponent<Animator>().SetFloat("speed", 1.0f);
         }
         if(Input.GetKeyUp(KeyCode.W)) {
             this.gameObject.GetComponent<Animator>().SetFloat("speed", 0.0f);
         }
         

         
         if(Input.GetKey(KeyCode.S)) {
             transform.Translate(Vector3.back * Time.deltaTime * walkSpeed);
         }

         

         if(Input.GetKey(KeyCode.A)) {
             transform.Translate(Vector3.left * Time.deltaTime * walkSpeed);
         }



         if(Input.GetKey(KeyCode.D)) {
             transform.Translate(Vector3.right * Time.deltaTime * walkSpeed);
         }

         if(Input.GetKeyDown(KeyCode.Space)) {
             this.gameObject.GetComponent<Animator>().SetBool("jumping", true);
             deltaTime = (Time.deltaTime - deltaTime) * 0.1f;
             fraps = (int) (1.0f / deltaTime / 100);
            this.gameObject.transform.Translate(0, 5, 0);
         }

        if(fraps > 0) {
            fraps--;
        } else {
            if(IsGrounded()) {
                this.gameObject.GetComponent<Animator>().SetBool("jumping", false);
            }
        }
     }


     bool IsGrounded() {
        return Physics.Raycast(this.transform.position, -Vector3.up, distToGround + 0.1f);
    }
 }
