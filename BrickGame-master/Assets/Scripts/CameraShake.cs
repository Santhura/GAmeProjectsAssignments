using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    // transform of the camrea to shake. Grabs the gameObject's transform
    public Transform camTransform;

    //how long the object should shake for.
    public float shakeDuration = 0;

    // amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    private Vector3 originalPos;
    public static bool isHit;
    void Awake()
    {
        isHit = false;
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

   
	// Update is called once per frame
	void Update ()
    {
        if(isHit)
        {
            Shake();
        }
    }

    public void Shake()
    {
        if (GameManager.CurrentGameState == GameManager.GameState.Playing)
        {
            if (shakeDuration > 0)
            {
                camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

                shakeDuration -= Time.deltaTime * decreaseFactor;
            }
            else
            {
                shakeDuration = 0.3f;
                camTransform.localPosition = originalPos;
                isHit = false;
            }
        }
    }
}
