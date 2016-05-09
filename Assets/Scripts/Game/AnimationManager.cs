using UnityEngine;
using System.Collections;

public class AnimationManager: MonoBehaviour {
    [System.Serializable]
    public class AnimationSet
    {
        public AnimationClip animation;
        public float speedAnimation = 1;
    }
    public AnimationSet run, turnLeft, turnRight, jumpUp, jumpLoop, jumpDown, roll, dead;

    public delegate void AnimationHandle();
    public AnimationHandle animationState;

    private GameScene sceneManager;
    private PlayerController playerController;
    private float speed_Run;
    private float default_Speed_Run;

	// Use this for initialization
	void Start () {
        sceneManager = GameObject.Find("Main Camera").GetComponent<GameScene>();
        playerController = GetComponent<PlayerController>();
        default_Speed_Run = sceneManager.currentSpeed;
        animationState = Run;
	}
	
	// Update is called once per frame
	void Update () {
        if (animationState != null)
        {
            animationState();
        }
	}

    public void Run()
    {
        //GetComponent<Animation>().CrossFade("run");
        //GetComponent<Animation>().Play(run.animation.name);
        speed_Run = (sceneManager.currentSpeed / default_Speed_Run) * (run.speedAnimation);
        //GetComponent<Animation>()[run.animation.name].speed = speed_Run;
    }

    public void Jump()
    {
        GetComponent<Animation>().Play(jumpUp.animation.name);
        if (GetComponent<Animation>()[jumpUp.animation.name].normalizedTime > 0.95f)
        {
            animationState = JumpLoop;
        }
    }
    public void JumpSecond()
    {
        GetComponent<Animation>().Play(roll.animation.name);
        if (GetComponent<Animation>()[roll.animation.name].normalizedTime > 0.95f)
        {
            animationState = JumpLoop;
        }
    }

    public void JumpLoop()
    {
        GetComponent<Animation>().CrossFade(jumpLoop.animation.name);
        if (playerController.characterController.isGrounded)
        {
            animationState = Run;
        }
    }

    public void TurnLeft()
    {
        GetComponent<Animation>().Play(turnLeft.animation.name);
        GetComponent<Animation>()[turnLeft.animation.name].speed = turnLeft.speedAnimation;
        if (GetComponent<Animation>()[turnLeft.animation.name].normalizedTime > 0.95f)
        {
            animationState = Run;
        }
    }

    public void TurnRight()
    {
        GetComponent<Animation>().Play(turnRight.animation.name);
        GetComponent<Animation>()[turnRight.animation.name].speed = turnRight.speedAnimation;
        if (GetComponent<Animation>()[turnRight.animation.name].normalizedTime > 0.95f)
        {
            animationState = Run;
        }
    }

    public void Roll()
    {
        GetComponent<Animation>().Play(roll.animation.name);
        if (GetComponent<Animation>()[roll.animation.name].normalizedTime > 0.95f)
        {
            this.sceneManager.isRoll = false;
            animationState = Run;
        }
        else
        {
            this.sceneManager.isRoll = true;
        }
    }

    public void Dead()
    {
        GetComponent<Animation>().Play(dead.animation.name);
    }
}
