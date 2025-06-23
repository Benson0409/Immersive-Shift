using UnityEngine;

public class PlayerMove : MonoBehaviour
{
// --- 可在 Inspector 中調整的公開參數 ---
    public float moveSpeed = 5f;       // 角色的移動速度
    public float rotationSpeed = 200f; // 角色面向移動方向時的旋轉速度
    public float gravity = -9.81f;     // 重力加速度 (預設地球重力)

    // --- 私有變數 ---
    private CharacterController controller;
    private PlayerInputController inputController; // 引用 PlayerInputController 腳本
    private Vector3 verticalVelocity;  // 儲存垂直方向的速度，用於處理重力

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputController = GetComponent<PlayerInputController>(); // 獲取同物件上的 PlayerInputController
    }

    void Update()
    {
        // --- 處理角色水平移動 ---
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0; 
        cameraRight.y = 0;
        cameraForward.Normalize(); 
        cameraRight.Normalize();

        // 從 PlayerInputController 獲取移動輸入
        Vector3 horizontalMoveDirection = cameraForward * inputController.MoveInput.y + cameraRight * inputController.MoveInput.x;

        Gravity();

        // --- 整合水平移動和垂直速度，並應用到 CharacterController ---
        Vector3 totalMovement = (horizontalMoveDirection * moveSpeed) + verticalVelocity;
        controller.Move(totalMovement * Time.deltaTime);

        // --- 處理角色面向移動方向的旋轉 ---
        if (horizontalMoveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(horizontalMoveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    
    private void Gravity()
    {
        // --- 處理重力效果 ---
        if (controller.isGrounded) 
        {
            verticalVelocity.y = -0.5f; 
        }
        else 
        {
            verticalVelocity.y += gravity * Time.deltaTime; 
        }
    }
}
