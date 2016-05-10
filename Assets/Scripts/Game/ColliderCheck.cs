using UnityEngine;
using System.Collections;

public class ColliderCheck : MonoBehaviour {
    public bool isCollision = false;

    private GameObject player;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {
            if (player.transform.position.z >= this.transform.position.z)
            {
                isCollision = true;
            }
        }
	}
}
