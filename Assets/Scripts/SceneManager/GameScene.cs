using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameScene : MonoBehaviour {
    public float currentSpeed;
    public float currentLife;
    public float currentDistance = 0;

    public GameObject player;

    public GameObject pauseButton;
    public GameObject pausePanel;

    public GameObject[] playerPref;

    public bool isPause = false;
    public bool isRoll = false;
    public int countAddSpeed;//速度增加次数

    private float distanceCheck = 0;

	// Use this for initialization
	void Start () {
        this.currentLife = GlobalDefine.startLife;
        this.currentSpeed = GlobalDefine.startSpeed;

        this.pausePanel.SetActive(false);
        this.pauseButton.SetActive(true);

        GameStart();
	}
	
	// Update is called once per frame
	void Update () {

	}
    // 游戏开始
    void GameStart()
    {
        Debug.Log("Game Start");

        this.LoadPlayer();
    }

    //加载玩家
    void LoadPlayer()
    {
        Debug.Log("加载玩家");
        GameObject go = Instantiate<GameObject>(this.playerPref[0]);
        go.transform.position = new Vector3(0, 0, 0);
        StartCoroutine(UpdatePerDistance());
    }

    //速度检测
    IEnumerator UpdatePerDistance()
    {
        while (true)
        {
            if (this.isPause == false && this.currentLife > 0)
            {
                currentDistance += currentSpeed * Time.deltaTime;
                distanceCheck += currentSpeed * Time.deltaTime;
                if (distanceCheck >= GlobalDefine.addSpeedEveryDistance)
                {
                    //增加速度

                    currentSpeed += GlobalDefine.addedSpeed;
                    if (currentSpeed >= GlobalDefine.maxSpeed)
                    {
                        currentSpeed = GlobalDefine.maxSpeed;
                    }
                    countAddSpeed++;
                    distanceCheck = 0;
                }
            }
            yield return 0;
        }
    }

    public void PauseGame()
    {
        Debug.Log("暂停游戏");

        this.pausePanel.SetActive(true);
        this.pauseButton.SetActive(false);
    }
}
