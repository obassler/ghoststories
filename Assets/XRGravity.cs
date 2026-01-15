using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class XRGravity : MonoBehaviour
{
    public float gravity = -9.81f;
    private CharacterController cc;
    private float verticalVelocity = 0f;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (cc.isGrounded && verticalVelocity < 0)
            verticalVelocity = -0.1f;

        verticalVelocity += gravity * Time.deltaTime;
        cc.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }
}
