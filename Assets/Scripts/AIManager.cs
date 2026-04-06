using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class AIManager : MonoBehaviour
{

    [SerializeField]
    public GameObject player;
    [SerializeField]
    public Transform entrance;
    [SerializeField]
    public Transform exit;

    public float detectionRange = 1f;
    public float exitRange = 2f;
    public float minDistanceFromEntrance = 15f;

    List<NavMeshAgent> agents = new List<NavMeshAgent>();

    NavMeshTriangulation triangulation;
    Vector3 entrancePos;

    float detectionRangePow;
    float exitRangePow;
    float minDistancePow;

    bool gameWon = false;

    System.Random random = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        entrancePos = entrance.position;
        triangulation = NavMesh.CalculateTriangulation();

        detectionRangePow = detectionRange * detectionRange;
        exitRangePow = Mathf.Pow(exitRange, 2);
        minDistancePow = Mathf.Pow(minDistanceFromEntrance, 2);
        
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 playerPos = player.transform.position;

        // Detection of enemies in player´s position

        bool playerCaught = false;

        foreach (var agent in agents) {

            if(!agent.enabled) continue;

            if ((agent.transform.position - playerPos).sqrMagnitude < detectionRangePow) {

            }

        playerCaught = true;

        }
        
    }

    // Method to move the player at the entrance

    void TeleportPlayerEntrance(){

        var cc = player.GetComponent<NavMeshAgent>();

        if (cc != null) {
            cc.enabled = true;
        }

        Debug.Log("Teletransport: " + entrancePos);

    }

    //Method to relocate the enemies
    void RelocateAllNPC() {

        if (triangulation.vertices.Length == 0) return;

        foreach (var agent in agents) {

            agent.enabled = false;
            agent.transform.position = GetValidRandomPosition();
            agent.enabled = true;


        }

    }

    //Method to calculate the valid enemie´s position

    Vector3 GetValidRandomPosition() {

        Vector3 pos;

        do {

            int i = random.Next(0, triangulation.indices.Length / 3) * 3;

            Vector3 v1 = triangulation.vertices[triangulation.indices[i]];
            Vector3 v2 = triangulation.vertices[triangulation.indices[i+1]];
            Vector3 v3 = triangulation.vertices[triangulation.indices[i+2]];

            float r1 = (float)random.NextDouble();
            float r2 = (float)random.NextDouble();

            if(r1+r2 > 1f) {

                r1 = 1f - r1;
                r2 = 1f - r2;

            }

            pos = v1 + r1*(v2-v1) + r2*(v3-v1);

        } while ((pos - entrancePos).sqrMagnitude < minDistancePow);

        return pos;

    }

    void FindAllEnemies() {

        agents.Clear();

        foreach(var agent in FindObjectsByType<NavMeshAgent>(FindObjectsSortMode.None)) {

            if (agent.CompareTag("Enemy")) {
                agents.Add(agent);
            }

        }

    }

}
