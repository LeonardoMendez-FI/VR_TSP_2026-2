using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections; //Optional
using System.Collections.Generic;
using JetBrains.Annotations; //Optional

public class UISelection : MonoBehaviour
{

    public static bool gazedAt;
    [SerializeField]
    public float fillTime = 5f;
    public Image radialImage;
    public UnityEvent onFillComplete; //Evento generico que se asigna al termnar la carga


    //Proceso asincrono
    private Coroutine fillCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gazedAt = false;
        radialImage.fillAmount = 0;

    }

    public void OnPointerEnter() {
        gazedAt = true;

        if(fillCoroutine != null) {

            StopCoroutine(fillCoroutine);
        }

        fillCoroutine = StartCoroutine(FillRadial());

    }

    private IEnumerator FillRadial() {

        float elapsedTime = 0f;

        while (elapsedTime < fillTime) {

            if (!gazedAt) { //Se dejo de ver el boton
                yield break;
            }

            elapsedTime += Time.deltaTime;
            radialImage.fillAmount = Mathf.Clamp01(elapsedTime/fillTime);

            yield return null;
        }

        //Evento a ejecutar
        onFillComplete?.Invoke();
    }

    public void OnPointerExit() {
        gazedAt = false;
        if (fillCoroutine != null) {
            StopCoroutine(fillCoroutine);
            fillCoroutine = null;
        }
        radialImage.fillAmount = 0f; //Reinicia el llenado a 0
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
