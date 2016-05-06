using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameScene : MonoBehaviour {
    public float currentSpeed;
    public float currentLife;

    public GameObject player;

    public GameObject pauseButton;
    public GameObject pausePanel;

	// Use this for initialization
	void Start () {
        Debug.Log("游戏开始");

        this.currentLife = GlobalDefine.startLife;
        this.currentSpeed = GlobalDefine.startSpeed;

        LoadPlayer();

        GlobalDefine.SetImageColorAlpha(this.pausePanel.GetComponent<Image>(), 0);
        GlobalDefine.SetImageColorAlpha(this.pauseButton.GetComponent<Image>(), 255);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    //加载玩家
    void LoadPlayer()
    {
        //todo
    }

    public void PauseGame()
    {
        Debug.Log("暂停游戏");

        GlobalDefine.SetImageColorAlpha(this.pausePanel.GetComponent<Image>(), 255);
        GlobalDefine.SetImageColorAlpha(this.pauseButton.GetComponent<Image>(), 0);
    }
}
