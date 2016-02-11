using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class VoxelizationClientTest : MonoBehaviour
{    
    //Behaviour variables
    public float voxelSideSize = 0.1f;
    public bool fillVoxelShell = true;

    public bool includeChildren = true;
    public bool createMultipleGrids = true;

    public Material material;

    //Debug variables
    private static bool debug = true;
    public bool drawActiveVoxelsGizmo = true;
    public Color activeVoxelsColor = new Color(1, 0, 0, 0.5f);
    public bool drawInactiveVoxelsGizmo = false;
    public Color inactiveVoxelsColor = new Color(0, 1, 0, 0.5f);
    public Vector3 voxelGridViewOffset = Vector3.zero;
    public KeyCode calculationKey;
    public KeyCode spawningKey;

    
    private List<VoxelizationServer.AABCGrid> aABCGrids;
    private bool gridsCreated;
    private GameObject voxel;

    //Precalculated variables (so calculation of grid will be faster on realtime)
    Transform transf;
    Renderer rend;
    SkinnedMeshRenderer sRend;
    MeshFilter meshFilter;
    List<Transform> transforms;
    List<Renderer> renderers;
    List<bool> isSkinedMeshRenderer;
    List<MeshFilter> meshFilters;
    List<Mesh> meshes;

    void Start()
    {
        if (!includeChildren)
        {
            transf = gameObject.transform;
            rend = gameObject.GetComponent<Renderer>();
            meshFilter = gameObject.GetComponent<MeshFilter>();
            sRend = gameObject.GetComponent<SkinnedMeshRenderer>();
        }
        else
        {
            transforms = new List<Transform>();
            renderers = new List<Renderer>();
            isSkinedMeshRenderer = new List<bool>();
            meshFilters = new List<MeshFilter>();
            meshes = new List<Mesh>();

            PopulateLists(gameObject);
        }
    }

    private void PopulateLists(GameObject gameObj)
    {
        Renderer rend = gameObj.GetComponent<Renderer>();
        if (rend != null)
        {
            transforms.Add(gameObj.transform);
            renderers.Add(rend);
            meshFilters.Add(gameObj.GetComponent<MeshFilter>());
            isSkinedMeshRenderer.Add(false);
        }
        else
        {
            SkinnedMeshRenderer sRend = gameObject.GetComponent<SkinnedMeshRenderer>();
            if(sRend != null)
            {
                transforms.Add(gameObj.transform);
                renderers.Add(sRend);
                meshFilters.Add(null);
                isSkinedMeshRenderer.Add(true);
            }
        }

        //Call children
        for (int i = 0; i < gameObj.transform.childCount; ++i)
        {
            PopulateLists(gameObj.transform.GetChild(i).gameObject);
        }
    }

    public void CalculateVoxelsGrid()
    {
        float startTime = 0.0f; float endTime;
        if (debug)
        {
            Debug.Log("--Start CalculateVoxelsGrid:");
            Debug.Log("--    Cube Side: " + voxelSideSize);
            Debug.Log("--    Include Inside: " + fillVoxelShell);
            startTime = Time.realtimeSinceStartup;
        }

        //Old
        /*if (createMultipleGrids && includeChildren)
        {
            aABCGrids = VoxelizationServer.CreateMultipleGridsWithGameObjectMesh(gameObject, voxelSideSize, fillVoxelShell);
        }
        else
        {
            aABCGrids = new List<VoxelizationServer.AABCGrid>();
            aABCGrids.Add(VoxelizationServer.CreateGridWithGameObjectMesh(gameObject, voxelSideSize, includeChildren, fillVoxelShell));
        }*/

        //New
        //1 Grid 1 Mesh
        if (!includeChildren)
        {
            aABCGrids = new List<VoxelizationServer.AABCGrid>();

            if (rend != null)
            {
                aABCGrids.Add(VoxelizationServer.Create1Grid1Object(transf, meshFilter.mesh, rend, voxelSideSize, fillVoxelShell));
            }
            else if (sRend != null)
            {
                Mesh mesh = new Mesh();
                sRend.BakeMesh(mesh);
                aABCGrids.Add(VoxelizationServer.Create1Grid1Object(transf, mesh, sRend, voxelSideSize, fillVoxelShell));
            }
        }
        else
        {
            meshes.Clear();
            for(int i = 0; i < renderers.Count; ++i)
            {
                if(isSkinedMeshRenderer[i])
                {
                    Mesh mesh = new Mesh();
                    (renderers[i] as SkinnedMeshRenderer).BakeMesh(mesh);
                    meshes.Add(mesh);
                }
                else
                {
                    meshes.Add(meshFilters[i].mesh);
                }
            }

            //1 Grid N Meshes
            if (!createMultipleGrids)
            {
                aABCGrids = new List<VoxelizationServer.AABCGrid>();
                aABCGrids.Add(VoxelizationServer.Create1GridNObjects(transforms, meshes, renderers, voxelSideSize, fillVoxelShell));
            }
            //N Grids N Meshes
            else
            {
                aABCGrids = VoxelizationServer.CreateNGridsNObjects(transforms, meshes, renderers, voxelSideSize, fillVoxelShell);
            }
        }
        
        if (debug)
        {
            endTime = Time.realtimeSinceStartup;
            Debug.Log("--TIME SPENT: " + (endTime - startTime) + "s");
        }

        if (aABCGrids != null && aABCGrids.Count > 0)
            gridsCreated = true;
    }


    public void SpawnVoxels()
    {
        float startTime = 0.0f; float endTime;
        if (debug)
        {
            Debug.Log("--Start SpawnVoxels:");
            startTime = Time.realtimeSinceStartup;
        }

        if (aABCGrids != null)
        {
            foreach (VoxelizationServer.AABCGrid aABCGrid in aABCGrids)
            {
                Vector3 preCalc = aABCGrid.GetOrigin();
                for (short x = 0; x < aABCGrid.width; ++x)
                {
                    for (short y = 0; y < aABCGrid.height; ++y)
                    {
                        for (short z = 0; z < aABCGrid.depth; ++z)
                        {
                            if (aABCGrid.IsAABCActiveUnsafe(x, y, z))
                            {
                                Vector3 cubeCenter = aABCGrid.GetAABCCenterUnsafe(x, y, z) + preCalc;
                                voxel = Man.voxelPool.GetVoxel();
                                if (voxel != null)
                                {
                                    voxel.transform.position = cubeCenter;
                                    voxel.transform.rotation = Quaternion.identity;
                                    if (material != null)   
                                        voxel.GetComponent<Renderer>().material = material;
                                    voxel.SetActive(true);
                                }
                            }
                        }
                    }
                }
            }
        }

        if (debug)
        {
            endTime = Time.realtimeSinceStartup;
            Debug.Log("--TIME SPENT: " + (endTime - startTime) + "s");
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(calculationKey))
        {         
            CalculateVoxelsGrid();              
        }
        else if (Input.GetKeyDown(spawningKey))
        {
            SpawnVoxels();
        }
    }

    void OnDrawGizmos()
    {
        //Caution! Performance hit if heavy grid is drawn
        if (gridsCreated && (drawActiveVoxelsGizmo || drawInactiveVoxelsGizmo))
        {
            DrawMeshShell();
        }
    }

    void DrawMeshShell()
    {
        Vector3 cubeSize = new Vector3(voxelSideSize, voxelSideSize, voxelSideSize);

        foreach (VoxelizationServer.AABCGrid aABCGrid in aABCGrids)
        {
            Vector3 preCalc = aABCGrid.GetOrigin() + voxelGridViewOffset;

            for (short x = 0; x < aABCGrid.GetWidth(); ++x)
            {
                for (short y = 0; y < aABCGrid.GetHeight(); ++y)
                {
                    for (short z = 0; z < aABCGrid.GetDepth(); ++z)
                    {                  
                        if (aABCGrid.IsAABCActiveUnsafe(x, y, z))
                        {
                            if (drawActiveVoxelsGizmo)
                            {
                                Gizmos.color = activeVoxelsColor;
                                Gizmos.DrawWireCube(aABCGrid.GetAABCCenterUnsafe(x, y, z) + preCalc, cubeSize);
                            }
                        }
                        else if (drawInactiveVoxelsGizmo)
                        {
                            Gizmos.color = inactiveVoxelsColor;
                            Gizmos.DrawWireCube(aABCGrid.GetAABCCenterUnsafe(x, y, z) + preCalc, cubeSize);
                        }
                    }
                }
            }
        }
    }
}
