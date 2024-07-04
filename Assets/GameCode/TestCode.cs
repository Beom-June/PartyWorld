using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour
{


    private Rigidbody rigid;
    public int JumpPower;
    public int MoveSpeed;
    void Start()
    {
        rigid = GetComponent<Rigidbody>();      //Rigidbody 컴포넌트를 받아옴    
    }
    void Update() { Move(); Jump(); }
    void Move()
    {
        float h = Input.GetAxis("Horizontal"); float v = Input.GetAxis("Vertical");
        transform.Translate((new Vector3(h, 0, v) * MoveSpeed) * Time.deltaTime);
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        { rigid.AddForce(Vector3.up * JumpPower, ForceMode.Impulse); }
    }
}