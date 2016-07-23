using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public enum DirectionInput
    {
        Null, Left, Right, Up, Down
    }
    public enum Position
    {
        Middle, Left, Right
    }

    public bool enableKeyInput = true;
    public bool enableTouchInput = true;

    public CharacterController characterController;
    public AnimationManager animationManager;

    //玩家状态
    public bool isRoll;//翻滚
    public bool isDoubleJump;//是否是二段跳阶段

    public float gravity = 10;//重力系数
    private float jumpValue;//起跳高度

    private Vector3 currentPos;//鼠标当前位置

    private Position playerPos = Position.Middle;//相对中间的位置

    private bool activeInput;
    private DirectionInput directInput;

    private Vector3 moveDir;//玩家局部移动方向

    private GameScene scene;


    //public static PlayerController instance;//单例

    //触碰事件
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Item")
        {
            GameItem item = col.GetComponent<GameItem>();
            if (item != null)
            {
                item.OnHit();
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        //instance = this;
        jumpValue = GlobalDefine.defaultJumpValue;

        this.scene = GameObject.Find("Main Camera").GetComponent<GameScene>();
        StartCoroutine(UpdateAction());
    }

    IEnumerator UpdateAction()
    {
        while (scene != null && scene.currentLife > 0)
        {
            if (scene.isPause == false)
            {
                if (enableKeyInput)
                {
                    KeyInput();
                }

                if (enableTouchInput)
                {
                    TouchInput();
                }

                CheckLane();
                MoveForward();
            }
            else
            {
                Debug.LogWarning("游戏暂停 未处理");
            }

            yield return 0;
        }

        StartCoroutine(MoveBack());
        Debug.LogWarning("玩家死亡 未处理");

        yield return new WaitForSeconds(2.0f);

        Debug.Log("重启游戏");
    }

    void KeyInput()
    {
        if (Input.anyKeyDown)
        {
            activeInput = true;
        }

        if (activeInput)
        {
            if (Input.GetKey(KeyCode.A))
            {
                directInput = DirectionInput.Left;
                activeInput = false;
            }
            else
            {
                if (Input.GetKey(KeyCode.D))
                {
                    directInput = DirectionInput.Right;
                    activeInput = false;
                }
                else
                {
                    if (Input.GetKey(KeyCode.W))
                    {
                        directInput = DirectionInput.Up;
                        activeInput = false;
                    }
                    else
                    {
                        if (Input.GetKey(KeyCode.S))
                        {
                            directInput = DirectionInput.Down;
                            activeInput = false;
                        }
                    }
                }
            }
        }
        else
        {
            directInput = DirectionInput.Null;
        }
    }

    void TouchInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentPos = Input.mousePosition;
            activeInput = true;
        }

        if (Input.GetMouseButton(0) && activeInput)
        {
            float ang = CalculateAngle.GetAngle(currentPos, Input.mousePosition);
            if ((Input.mousePosition.x - currentPos.x) > 20)
            {
                if (ang < 45 && ang > -45)
                {
                    directInput = DirectionInput.Right;
                    activeInput = false;
                }
                else if (ang >= 45)
                {
                    directInput = DirectionInput.Up;
                    activeInput = false;
                }
                else if (ang <= -45)
                {
                    directInput = DirectionInput.Down;
                    activeInput = false;
                }
            }
            else if ((Input.mousePosition.x - currentPos.x) < -20)
            {
                if (ang < 45 && ang > -45)
                {
                    directInput = DirectionInput.Left;
                    activeInput = false;
                }
                else if (ang >= 45)
                {
                    directInput = DirectionInput.Down;
                    activeInput = false;
                }
                else if (ang <= -45)
                {
                    directInput = DirectionInput.Up;
                    activeInput = false;
                }
            }
            else if ((Input.mousePosition.y - currentPos.y) > 20)
            {
                if ((Input.mousePosition.x - currentPos.x) > 0)
                {
                    if (ang > 45 && ang <= 90)
                    {
                        directInput = DirectionInput.Up;
                        activeInput = false;
                    }
                    else if (ang <= 45 && ang >= -45)
                    {
                        directInput = DirectionInput.Right;
                        activeInput = false;
                    }
                }
                else if ((Input.mousePosition.x - currentPos.x) < 0)
                {
                    if (ang < -45 && ang >= -89)
                    {
                        directInput = DirectionInput.Up;
                        activeInput = false;
                    }
                    else if (ang >= -45)
                    {
                        directInput = DirectionInput.Left;
                        activeInput = false;
                    }
                }
            }
            else if ((Input.mousePosition.y - currentPos.y) < -20)
            {
                if ((Input.mousePosition.x - currentPos.x) > 0)
                {
                    if (ang > -89 && ang < -45)
                    {
                        directInput = DirectionInput.Down;
                        activeInput = false;
                    }
                    else if (ang >= -45)
                    {
                        directInput = DirectionInput.Right;
                        activeInput = false;
                    }
                }
                else if ((Input.mousePosition.x - currentPos.x) < 0)
                {
                    if (ang > 45 && ang < 89)
                    {
                        directInput = DirectionInput.Down;
                        activeInput = false;
                    }
                    else if (ang <= 45)
                    {
                        directInput = DirectionInput.Left;
                        activeInput = false;
                    }
                }

            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            directInput = DirectionInput.Null;
            activeInput = false;
        }
    }

    //检测左右移动
    void CheckLane()
    {
        if (directInput == DirectionInput.Left)
        {
            SwitchTrack(Position.Left);
        }
        else if (directInput == DirectionInput.Right)
        {
            SwitchTrack(Position.Right);
        }

        if (playerPos == Position.Middle)
        {
            if(transform.position.x > 0.05f){
				moveDir = Vector3.Lerp(moveDir, Vector3.left*6, Time.deltaTime * 500);
			}else if(transform.position.x < -0.05f){
				moveDir = Vector3.Lerp(moveDir, Vector3.right*6, Time.deltaTime * 500);
			}else{
				moveDir.x = 0;
				//checkSideCollision = false;
				transform.position = Vector3.Lerp(transform.position, new Vector3(0,transform.position.y,transform.position.z), 6 * Time.deltaTime);
			}
        }
        else if (playerPos == Position.Left)
        {
            if (transform.position.x > -1.8f)
            {
                moveDir = Vector3.Lerp(moveDir, Vector3.left * 6, Time.deltaTime * 500);
            }
            else
            {
                moveDir.x = 0;
                //checkSideCollision = false;
                transform.position = Vector3.Lerp(transform.position, new Vector3(-1.8f, transform.position.y, transform.position.z), 6 * Time.deltaTime);
            }
        }
        else if (playerPos == Position.Right)
        {
            if (transform.position.x < 1.8f)
            {
                moveDir = Vector3.Lerp(moveDir, Vector3.right * 6, Time.deltaTime * 500);
            }
            else
            {
                moveDir.x = 0;
                //checkSideCollision = false;
                transform.position = Vector3.Lerp(transform.position, new Vector3(1.8f, transform.position.y, transform.position.z), 6 * Time.deltaTime);
            }
        }
    }
    //向前移动并检测跳跃与翻滚
    void MoveForward()
    {
        if (scene != null)
        {
            float speedMove = scene.currentSpeed;

            if (characterController.isGrounded)
            {
                moveDir.y = 0;
                //moveDir = Vector3.zero;//在地面则清零防止重力无限叠加过大
                if (directInput == DirectionInput.Up)
                {
                    Debug.Log("跳跃");
                    Jump();
                    if (isDoubleJump)
                    {
                        isDoubleJump = false;
                    }
                }
                else if (directInput == DirectionInput.Down)
                {
                    Debug.Log("滚动");
                }
            }
            else
            {
                if (directInput == DirectionInput.Down)
                {
                    Debug.Log("空中 - 下");
                    QuickDown();
                }
                if (directInput == DirectionInput.Up)
                {
                    Debug.Log("空中 - 上");
                    if (!isDoubleJump)
                    {
                        Debug.Log("二段跳");
                        isDoubleJump = true;
                        JumpSecond();
                    }
                }
            }

            moveDir.z = 0;
            moveDir += this.transform.TransformDirection(Vector3.forward * speedMove);
            moveDir.y -= gravity * Time.deltaTime;
            CheckSideCollision();
            characterController.Move((moveDir) * Time.deltaTime);
        }
    }

    private void CheckSideCollision()
    {

    }

    //快速下落
    private void QuickDown()
    {
        moveDir.y -= jumpValue * 3;
    }

    //跳跃
    private void Jump()
    {
        animationManager.animationState = animationManager.Jump;
        moveDir.y += jumpValue;
    }

    //二段跳
    private void JumpSecond()
    {
        moveDir.y += jumpValue * 1.15f;
    }

    //切换道路
    private void SwitchTrack(Position pos)
    {
        if (playerPos == Position.Middle)
        {
            playerPos = pos;
            if(pos == Position.Right){
                //this.transform.Translate(1.5f, 0, 0);
            }else if(pos == Position.Left){
                //this.transform.Translate(-1.5f, 0, 0);
            }
        }else if(playerPos == Position.Left){
            if (pos == Position.Middle || pos == Position.Right)
            {
                playerPos = Position.Middle;
                //this.transform.Translate(1.5f, 0, 0);
            }
        }else if(playerPos == Position.Right){
            if(pos == Position.Middle || pos == Position.Left) {
                playerPos = Position.Middle;
                //this.transform.Translate(-1.5f, 0, 0);
            }
        }
    }

    IEnumerator MoveBack()
    {
        float z = transform.position.z - 0.5f;
        bool complete = false;
        while (complete == false)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, z), 2 * Time.deltaTime);
            if ((transform.position.z - z) < 0.05f)
            {
                complete = true;
            }
            yield return 0;
        }

        yield return 0;
    }
}
