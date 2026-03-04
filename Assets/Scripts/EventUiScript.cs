using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EventUiScript : MonoBehaviour {

    public List<GameObject> instructions;
    public List<string> messages;
    public int currentIndex = 0;
    public TextMeshProUGUI text;

    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {

        UpdateVisibility();

    }

    // Update is called once per frame
    void Update() {

    }


    // Metodo para salir de la aplicacion

    public void ExitGame() {

        Debug.Log("Va a salir");
        Application.Quit();
        Debug.Log("Ya salio");

    }


    // Metodo para actualizar texto

    public void updateText() {



    }

    // Metodo para actualizar visibilidad de paneles

    public void UpdateVisibility(){

        for (int i = 0; i < instructions.Count; i++) {
            instructions[i].gameObject.SetActive(i == currentIndex);
        }

    }

    // Metodo para cambiar entre paneles
    public void CycleObjects(int dir_step){ 

        currentIndex = (currentIndex + dir_step) % instructions.Count;
        UpdateVisibility();

    }


    // Metodo para cambiar escena
    public void ChangeSceneByIndex(int sceneIndex) {
        SceneManager.LoadScene(sceneIndex);
    }

    public void ChangeSceneByName(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

}
