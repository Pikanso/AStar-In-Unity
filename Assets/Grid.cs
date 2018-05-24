using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public bool isGrid = true;
    public int X;
    public int Y;
    public int G_Number;
    public float H_Number;
    public float F_Number;
    public Vector3 pos;

    public GridManager gridManager;

    private bool isBeginFind=true;
    private List<float> keylist;
    private Grid foundGrid;
    public static int index=0;
    public GameObject pathGameObject;


    void Start()
    {
         //data= new GridData();
    }

    void OnTriggerEnter(Collider other)
    {
        switch(other.tag)
        {
            case "Wall":
            //isGrid = false;
                this.isGrid = false;
                break;
            case "Player":
            //设置该方块为起点
                this.tag = "PlayerGrid";
                this.GetComponent<MeshRenderer>().material.color = Color.gray;
                break;
            case "Target":
            //设置终点位置
                this.tag = "TargetGrid";
                this.GetComponent<MeshRenderer>().material.color = Color.gray;
                break;
        }
    }
    void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "Player":
                //设置该方块为起点
                this.tag = "Cube";
                this.GetComponent<MeshRenderer>().material.color = Color.white;
                break;
            case "Target":
                //设置该方块为起点
                this.tag = "Cube";
                this.GetComponent<MeshRenderer>().material.color = Color.white;
                break;
        }
    }
    void Update()
    {
        if (gridManager == null)
        {
            gridManager = GameObject.Find("GameManager").GetComponent<GridManager>();
        }
        //if (this.tag == "PlayerGrid" && isBeginFind)
        //{
        //    gridManager.Close_List.Add(this);
        //    isBeginFind = false;
        //    FindPath();
        //}
    }
    public void FindPath()
    {
        if (this.gameObject.tag == "TargetGrid")
        {
            foreach (var item in gridManager.Close_List)
            {
                item.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
            } 
            for (int a = gridManager.Close_List.Count - 1; a >= 0; a--)
            {
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        int x = gridManager.Close_List[a].X + i;
                        int y = gridManager.Close_List[a].Y + j;
                        string name = (10 * x + y + 1).ToString();
                        gridManager.Path_List.Add(gridManager.Close_List[a]);
                        Object cubePref=Resources.Load("Models/Sphere",typeof(GameObject));
                        GameObject pathGo = GameObject.Instantiate(cubePref, gridManager.Close_List[a].gameObject.transform.position, Quaternion.identity) as GameObject;
                        pathGo.transform.SetParent(GameObject.Find("PathPoint").transform);
                        GameObject point = GameObject.Find("SpawnPosition/" + name);
                        if(point!=null)
                        {
                            if (GameObject.Find("SpawnPosition/" + name).tag == "PlayerGrid")
                            {
                                return;
                            }
                        }


                    }
                }
            }
            return;
        }
        Dictionary<float,Grid> fNumDict=new Dictionary<float,Grid>();
        if (this.gameObject.tag != "TargetGrid")
        {
            //index++;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }
                    int x = this.X + i;
                    int y = this.Y + j;
                    string name = (10 * x + y + 1).ToString();
                    //Debug.Log(name);
                    Grid grid;
                    if(10 * x + y + 1<100&&10 * x + y + 1>0)
                    {
                        grid = GameObject.Find("SpawnPosition/" + name).GetComponent<Grid>();
                        if (grid.isGrid == false)
                        {
                            continue;
                        }
                        //Debug.Log(name);
                        grid.G_Number++;
                        grid.H_Number = Vector3.Distance(grid.gameObject.transform.position, GameObject.FindWithTag("TargetGrid").transform.position);
                        grid.F_Number = grid.G_Number + grid.H_Number;
                        if(gridManager.Close_List.Contains(grid))
                        {
                            continue;
                        }
                        if (!gridManager.Open_Dict.ContainsKey(grid))
                        {
                            gridManager.Open_Dict.Add(grid, grid.F_Number);//添加到OPen列表中
                        }
                        else
                        {
                            gridManager.Open_Dict[grid] = grid.F_Number;
                        }
                    }


                    //if(!fNumDict.ContainsKey(grid.F_Number))
                    //{
                    //    fNumDict.Add(grid.F_Number, grid);
                    //}
                }
            }
            keylist = new List<float>(gridManager.Open_Dict.Values);
            keylist.Sort();
            //fNumDict.TryGetValue(keylist[0], out foundGrid);
            foreach (KeyValuePair<Grid,float>kvp in gridManager.Open_Dict)
            {
                if(kvp.Value.Equals(keylist[0]))
                {
                    foundGrid = kvp.Key;//////根据最小的F值查找到对应的键
                }
            }
            //Debug.Log(foundGrid.gameObject.name);
            gridManager.Open_Dict.Remove(foundGrid);//移除OPen列表中的最小F值对应的元素
            gridManager.Close_List.Add(foundGrid);//添加该元素到Close列表中
            foundGrid.FindPath();
            foundGrid = null;
            keylist.Clear();
        }
        foreach (var item in gridManager.Open_Dict)
        {
            //Debug.Log(item);
        }
        isBeginFind = false;
    }
}

