using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PatternManager : MonoBehaviour {
    public class BuildingSet
    {
        public int[] stateBuilding_Left = new int[8];
        public int[] stateBuilding_Right = new int[8];
    }
    public class ItemSet
    {
        public Vector2[] itemType_Left = new Vector2[31];
        public Vector2[] itemType_SubLeft = new Vector2[31];
        public Vector2[] itemType_Middle = new Vector2[31];
        public Vector2[] itemType_SubRight = new Vector2[31];
        public Vector2[] itemType_Right = new Vector2[31];
    }

    [HideInInspector]
    public List<Vector3> defaultPosBuilding_Left = new List<Vector3>();
    [HideInInspector]
    public List<Vector3> defaultPosBuilding_Right = new List<Vector3>();
    [HideInInspector]
    public List<Vector3> defaultPosItem_Left = new List<Vector3>();
    [HideInInspector]
    public List<Vector3> defaultPosItem_SubLeft = new List<Vector3>();
    [HideInInspector]
    public List<Vector3> defaultPosItem_Middle = new List<Vector3>();
    [HideInInspector]
    public List<Vector3> defaultPosItem_SubRight = new List<Vector3>();
    [HideInInspector]
    public List<Vector3> defaultPosItem_Right = new List<Vector3>();

    //prefab
    public List<GameObject> building_Pref = new List<GameObject>();
    public List<GameObject> item_Pref = new List<GameObject>();
    //public GameObject spawnObj_Pref;
    //public GameObject floor_Pref;

    public List<BuildingSet> patternBuilding = new List<BuildingSet>();
    public List<ItemSet> patternItem = new List<ItemSet>();

    public bool isLoadingComplete = false;
    public float loadingPercent = 0f;

    private List<GameObject> building_Obj = new List<GameObject>();
    private List<GameObject> item_Obj = new List<GameObject>();
    private List<GameObject> floor_Obj = new List<GameObject>();

	// Use this for initialization
	void Start () {
        SettingVariableFirst();
        //StartCoroutine(CalAmountItem());
	}

    void SettingVariableFirst()
    {
        if (defaultPosBuilding_Left.Count <= 0)
        {
            Vector3 pos = new Vector3(-3, 0, 12);
            for (int i = 0; i < 8; i++)
            {
                defaultPosBuilding_Left.Add(new Vector3(pos.x, pos.y, pos.z - (i * 4)));
            }
        }

        if (defaultPosBuilding_Right.Count <= 0)
        {
            Vector3 pos = new Vector3(3, 0, 16);
            for (int i = 0; i < 8; i++)
            {
                defaultPosBuilding_Right.Add(new Vector3(pos.x, pos.y, pos.z - (i * 4)));
            }
        }

        if (defaultPosItem_Left.Count <= 0)
        {
            Vector3 pos = new Vector3(-1.8f, 0, 15);
            for (int i = 0; i < 31; i++)
            {
                defaultPosItem_Left.Add(new Vector3(pos.x, pos.y, pos.z - i));
            }
        }

        if (defaultPosItem_SubLeft.Count <= 0)
        {
            Vector3 pos = new Vector3(-0.9f, 0, 15);
            for (int i = 0; i < 31; i++)
            {
                defaultPosItem_SubLeft.Add(new Vector3(pos.x, pos.y, pos.z - i));
            }
        }

        if (defaultPosItem_Middle.Count <= 0)
        {
            Vector3 pos = new Vector3(0, 0, 15);
            for (int i = 0; i < 31; i++)
            {
                defaultPosItem_Middle.Add(new Vector3(pos.x, pos.y, pos.z - i));
            }
        }

        if (defaultPosItem_SubRight.Count <= 0)
        {
            Vector3 pos = new Vector3(0.9f, 0, 15);
            for (int i = 0; i < 31; i++)
            {
                defaultPosItem_SubRight.Add(new Vector3(pos.x, pos.y, pos.z - i));
            }
        }

        if (defaultPosItem_Right.Count <= 0)
        {
            Vector3 pos = new Vector3(1.8f, 0, 15);
            for (int i = 0; i < 31; i++)
            {
                defaultPosItem_Right.Add(new Vector3(pos.x, pos.y, pos.z - i));
            }
        }

        if (patternBuilding.Count <= 0)
        {
            patternBuilding.Add(new BuildingSet());
        }
        if (patternItem.Count <= 0)
        {
            patternItem.Add(new ItemSet());
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    /*IEnumerator CalAmountItem()
    {
        
        //25%
        ConvertPatternToItemTpyeSet();
        itemTypeMax = new SetFloatItemType();
        int i = 0;
        while (i < item_Pref.Count)
        {
            itemTypeMax.item.Add(0);
            i++;
        }
        i = 0;
        loadingPercent = 1;
        while (i < _itemType.Count)
        {
            int j = 0;
            while (j < _itemType[i].item.Count)
            {
                if (_itemType[i].item[j] > itemTypeMax.item[j])
                {
                    itemTypeMax.item[j] = _itemType[i].item[j];
                }
                j++;
            }
            i++;
        }
        i = 0;
        loadingPercent = 3;
        amountItemSpawn = new int[itemTypeMax.item.Count];
        while (i < amountItemSpawn.Length)
        {
            amountItemSpawn[i] = itemTypeMax.item[i] * amountFloorSpawn;
            amountItemSpawn[i]++;
            i++;
        }
        yield return 0;
        loadingPercent = 5;
        StartCoroutine(CalAmountBuilding());
    }*/
}
