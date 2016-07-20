using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PatternManager : MonoBehaviour
{
    static public int beforehandFloorNum = 3;//玩家面前视野预留的地块数量

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
    public GameObject coin_Pref;

    public List<BuildingSet> patternBuilding = new List<BuildingSet>();
    public List<ItemSet> patternItem = new List<ItemSet>();

    public List<GameObject> regionList = new List<GameObject>();//区域队列
    public List<GameObject> spawnObjList = new List<GameObject>();//生成区块检测队列
    private GameObject currentSpawnObj;//加载检测

    public bool isLoadingComplete = false;
    public float loadingPercent = 0f;

    //private List<GameObject> buildingList = new List<GameObject>();
    private List<GameObject> itemList = new List<GameObject>();


    private ColliderCheck currentColliderCheck;

    private Vector3 angleLeft = new Vector3(0, 180, 0);
    private Vector3 angleRight = new Vector3(0, 0, 0);

    // Use this for initialization
    void Start()
    {
        SettingVariableFirst();
        InitScene();
        //StartCoroutine(CalAmountBuilding());
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

        this.currentSpawnObj = GameObject.Instantiate(this.spawnObj_Pref);
        this.currentSpawnObj.transform.position = new Vector3(0, 0, GlobalDefine.floorPosInterval);
        this.currentColliderCheck = this.currentSpawnObj.GetComponentInChildren<ColliderCheck>();
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
        while (this.currentColliderCheck.isCollision == false)
        {
            yield return 0;
        }
        this.currentColliderCheck.isCollision = false;
        StartCoroutine(AddFloor());

        yield return 0;
    }

    //添加地块
    IEnumerator AddFloor()
    {
        print("添加区块");
        if (currentSpawnObj != null)
        {
            float startZPos = this.currentSpawnObj.transform.position.z + GlobalDefine.floorPosInterval * (beforehandFloorNum - 1);
            AddRegion(startZPos);

            //销毁之前的区块。销毁刚走过区块的前一块
            if (this.spawnObjList.Count >= beforehandFloorNum + 2)
            {
                Destroy(this.spawnObjList[0], 1.0f);
                this.spawnObjList.RemoveAt(0);
            }
            if (this.regionList.Count >= beforehandFloorNum + 2)
            {
                Destroy(this.regionList[0], 1.0f);
                this.regionList.RemoveAt(0);
            }
        }

        //添加完毕重新检测
        StartCoroutine(WaitCheckFloor());

        yield return 0;
    }

    //设置初始化场景
    void InitScene()
    {
        //初始化区块作为初始场景
        for (int i = 0; i < beforehandFloorNum; i++)
        {
            AddRegion(GlobalDefine.floorPosInterval * i);
        }

        StartCoroutine(WaitCheckFloor());//开启等待检测
    }

    //添加区块
    void AddRegion(float startZPos)
    {
        Vector3 pos = Vector3.zero;
        pos.z = startZPos + GlobalDefine.floorPosInterval / 2;

        //添加地块
        GameObject floor = Instantiate<GameObject>(this.floor_Pref);
        floor.transform.position = pos;
        this.regionList.Add(floor);

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
        GameObject spawnObj = Instantiate<GameObject>(this.spawnObj_Pref);
        spawnObj.transform.position = pos + new Vector3(0, 0, GlobalDefine.floorPosInterval / 2);
        this.spawnObjList.Add(spawnObj);

        //配置当前检测
        if (this.spawnObjList.Count >= beforehandFloorNum)
        {
            this.currentSpawnObj = this.spawnObjList[spawnObjList.Count - beforehandFloorNum];//为倒数第三个进行检测
            this.currentColliderCheck = this.currentSpawnObj.GetComponentInChildren<ColliderCheck>();
        }
    }

    void AddItem(Vector3 pos)
    {
        print("添加物品");



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
