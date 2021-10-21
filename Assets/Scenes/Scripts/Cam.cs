using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    [SerializeField]
     public float turnSpeed = 4.0f;
     [SerializeField]
     public Transform player;
 
     private Vector3 offset;
 
     void Start () {
         offset = new Vector3(player.position.x, player.position.y + 15.0f, player.position.z + 30.0f);
         player.eulerAngles = new Vector3(0, 180, 0);
     }
 
     void LateUpdate()
     {
         float turn = Input.GetAxis("Mouse X") * turnSpeed;
         offset = Quaternion.AngleAxis (turn, Vector3.up) * offset;
         transform.position = player.position + offset; 
         transform.LookAt(player.position + new Vector3(0, 10, 0));
         player.transform.Rotate(0, turn, 0);
     }
}
