using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {

	// Use this for initialization


    public int numberXAxisCount;
    public int numberZAxisCount;
    public GameObject cube;

    public GameObject spawnPositionPref;

    private GameObject spawnPosition;
    public GameObject walls;
    public GameObject playerPrefab;
    public GameObject targetPrefab;

    //public List<Grid> Open_List = new List<Grid>();
    public Dictionary<Grid, float> Open_Dict = new Dictionary<Grid, float>();
    public List<Grid> Close_List = new List<Grid>();
    public List<Grid> Path_List = new List<Grid>();

	void Start () {
        walls.SetActive(false);
        spawnPosition = Instantiate(spawnPositionPref, Vector3.zero, Quaternion.identity) as GameObject;
        spawnPosition.name = "SpawnPosition";
        
        StartCoroutine(SpawnGrid());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator SpawnGrid()
    {
        for (int x = 0; x < numberXAxisCount; x++)
        {
            for (int z = 0; z < numberZAxisCount; z++)
            {
                yield return new WaitForFixedUpdate();
                Vector3 position=new Vector3(x+0.5f,0,z+0.5f);
                GameObject grid = Instantiate(cube, position, Quaternion.identity);
                
                grid.transform.SetParent(spawnPosition.transform);
                grid.name = (x * 10 + z+1).ToString();
                grid.transform.Find("Text").GetComponent<TextMesh>().text = grid.name;
                grid.AddComponent<Grid>();
                grid.GetComponent<Grid>().X = x; grid.GetComponent<Grid>().Y = z;
                grid.transform.position -= new Vector3(numberXAxisCount / 2, 0, numberZAxisCount / 2);
            }
        }
        SpawnOthers();
    }
    void SpawnOthers()
    {
        walls.SetActive(true);
        GameObject player=Instantiate(playerPrefab,new Vector3(3.5f,0.1f,-0.5f),Quaternion.identity);
        GameObject target = Instantiate(targetPrefab, new Vector3(-3.5f, 0.1f, -1.5f), Quaternion.identity);
    }

    void GetFCount()
    {
        for (int x = 0; x < numberXAxisCount; x++)
        {
            for (int z = 0; z < numberZAxisCount; z++)
            {
                //Grid grid = GameObject.Find("SpawnPosition/" + name).GetComponent<Grid>();
                
            }
        }
    }
    
    public void OnStartFindClick()
    {
        GameObject go = GameObject.FindWithTag("PlayerGrid");
        this.Close_List.Add(go.GetComponent<Grid>());
        go.GetComponent<Grid>().FindPath();
    }
    public void OnClearClick()
    {
        foreach (var item in this.Close_List)
        {
            item.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
        }
        this.Close_List.Clear();
        this.Open_Dict.Clear();
        GameObject sp=GameObject.Find("SpawnPosition");
        for (int i = 0; i < sp.transform.childCount; i++)
        {
            sp.transform.GetChild(i).GetComponent<Grid>().G_Number = 0;
            sp.transform.GetChild(i).GetComponent<Grid>().F_Number = 0;
            sp.transform.GetChild(i).GetComponent<Grid>().H_Number = 0;
        }
        for (int i = 0; i < GameObject.Find("PathPoint").transform.childCount; i++)
        {
            Destroy(GameObject.Find("PathPoint").transform.GetChild(i).gameObject);
        }
    }
}
