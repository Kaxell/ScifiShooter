using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField]
    GroundCheck groundCheck;
    Rigidbody rigidbody;
    public float jumpStrength;
    public int jumpCount;
    private int curCount;
    //public event System.Action Jumped;


    void Reset()
    {
        groundCheck = GetComponentInChildren<GroundCheck>();
        if (!groundCheck)
            groundCheck = GroundCheck.Create(transform);
    }

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        curCount = 0;
    }

    void LateUpdate()
    {
        if (Input.GetButtonDown("Jump") && groundCheck.isGrounded)
        {
            //if (curCount == 1 && jumpCount > 1)
            //{
            //    rigidbody.velocity.Set(rigidbody.velocity.x, 0, rigidbody.velocity.z);
            //}

            rigidbody.AddForce(Vector3.up * (100) * jumpStrength, ForceMode.Force);
            //curCount++;

            //if (jumpCount -1 == curCount)
            //{
            //    ResetJumpCounter();
            //}
        }
    }

    //void ResetJumpCounter()
    //{
    //    if (groundCheck.isGrounded)
    //    {
    //        curCount = 0;
    //    }
    //    else
    //    {
    //        Invoke("ResetJumpCounter", 0.2f);
    //    }
    //}

}
