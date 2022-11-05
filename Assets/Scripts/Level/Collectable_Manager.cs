using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Collectable_Manager : MonoBehaviour
{
    public static Collectable_Manager instance;

    [SerializeField] GameObject cubePrefab;
    [SerializeField] bool SpawnCubes;
    [SerializeField] bool startWithPattern = true;
    [SerializeField] float cubeSpawnRateInSecond = 1f;
    [SerializeField] List<Material> colors;

    public List<GameObject> cubes;
    int cubeCount;

    public event EventHandler OnAllCubesCollected;

    private void Awake()
    {
        instance = this;

        if (startWithPattern)
        {
            cubes = new List<GameObject>();

            cubeCount = gameObject.transform.childCount;

            for (int i = 0; i < cubeCount; i++)
            {
                cubes.Add(transform.GetChild(i).gameObject);
            }
        }
        else GenerateCubes(50);

        Debug.Log("Cube count : " + cubeCount);
    }

    void Start()
    {
        Listener();

        if (SpawnCubes) StartCoroutine(SpawnCube());
    }

    void Listener()
    {
        Level_Manager.instance.OnLevelEnded += Event_OnLevelEnded;
    }

    private void Event_OnLevelEnded(object sender, EventArgs e)
    {
        StopAllCoroutines();
    }

    private void GenerateCubes(int amount)
    {
        cubes.Clear();
        cubeCount = 0;

        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < amount; i++)
        {
            CreateCube();
        }
    }

    public void CubeCollected()
    {
        cubeCount--;
        cubes.Clear();
        for (int i = 0; i < cubeCount; i++)
        {
            cubes.Add(transform.GetChild(i).gameObject);
        }

        Debug.Log("Collectable Manager -> Cube Collected. Cube count : " + cubeCount);

        if (cubeCount < 0) cubeCount = 0;

        if (cubeCount == 0) OnAllCubesCollected?.Invoke(this, EventArgs.Empty);
    }

    public void CreateCube()
    {
        cubeCount++;

        float rng_xPos = UnityEngine.Random.Range(-3.5f, 3.5f);
        float rng_yPos = UnityEngine.Random.Range(0.1f, 0.9f);
        float rng_zPos = UnityEngine.Random.Range(-1.6f, 1.6f);
        int rng_color_value = UnityEngine.Random.Range(0, colors.Count);

        Vector3 randomSpawnLocation = new Vector3(rng_xPos, rng_yPos, rng_zPos);

        GameObject cube = Instantiate(cubePrefab, randomSpawnLocation, Quaternion.identity, this.transform);
        cube.GetComponent<MeshRenderer>().material = colors[rng_color_value];
        cubes.Add(cube);
    }

    public void DestroyCube()
    {
        int rng_value = UnityEngine.Random.Range(0,cubeCount);
        
        cubeCount--;
        cubes.Remove(transform.GetChild(rng_value).gameObject);

        Destroy(transform.GetChild(rng_value).gameObject);
    }

    IEnumerator SpawnCube()
    {
        CreateCube();

        yield return new WaitForSeconds(cubeSpawnRateInSecond);

        StartCoroutine(SpawnCube());
    }
}

[CustomEditor(typeof(Collectable_Manager))]
public class Collectable_Manager_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Collectable_Manager col_manager = (Collectable_Manager)target;
        if (GUILayout.Button("Create Cube")) col_manager.CreateCube();
        if (GUILayout.Button("Destroy Cube")) col_manager.DestroyCube();
    }
}