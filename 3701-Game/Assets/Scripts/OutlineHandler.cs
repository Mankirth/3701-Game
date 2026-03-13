using System.Collections;
using UnityEngine;
using System;
using Unity.Mathematics;

public class OutlineHandler : MonoBehaviour
{
    private EnemyInput enemy;
    [SerializeField]
    private GameObject outlinePrefab;
    [SerializeField]
    private MusicManager musicManager;
    private Vector3 ogPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemy = GameObject.Find("Enemy").GetComponent<EnemyInput>();
        ogPos = transform.position;
    }

    public void Launch(State state, int bufferBeats)
    {
        Debug.Log("LAUNCHING THE OUTLINE!!!");
        switch (state)
        {
            case State.ParryHigh:
                StartCoroutine(LaunchOutline(state, enemy.highParry, enemy.high, bufferBeats));
                break;
            case State.ParryMedium:
                StartCoroutine(LaunchOutline(state, enemy.medParry, enemy.medium, bufferBeats));
                break;
            case State.ParryLow:
                StartCoroutine(LaunchOutline(state, enemy.lowParry, enemy.low, bufferBeats));
                break;
            default:
                break;
        }
    }

    private IEnumerator LaunchOutline(State state, Sprite sprite, Color color, int bufferBeats)
    {
        Debug.Log("LAUNCHING THE COROUTINE OUTLINE!!! " + bufferBeats);

        float outBeat = 60 / musicManager.metroTempo * bufferBeats;
        //activate outline
        GameObject outline = Instantiate(outlinePrefab, ogPos, quaternion.identity);
        outline.transform.localScale = transform.localScale;
        outline.GetComponent<SpriteRenderer>().sprite = sprite;
        outline.GetComponent<SpriteRenderer>().color = color;
        outline.transform.position = Camera.main.transform.position + (Camera.main.transform.position - ogPos);
        
        for(float i = 0; i < outBeat; i += Time.deltaTime)
        {
            outline.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, Math.Max(0,(outBeat - (i*1.25f)) / outBeat));
            if((outline.transform.position - ogPos).magnitude > 0.1f)
                outline.transform.position = Camera.main.transform.position + ((Camera.main.transform.position - ogPos) * 0.8f) + (i / outBeat * 2 * (ogPos - Camera.main.transform.position));
            yield return null;
        }
        Debug.Log("KILLING THE OUTLINE!!!");
        Destroy(outline);
    }
}
