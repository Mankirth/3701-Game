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

    public PlayerSettings settings;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemy = GameObject.Find("Enemy").GetComponent<EnemyInput>();
        ogPos = transform.position;
    }

    public void Launch(State state, int bufferBeats, bool isFient)
    {
        switch (state)
        {
            case State.ParryHigh:
                StartCoroutine(LaunchOutline(enemy.highParry, isFient? Color.gray:settings.highColor, bufferBeats));
                break;
            case State.ParryMedium:
                StartCoroutine(LaunchOutline(enemy.medParry, isFient? Color.gray:settings.medColor, bufferBeats));
                break;
            case State.ParryLow:
                StartCoroutine(LaunchOutline(enemy.lowParry, isFient? Color.gray:settings.lowColor, bufferBeats));
                break;
            default:
                break;
        }
    }

    private IEnumerator LaunchOutline(Sprite sprite, Color color, int bufferBeats)
    {
        float outBeat = 60 / musicManager.metroTempo * bufferBeats;
        //activate outline
        GameObject outline = Instantiate(outlinePrefab, ogPos, quaternion.identity);
        outline.transform.localScale = transform.localScale;
        outline.GetComponent<SpriteRenderer>().sprite = sprite;
        outline.GetComponent<SpriteRenderer>().color = color;
        outline.transform.position = Camera.main.transform.position;// + (Camera.main.transform.position - ogPos);
        
        for(float i = 0; i < outBeat; i += Time.deltaTime)
        {
            outline.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, settings.SetOutline(i * 1.1f, outBeat));
            if((outline.transform.position - ogPos).magnitude > 0.8f)
                outline.transform.position = Camera.main.transform.position + (i / outBeat * 1 * (ogPos - Camera.main.transform.position));
            yield return null;
        }
        Destroy(outline);
    }
}
