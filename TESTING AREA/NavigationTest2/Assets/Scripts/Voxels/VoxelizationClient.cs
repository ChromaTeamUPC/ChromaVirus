using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VoxelizationClient : MonoBehaviour {

    //Behaviour variables
    public float voxelSideSize = 0.2f;
    public bool fillVoxelShell = false;

    public bool includeChildren = true;
    public bool createMultipleGrids = false;

    public Material redMaterial;
    public Material greenMaterial;
    public Material blueMaterial;
    public Material yellowMaterial;

    private List<VoxelizationServer.AABCGrid> aABCGrids;
    private ObjectPool voxelPool;
    private GameObject voxel;




    //Precalculated variables (so calculation of grid will be faster on realtime)
    Material mat;
    Transform transf;
    Renderer rend;
    SkinnedMeshRenderer sRend;
    MeshFilter meshFilter;
    List<Transform> transforms;
    List<Renderer> renderers;
    List<bool> isSkinedMeshRenderer;
    List<MeshFilter> meshFilters;
    List<Mesh> meshes;

    // Use this for initialization
    void Start () {
        voxelPool = mng.poolManager.voxelPool;
        mat = redMaterial;

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
            if (sRend != null)
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


    public void SetMaterial(ChromaColor color)
    {
        switch(color)
        {
            case ChromaColor.RED: mat = redMaterial; break;
            case ChromaColor.GREEN: mat = greenMaterial; break;
            case ChromaColor.BLUE: mat = blueMaterial; break;
            case ChromaColor.YELLOW: mat = yellowMaterial; break;
        }
    }


    public void CalculateVoxelsGrid()
    {
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
            for (int i = 0; i < renderers.Count; ++i)
            {
                if (isSkinedMeshRenderer[i])
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
    }

    public void SpawnVoxels()
    {
        if (aABCGrids != null)
        {
            foreach (VoxelizationServer.AABCGrid aABCGrid in aABCGrids)
            {
                Vector3 preCalc = aABCGrid.GetOrigin();
                for (short x = 0; x < aABCGrid.GetWidth(); ++x)
                {
                    for (short y = 0; y < aABCGrid.GetHeight(); ++y)
                    {
                        for (short z = 0; z < aABCGrid.GetDepth(); ++z)
                        {
                            if (aABCGrid.IsAABCActiveUnsafe(x, y, z))
                            {
                                Vector3 cubeCenter = aABCGrid.GetAABCCenterUnsafe(x, y, z) + preCalc;
                                voxel = voxelPool.GetObject();
                                if (voxel != null)
                                {
                                    voxel.transform.position = cubeCenter;
                                    voxel.transform.rotation = Quaternion.identity;
                                    voxel.GetComponent<Renderer>().material = mat;
                                    voxel.SetActive(true);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
