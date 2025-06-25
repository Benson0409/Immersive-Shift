using System;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    public float moveSpeed = 5f;       
    public float rotationSpeed = 200f; 
    public float gravity = -9.81f;     
    public float jumpHeight = 1.2f;
    
    // --- 私有變數 ---
    private CharacterController controller;
    private PlayerInputController inputController; 
    private Vector3 verticalVelocity;
    private PlayerAttack playerAttack;
    private Animator playerAnimator;

    private bool isJump;
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputController = GetComponent<PlayerInputController>();
        playerAnimator = GetComponent<Animator>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    private void OnEnable()
    {
        if (inputController)
        {
            inputController.OnJump += Jump;
        }
    }

    private void OnDisable()
    {
        if (inputController)
        {
            inputController.OnJump -= Jump;
        }
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

        // --- 停止攻擊時的移動 ---
        if (playerAttack && playerAttack.IsAttacking) // <-- 如果正在攻擊
        {
            horizontalMoveDirection = Vector3.zero; // <-- 設置水平移動方向為零
            playerAnimator?.SetFloat("Speed", 0f); // <-- 確保 Animator 的 Speed 參數為零，讓它播放 Idle
        }
        else
        {
            playerAnimator.SetFloat("Speed", horizontalMoveDirection.magnitude * moveSpeed); 
        }
        
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
            isJump = false;
            verticalVelocity.y = -0.5f; 
            playerAnimator.SetBool("IsFalling", false); 
        }
        else 
        {
            verticalVelocity.y += gravity * Time.deltaTime;
            
            bool currentlyFalling = verticalVelocity.y < 0; 

            if (playerAnimator & !isJump)
            {
                playerAnimator.SetBool("IsFalling", currentlyFalling); 
            }
        }
    }
    
    private void Jump()
    {
        if (controller.isGrounded)
        {
            isJump = true;      
            // 使用物理公式計算跳躍所需的初始垂直速度
            verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            Debug.Log($"PlayerMove: 角色跳躍！初始垂直速度: {verticalVelocity.y}");
            controller.Move(verticalVelocity * Time.deltaTime);
            
            // 觸發跳躍動畫 (Animator Controller 會根據 Speed 決定播放哪個跳躍動畫)
            if (playerAnimator != null)
            {
                playerAnimator.SetTrigger("Jump");
                // 立即設定 IsFalling 為 false，避免跳躍初期（剛跳起時）被誤判為下落
                playerAnimator.SetBool("IsFalling", false);
            }
        } else {
            Debug.Log("PlayerMove: 不在地面上，無法跳躍。");
        }
    }

    //讓一般的跳躍不會過度到land的動畫，除非墜落太久
    public void IsJumpGround()
    {
        if (!controller.isGrounded)
        {
            isJump = false;
        }
    }
}
