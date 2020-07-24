using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/**
 * Place this class in a sensible object - probably at root level.
 * It gets loaded once per game and generates npc prefabs across the map.
 */
public class NpcSpawner : MonoBehaviour
{
    private int numberOfNpcs = 5;
    private GameObject hero;
    private GameObject terrain;
    public GameObject npc;

    // Start is called before the first frame update
    void Start()
    {
        hero = GameObject.FindGameObjectWithTag("Hero");
        terrain = GameObject.FindGameObjectWithTag("Terrain");

        for (int i = 0; i < numberOfNpcs; i++)
        {
            Instantiate(npc, getRandomLocation(), Quaternion.FromToRotation(transform.position, hero.transform.position));
        }
    }

    private Vector3 getRandomLocation()
    {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

        // Pick the first indice of a random triangle in the nav mesh
        int t = Random.Range(0, navMeshData.indices.Length - 3);

        // Select a random point on it
        Vector3 point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]], navMeshData.vertices[navMeshData.indices[t + 1]], Random.value);
        Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t + 2]], Random.value);

        point.y += 1;
        return point;
    }
}
