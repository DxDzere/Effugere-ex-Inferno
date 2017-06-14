using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Enemigo : MonoBehaviour
{

    public Transform Player;
    NavMeshAgent Agent;

    float Dist;
    public float DistanciaPerseguir;

    public bool Perseguir = false;
    public float TiempoD = 5f;

    public float TiempoCaminaR;
    private Vector3 NewPos;
    public bool MoveRandom = true;

    void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {

    }
    void Update()
    {
        Dist = Vector3.Distance(Player.position, transform.position);
        if (Dist <= DistanciaPerseguir)
        {
            Perseguir = true;
            TiempoD = 5f;
        }
        else
        {
            if (MoveRandom == true)
            {
                TiempoCaminaR -= Time.deltaTime;
                if (TiempoCaminaR <= 0)
                {
                    MoveR();
                    TiempoCaminaR = 1f;
                }
            }
            TiempoD -= Time.deltaTime;
            if (TiempoD <= 0)
            {
                TiempoD = 5f;
                Perseguir = false;
            }
        }
        if (Perseguir == true)
        {
            Agent.SetDestination(Player.position);
        }

    }
    void MoveR()
    {
        NewPos = transform.position + new Vector3(Random.onUnitSphere.x * 10, 1f, Random.onUnitSphere.z * 10);
        Agent.SetDestination(NewPos);
        Agent.speed = 5f;
    }
}
