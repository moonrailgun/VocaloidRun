using UnityEngine;
using System.Collections;

public class GameItem : MonoBehaviour {
    public float scoreAdd;//当物体为硬币时添加分数
    public int decreaseLife;//当物体为障碍物时减少生命
    [HideInInspector]
    public int itemID; //物品id

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
