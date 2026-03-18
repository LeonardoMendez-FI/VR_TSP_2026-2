using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class Concurrencia : MonoBehaviour
{

    [Header("Active the Methods")]
    public bool useSyncrone;
    public bool useThread;
    public bool useTask;
    public bool useCoroutine;

    [Header("Sphere to Move")]
    public Transform syncroneSphere;
    public Transform threadSphere;
    public Transform taskSphere;
    public Transform coroutineSphere;

    public Transform mainCube;


    // Secondary thread actions to execute

    private Queue<Action> mainThreadActions = new Queue<Action>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        if(useSyncrone) SyncroneMove();
        if(useThread) MoveWithThread();
        if(useTask) MoveWithTask();
        if(useCoroutine) StartCoroutine(MoveWithCoroutine());

    }

    // Update is called once per frame
    void Update()
    {

        // The reference cube always spin
        mainCube.Rotate(Vector3.up, 50*Time.deltaTime);

        // Execute the main thread actions
        lock (mainThreadActions) {

            while (mainThreadActions.Count > 0){
                mainThreadActions.Dequeue().Invoke();
            }

        }

    }

    // Methods for concurrence tools

    void SyncroneMove() {

        for (int i = 0; i < 100; i++) {

            syncroneSphere.position += Vector3.right * 0.05f;

        }

        Thread.Sleep(50);

    }


    // Secondary Thread move

    void MoveWithThread() {

        new Thread(() => {

            for (int i = 0; i <= 100; i++) {

                Thread.Sleep(50);

                lock (mainThreadActions) {

                    mainThreadActions.Enqueue(() => {

                        threadSphere.position += Vector3.right * 0.05f;

                    });

                }
                
           }

        }).Start();

    }

    // Methods win async task

    async void MoveWithTask() {

        await Task.Run(() => {
            
            for(int i = 0; i <= 100; i++) {
                
                Thread.Sleep(50);

                lock (mainThreadActions) {

                    mainThreadActions.Enqueue(() => {

                        taskSphere.position += Vector3.right * 0.05f;

                    });
                }

            }

        });

    }

    // Corroutine

    IEnumerator MoveWithCoroutine() {

        for (int i = 0; i < 100; i++) {
            coroutineSphere.position += Vector3.right * 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
