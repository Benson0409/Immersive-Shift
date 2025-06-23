using UnityEngine;
using UnityEngine.InputSystem; // 引入新的輸入系統命名空間

public class PlayerMoveController : MonoBehaviour
{

    public float moveSpeed = 5f;       
    public float rotationSpeed = 200f; 
    public float gravity = -9.81f;     


    private CharacterController controller;
    
    private DefaultInputActions.PlayerActions playerActions; 
    
    // 儲存從 Input System 讀取到的 2D 移動輸入
    private Vector2 moveInput;
    // 儲存垂直方向的速度，用於處理重力
    private Vector3 verticalVelocity;  

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerActions = new DefaultInputActions().Player; 
        
        playerActions.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerActions.Move.canceled += ctx => moveInput = Vector2.zero; 

    }

    void OnEnable()
    {
        playerActions.Enable(); 
    }

    void OnDisable()
    {
        playerActions.Disable(); 
    }

    void Update()
    {
        // --- 處理角色水平移動 ---

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        // 忽略攝影機的 Y 軸分量，確保角色只在水平面上移動
        cameraForward.y = 0; 
        cameraRight.y = 0;
        
        // 標準化向量，避免對角線移動速度過快
        cameraForward.Normalize(); 
        cameraRight.Normalize();

        // 根據 Move 輸入計算最終的水平移動方向
        Vector3 horizontalMoveDirection = cameraForward * moveInput.y + cameraRight * moveInput.x;
        
        Gravity();

        Vector3 totalMovement = (horizontalMoveDirection * moveSpeed) + verticalVelocity;

        controller.Move(totalMovement * Time.deltaTime);

        // --- 處理角色面向移動方向的旋轉 ---
        if (horizontalMoveDirection != Vector3.zero)
        {
            // 計算角色應該看向哪個方向的旋轉
            Quaternion targetRotation = Quaternion.LookRotation(horizontalMoveDirection);
            // 平滑地將角色從當前方向旋轉到目標方向
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