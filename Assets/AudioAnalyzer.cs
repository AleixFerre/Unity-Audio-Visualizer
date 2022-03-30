using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioAnalyzer : MonoBehaviour
{
    public FFTWindow window = FFTWindow.Blackman;
    [Range(100f, 20000f)]
    public float visualizationVolumeMultiplier = 1000f;

    private const int DIMENSION = 256;

    private float[] spectrum = new float[DIMENSION];
    private AudioSource source;
    private GameObject[] cubes = new GameObject[DIMENSION];

    private void Start()
    {
        source = GetComponent<AudioSource>();
        for (int i = 0; i < spectrum.Length - 1; i++)
        {
            cubes[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Destroy(cubes[i].GetComponent<Collider>());
            cubes[i].transform.position = new Vector3(i / 2f, 0, 0);
        }

        Vector3 camPos = Camera.main.transform.position;
        camPos.x = cubes[DIMENSION / 2].transform.position.x;
        camPos.y = 30f;
        Camera.main.transform.position = camPos;
    }

    void Update()
    {
        source.GetSpectrumData(spectrum, 0, window);

        for (int i = 0; i < spectrum.Length - 1; i++)
        {
            cubes[i].transform.localScale = new Vector3(.5f, -Mathf.Min(Mathf.Log(1f - spectrum[i]) * i * visualizationVolumeMultiplier, 100f), .5f);
            Vector3 pos = cubes[i].transform.position;
            pos.y = cubes[i].transform.localScale.y / 2;
            cubes[i].transform.position = pos;
        }
    }

}
