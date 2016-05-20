using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PatternManager : MonoBehaviour
{
    public enum BuildingPosition
    {
        Left, Right
    }
    public class BuildingSet
    {
        public GameObject go;
        public Vector3 pos;
        public BuildingPosition dir;
    }
    public class ItemSet
    {
        public Vector2[] itemType_Left = new Vector2[31];
        public Vector2[] itemType_SubLeft = new Vector2[31];
        public Vector2[] itemType_Middle = new Vector2[31];
        public Vector2[] itemType_SubRight = new Vector2[31];
        public Vector2[] itemType_Right = new Vector2[31];
    }

    //prefab
    public List<GameObject> building_Pref = new List<GameObject>();

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


    public GameObject spawnObj_Pref;
    public GameObject floor_Pref;

    public List<BuildingSet> patternBuilding = new List<BuildingSet>();
    public List<ItemSet> patternItem = new List<ItemSet>();

    public bool isLoadingComplete = false;
    public float loadingPercent = 0f;

    //private List<GameObject> buildingList = new List<GameObject>();
    private List<GameObject> itemList = new List<GameObject>();
    private List<GameObject> floorList = new List<GameObject>();

    private GameObject spawnObj_Obj;//加载检测
    private ColliderCheck colliderCheck;

    private Vector3 angleLeft = new Vector3(0, 180, 0);
    private Vector3 angleRight = new Vector3(0, 0, 0);

    // Use this for initialization
    void Start()
    {
        SettingVariableFirst();
        StartCoroutine(CalAmountBuilding());
    }

    //计算位置信息
    void SettingVariableFirst()
    {
        if (defaultPosBuilding_Left.Count <= 0)
        {
            Vector3 pos = new Vector3(-3, 0, 0);
            for (int i = 0; i < 8; i++)
            {
                defaultPosBuilding_Left.Add(new Vector3(pos.x, pos.y, pos.z + (i * GlobalDefine.defaultBuildingInterval)));
            }
        }

        if (defaultPosBuilding_Right.Count <= 0)
        {
            Vector3 pos = new Vector3(3, 0, 4);
            for (int i = 0; i < 8; i++)
            {
                defaultPosBuilding_Right.Add(new Vector3(pos.x, pos.y, pos.z + (i * GlobalDefine.defaultBuildingInterval)));
            }
        }
        /*
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
        }*/
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator CalAmountBuilding()
    {
        //配置
        for (int i = 0; i < 8; i++)
        {
            AddRightBuilding(this.defaultPosBuilding_Right[i]);
        }
        for (int i = 0; i < 8; i++)
        {
            AddLeftBuilding(this.defaultPosBuilding_Left[i]);
        }

        this.spawnObj_Obj = GameObject.Instantiate(this.spawnObj_Pref);
        this.spawnObj_Obj.transform.position = new Vector3(0, 0, GlobalDefine.floorPosInterval);
        this.colliderCheck = this.spawnObj_Obj.GetComponentInChildren<ColliderCheck>();
        StartCoroutine(WaitCheckFloor());

        yield return 0;
    }

    GameObject AddRightBuilding(Vector3 pos)
    {
        GameObject go = GameObject.Instantiate<GameObject>(building_Pref[0]);
        go.transform.position = pos;
        go.transform.eulerAngles = angleRight;
        return go;
    }
    GameObject AddLeftBuilding(Vector3 pos)
    {
        GameObject go = GameObject.Instantiate<GameObject>(building_Pref[0]);
        go.transform.position = pos;
        go.transform.eulerAngles = angleLeft;
        return go;
    }

    //等待检测
    IEnumerator WaitCheckFloor()
    {
        while (this.colliderCheck.isCollision == false)
        {
            yield return 0;
        }
        this.colliderCheck.isCollision = false;
        StartCoroutine(AddFloor());

        yield return 0;
    }

    //添加地块
    IEnumerator AddFloor()
    {
        print("添加区块");
        if (spawnObj_Obj != null)
        {
            float startZPos = this.spawnObj_Obj.transform.position.z;
            AddRegion(startZPos);
        }

        //添加完毕重新检测
        StartCoroutine(WaitCheckFloor());

        yield return 0;
    }

    //设置初始化场景
    void InitScene()
    {

    }

    //添加区块
    void AddRegion(float startZPos)
    {
        Vector3 pos = Vector3.zero;
        pos.z = startZPos + GlobalDefine.floorPosInterval / 2;

        //添加地块
        GameObject floor = Instantiate<GameObject>(this.floor_Pref);
        floor.transform.position = pos;
        floorList.Add(floor);

        //添加建筑
        for (int i = 0; i < 8; i++)
        {
            //左
            Vector3 leftBuildingPos = new Vector3(-3, 0, startZPos + GlobalDefine.defaultBuildingInterval * i);
            GameObject leftBuilding = AddLeftBuilding(leftBuildingPos);
            leftBuilding.transform.parent = floor.transform;

            //右
            Vector3 rightBuildingPos = new Vector3(3, 0, 4 + startZPos + GlobalDefine.defaultBuildingInterval * i);
            GameObject rightBuilding = AddRightBuilding(rightBuildingPos);
            rightBuilding.transform.parent = floor.transform;
        }

        //添加检测
        this.spawnObj_Obj = Instantiate<GameObject>(this.spawnObj_Pref);
        this.spawnObj_Obj.transform.position = pos + new Vector3(0,0,GlobalDefine.floorPosInterval / 2);
        this.colliderCheck = this.spawnObj_Obj.GetComponentInChildren<ColliderCheck>();
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
