using UnityEngine;

[RequireComponent(typeof(CharacterController))]
/// <summary>
/// プレイヤー管理
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Parameter")]
    [SerializeField] float moveSpeed;
    [SerializeField] float roteSpeed;

    [Header("Others")]
    [SerializeField] Transform camera;

    [Header("Component")]
    [SerializeField] CharacterController controller;

    private float verticalVelocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!controller)
        {
            controller = GetComponent<CharacterController>();
        }
        if (!camera)
        {
            camera = Camera.main.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        var moveValue = S_InputSystem.instance.moveValue;
        Vector3 moveF = Vector3.zero;

        if (controller.isGrounded)
        {
            // 接地している間も、少しだけ下向きの力を与え続けて接地を安定させる
            //verticalVelocity = -1.0f;
        }
        else
        {
            // 空中にいる場合は、時間の経過とともに重力で加速させる
            verticalVelocity += Physics.gravity.y;
        }

        if (moveValue != Vector2.zero)
        {
            var cameraF = Vector3.Scale(camera.forward, new Vector3(1, 0, 1)).normalized;
            moveF = cameraF * moveValue.y + camera.right * moveValue.x;
            moveF *= moveSpeed;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveF), roteSpeed);

            

        }

            moveF.y = verticalVelocity;

        controller.Move(moveF);

        //Debug.Log(controller.velocity);　移動量
    }
}
/*
 
 
 targetGroup.AddMember(enemy.transform, 1f, 1f);
targetGroup.RemoveMember(enemy.transform);
 
 */