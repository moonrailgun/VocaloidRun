using UnityEngine;
using System.Collections;

public class GameItem : MonoBehaviour {
    public enum ItemType
    {
        Coin
    }

    public float scoreAdd;//当物体为硬币时添加分数
    public int decreaseLife;//当物体为障碍物时减少生命
    [HideInInspector]
    public int itemID; //物品id
    public ItemType type; //物品类型

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnHit()
    {
        if (this.type == ItemType.Coin)
        {
            if (GameScene.instance != null)
            {
                GameScene.instance.AddCoin();
            }
            Hide();
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
