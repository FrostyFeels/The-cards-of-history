using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MapEditor : MonoBehaviour
{
    MapGen gen;
    private Vector3 startTile, endTile;

    public Material currentMaterial;
    [SerializeField] Button failSafeBtn;
    [SerializeField] Button resetBtn, resetAllBtn;

    public CameraController camera2;

    public List<MapData> CurrentlySelected = new List<MapData>();
    public List<Renderer> gameobjectSelected = new List<Renderer>();

    public void Start()
    {
        gen = GameObject.Find("MapGenerator").GetComponent<MapGen>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MapSelecter(true);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            MapSelecter(false);
        }


        if (Input.GetMouseButton(0))
        {
            MapHolderSelecter();

        }
        else
        {
            camera2.enabled = true;
        }

        if(Input.GetMouseButtonUp(0))
        {
            Debug.Log("OwO");
            emptyList();
        }
    }

    public void MapHolderSelecter()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
                camera2.enabled = false;

            if (endTile == hit.collider.GetComponent<TileStats>()._ID)
                return;


            Debug.Log("PwP");
                cancelBuild();
                endTile = hit.collider.GetComponent<TileStats>()._ID;
                buildSelect2();
        }
    }

    public void MapSelecter(bool firstTile)
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (firstTile)
            {
                startTile = hit.collider.GetComponent<TileStats>()._ID;
            }
            else
            {
                endTile = hit.collider.GetComponent<TileStats>()._ID;
                buildSelect();
              
            }
        }
    }
    public void buildSelect()
    {
        Vector3 realStart;
        Vector3 realEnd;

        if (startTile.x > endTile.x)
        {
            realStart.x = endTile.x;
            realEnd.x = startTile.x;
        }
        else
        {
            realStart.x = startTile.x;
            realEnd.x = endTile.x;
        }

        if (startTile.z > endTile.z)
        {
            realStart.z = endTile.z;
            realEnd.z = startTile.z;
        }
        else
        {
            realStart.z = startTile.z;
            realEnd.z = endTile.z;
        }

        if(startTile.y > endTile.y)
        {
            realStart.y = endTile.y;
            realEnd.y = startTile.y;
        }
        else
        {
            realStart.y = startTile.y;
            realEnd.y = endTile.y;
        }




        for (int i = (int)realStart.z; i <= realEnd.z; i++)
        {
            for (int j = (int)realStart.x; j <= realEnd.x; j++)
            {

                if(gen.mode == MapGen.Mode._BUILDING)
                {
                    gen.map[gen.currentMapLevel].map[j + (i * gen.map[gen.currentMapLevel].gridSizeX)].selected = !gen.map[gen.currentMapLevel].map[j + (i * gen.map[gen.currentMapLevel].gridSizeX)].selected;

                    if (gen.map[gen.currentMapLevel].map[j + (i * gen.map[gen.currentMapLevel].gridSizeX)].selected)
                    {
                        gen.map[gen.currentMapLevel].gridArray[j, i].GetComponent<Renderer>().material = gen.filledMat;
                    }
                    else
                    {
                        gen.map[gen.currentMapLevel].gridArray[j, i].GetComponent<Renderer>().material = gen.nonFilledMat;
                        gen.map[gen.currentMapLevel].map[j + (i * gen.map[gen.currentMapLevel].gridSizeX)].currentMat = gen.filledMat;
                    }
                } 

                if(gen.mode == MapGen.Mode._ADDINGSPRITES)
                {
                    for (int level = (int)realStart.y; level <= realEnd.y; level++)
                    {
                        
                        if (gen.map[level].map[j + (i * gen.map[gen.currentMapLevel].gridSizeX)].selected)
                        {
                            gen.map[level].gridArray[j, i].GetComponent<Renderer>().material = currentMaterial;
                            gen.map[level].map[j + (i * gen.map[gen.currentMapLevel].gridSizeX)].currentMat = currentMaterial;
                        }
                    }
      
                }

                if(gen.mode == MapGen.Mode._ADDINGENVIROMENT)
                {
                    for (int level = (int)realStart.y; level <= realEnd.y; level++)
                    {
                        if (gen.map[level].map[j + (i * gen.map[gen.currentMapLevel].gridSizeX)].selected)
                        {
                            gen.map[level].gridArray[j, i].GetComponent<Renderer>().material = currentMaterial;
                            gen.map[level].map[j + (i * gen.map[gen.currentMapLevel].gridSizeX)].currentMat = currentMaterial;
                        }
                    }
                }


            }
        }
    }

    public void buildSelect2()
    {
        Vector3 realStart;
        Vector3 realEnd;

        if (startTile.x > endTile.x)
        {
            realStart.x = endTile.x;
            realEnd.x = startTile.x;
        }
        else
        {
            realStart.x = startTile.x;
            realEnd.x = endTile.x;
        }

        if (startTile.z > endTile.z)
        {
            realStart.z = endTile.z;
            realEnd.z = startTile.z;
        }
        else
        {
            realStart.z = startTile.z;
            realEnd.z = endTile.z;
        }

        if (startTile.y > endTile.y)
        {
            realStart.y = endTile.y;
            realEnd.y = startTile.y;
        }
        else
        {
            realStart.y = startTile.y;
            realEnd.y = endTile.y;
        }




        for (int i = (int)realStart.z; i <= realEnd.z; i++)
        {
            for (int j = (int)realStart.x; j <= realEnd.x; j++)
            {

                if (gen.mode == MapGen.Mode._BUILDING)
                {


                    if (!gen.map[gen.currentMapLevel].map[j + (i * gen.map[gen.currentMapLevel].gridSizeX)].selected)
                    {
                        CurrentlySelected.Add(gen.map[gen.currentMapLevel].map[j + (i * gen.map[gen.currentMapLevel].gridSizeX)]);
                        gameobjectSelected.Add(gen.map[gen.currentMapLevel].gridArray[j, i].GetComponent<Renderer>());
                        gen.map[gen.currentMapLevel].map[j + (i * gen.map[gen.currentMapLevel].gridSizeX)].currentMat = gen.filledMat;
                    }

                    gen.map[gen.currentMapLevel].map[j + (i * gen.map[gen.currentMapLevel].gridSizeX)].selected = true;

                    gen.map[gen.currentMapLevel].gridArray[j, i].GetComponent<Renderer>().material = gen.filledMat;



                }

                if (gen.mode == MapGen.Mode._ADDINGSPRITES)
                {
                    for (int level = (int)realStart.y; level <= realEnd.y; level++)
                    {

                        if (gen.map[level].map[j + (i * gen.map[gen.currentMapLevel].gridSizeX)].selected)
                        {
                            gen.map[level].gridArray[j, i].GetComponent<Renderer>().material = currentMaterial;
                            gen.map[level].map[j + (i * gen.map[gen.currentMapLevel].gridSizeX)].currentMat = currentMaterial;
                        }
                    }

                }

                if (gen.mode == MapGen.Mode._ADDINGENVIROMENT)
                {
                    for (int level = (int)realStart.y; level <= realEnd.y; level++)
                    {
                        if (gen.map[level].map[j + (i * gen.map[gen.currentMapLevel].gridSizeX)].selected)
                        {
                            gen.map[level].gridArray[j, i].GetComponent<Renderer>().material = currentMaterial;
                            gen.map[level].map[j + (i * gen.map[gen.currentMapLevel].gridSizeX)].currentMat = currentMaterial;
                        }
                    }
                }


            }
        }
    }

    public void emptyList()
    {
        CurrentlySelected.Clear();
        gameobjectSelected.Clear();
    }

    public void cancelBuild()
    {
        for (int i = 0; i < CurrentlySelected.Count; i++)
        {
            CurrentlySelected[i].selected = false;
            CurrentlySelected[i].currentMat = gen.nonFilledMat;
            gameobjectSelected[i].material = gen.nonFilledMat;

        }

        CurrentlySelected.Clear();
        gameobjectSelected.Clear();
    }

    public void GetMaterial(Material mat)
    {
        currentMaterial = mat;
        gen.assetsMenu.SetActive(false);
    }

    public void GetAssetMenu()
    {

        if (gen.mode == MapGen.Mode._ADDINGENVIROMENT)
        {
            gen.assetsMenu.SetActive(!gen.enviromentMenu.activeInHierarchy);
        }
        if (gen.mode == MapGen.Mode._ADDINGSPRITES)
        {
            gen.assetsMenu.SetActive(!gen.assetsMenu.activeInHierarchy);
        }
       
    }

    public void failSafeFullReset()
    {      
        gen.failSafeBtn.SetActive(true);
        failSafeBtn.onClick.RemoveAllListeners();
        failSafeBtn.onClick.AddListener(ResetAll);
    }

    public void failSafeReset()
    {
        gen.failSafeBtn.SetActive(true);
        failSafeBtn.onClick.RemoveAllListeners();
        failSafeBtn.onClick.AddListener(resetFloor);
    }

    public void CancelReset()
    {
        gen.failSafeBtn.SetActive(false);
       
    }


    public void resetFloor()
    {
            for (int y = 0; y < gen.map[gen.currentMapLevel].gridSizeY; y++)
            {
                for (int x = 0; x < gen.map[gen.currentMapLevel].gridSizeX; x++)
                {
                    gen.map[gen.currentMapLevel].map[x + (y * gen.map[gen.currentMapLevel].gridSizeX)].selected = false;                    
                    gen.map[gen.currentMapLevel].map[x + (y * gen.map[gen.currentMapLevel].gridSizeX)].currentMat = gen.nonFilledMat;
                    gen.map[gen.currentMapLevel].gridArray[x, y].GetComponent<MeshRenderer>().material = gen.nonFilledMat;

            }
            }
        gen.failSafeBtn.SetActive(false);
    }

    public void ResetAll()
    {
        for (int i = 0; i < gen.map.Count; i++)
        {
            for (int y = 0; y < gen.map[i].gridSizeY; y++)
            {
                for (int x = 0; x < gen.map[i].gridSizeX; x++)
                {
                    gen.map[i].map[x + (y * gen.map[i].gridSizeX)].selected = false;
                    gen.map[i].map[x + (y * gen.map[i].gridSizeX)].currentMat = gen.nonFilledMat;
                    gen.map[i].gridArray[x, y].GetComponent<MeshRenderer>().material = gen.nonFilledMat;
                    gen.map[i].gridArray[x, y].SetActive(false);
                    gen.map[gen.currentMapLevel].gridArray[x, y].SetActive(true);
                }
            }
        }
        gen.failSafeBtn.SetActive(false);
    }


}
