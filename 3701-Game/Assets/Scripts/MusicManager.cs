using FMOD.Studio;
using FMODUnity;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public static MusicManager me;

    private Stack<string> timeWindow = new Stack<string>();
    private bool windowOpen;
    private string lastMarkerName;

    public List<BeatEvent> beatEvents = new List<BeatEvent>();
    private Dictionary<int, (State, int)> beatmap = new Dictionary<int, (State, int)>();


    public State beatStance;
    public int beatInterval;


    public float timeInterval;


    [SerializeField]
    private EventReference music;

    [SerializeField]
    private GameMenu gameMenu;


    public float metroBeat;
    public float metroTempo;

    public delegate void BeatEventDelegate();
    public static event BeatEventDelegate beatUpdated;

    public delegate void MarkerListenerDelegate();
    public static event MarkerListenerDelegate markerUpdated;

    public static int lastBeat = 0;
    public static string lastMarkerString = null;

    private bool gameOver;

    private readonly int outlineBufferBeats = 2;
    [SerializeField]
    private GameObject outlineHandler;

    [SerializeField]
    private PlayerSettings settings;
    private float gameSpeed;

    [StructLayout(LayoutKind.Sequential)]
    public class TimelineInfo
    {
        public int beatMapIndex = 0;
        public int totalBeat = 0;
        public int currentBeat = 0;
        public int currentBar = 0;
        public float currentTempo = 0;
        public int currentPosition = 0;
        public float songLength = 0;
        public int nextAvailBeat = 1;
        public int nextAvailOutlineBeat = 1;
        public int beatMapOutlineIndex = 0;
        public FMOD.StringWrapper lastMarker = new FMOD.StringWrapper(); // Gets name of marker passed on FMOD timeline, useful for tracking beat windows
    }

    public TimelineInfo timelineInfo = null;

    private GCHandle timelineHandle;

    private FMOD.Studio.EVENT_CALLBACK beatCallback; // Returns beat event from FMOD track
    private FMOD.Studio.EventDescription descriptionCallback;
    [SerializeField]
    private Slider songSlider;
    [SerializeField]
    private TMP_Text songProgress;
    private SfxManager sfxManager;

    public FMOD.Studio.EventInstance musicPlayEvent;

    private void Awake()
    {
        me = this;

        musicPlayEvent = RuntimeManager.CreateInstance(music);
        musicPlayEvent.start();

        beatmap.Clear();
        for (int i = 0; i < beatEvents.Count; i++)
        {
            beatmap[i] = (beatEvents[i].stance, beatEvents[i].interval);
        }
    }

    private void Start()
    {
        timelineInfo = new TimelineInfo(); // Holds track information
        beatCallback = new FMOD.Studio.EVENT_CALLBACK(BeatEventCallback);

        timelineHandle = GCHandle.Alloc(timelineInfo, GCHandleType.Pinned);
        musicPlayEvent.setUserData(GCHandle.ToIntPtr(timelineHandle));
        musicPlayEvent.setCallback(beatCallback, FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT | FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER);

        musicPlayEvent.getDescription(out descriptionCallback);
        descriptionCallback.getLength(out int length);

        timelineInfo.songLength = length;
        timeWindow.Clear();

        sfxManager = GameObject.Find("SfxManager").GetComponent<SfxManager>();
    }

    private void Update()
    {
        musicPlayEvent.getTimelinePosition(out timelineInfo.currentPosition);

        //BeatMap();

        if (lastBeat != timelineInfo.currentBeat)
        {
            lastBeat = timelineInfo.currentBeat;

            if (beatUpdated != null)
            {
                beatUpdated();
            }
        }
        switch (settings.gameSpeed)
        {
            case PlayerSettings.GameSpeed.Normal:
                gameSpeed = 1.0f;
                break;
            case PlayerSettings.GameSpeed.Double:
                gameSpeed = 2.0f;
                break;
        }

        musicPlayEvent.setPitch(gameSpeed);

        if (!IsPlaying(musicPlayEvent) && !gameOver)
        {
            gameOver = true;
            Debug.Log("IT'S OVER");
            sfxManager.QueueSound(false, sfxManager.enemyHit);
            gameMenu.EndGame(true);
        }

        
        
        metroBeat = timelineInfo.currentPosition * timelineInfo.currentTempo / 60000f; // Exact beat (with decimals)
        metroTempo = timelineInfo.currentTempo;

        songSlider.value = timelineInfo.currentPosition / timelineInfo.songLength;
        songProgress.text = Math.Round(songSlider.value * 100, 0) + "%";

    }



    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    FMOD.RESULT BeatEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
    {
        FMOD.Studio.EventInstance instance = new FMOD.Studio.EventInstance(instancePtr);
        IntPtr timelineInfoPtr;
        FMOD.RESULT result = instance.getUserData(out timelineInfoPtr);

        if (result != FMOD.RESULT.OK)
        {
            Debug.LogError("Timeline Callback error: " + result);
        }
        else if (timelineInfoPtr != IntPtr.Zero) //System(IntPtr) for garbage collection
        {
            GCHandle timelineHandle = GCHandle.FromIntPtr(timelineInfoPtr);
            TimelineInfo timelineInfo = (TimelineInfo)timelineHandle.Target;

            switch (type)
            {
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT:
                    {
                        if(Time.timeScale != 1)
                            break;
                        var parameter = (FMOD.Studio.TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_BEAT_PROPERTIES));
                        timelineInfo.totalBeat++;
                        try
                        {
    
                            beatStance = beatmap[timelineInfo.beatMapIndex].Item1;
                            beatInterval = beatmap[timelineInfo.beatMapIndex].Item2;

                
                            //enemy windup
                            if (timelineInfo.totalBeat == timelineInfo.nextAvailBeat)
                            {
                                if(beatmap[timelineInfo.beatMapIndex + 1].Item1 != State.Idle && beatmap[timelineInfo.beatMapIndex + 1].Item1 != State.Hurting)
                                    GameObject.Find("Enemy").GetComponent<EnemyInput>().StartAttack(beatStance, beatInterval/gameSpeed, true);
                                else
                                    GameObject.Find("Enemy").GetComponent<EnemyInput>().StartAttack(beatStance, beatInterval/gameSpeed, false);
                                timelineInfo.nextAvailBeat = timelineInfo.totalBeat + beatInterval;
                                timelineInfo.beatMapIndex++;
                            }
                            else
                                sfxManager.QueueSound(true, sfxManager.metronome);
                            //spawn outline with set window
                            if(timelineInfo.totalBeat + outlineBufferBeats >= timelineInfo.nextAvailOutlineBeat + beatmap[timelineInfo.beatMapOutlineIndex].Item2)
                            {
                                outlineHandler.GetComponent<OutlineHandler>().Launch(beatmap[timelineInfo.beatMapOutlineIndex].Item1, outlineBufferBeats);
                                timelineInfo.nextAvailOutlineBeat += beatmap[timelineInfo.beatMapOutlineIndex].Item2;
                                timelineInfo.beatMapOutlineIndex++;
                            }
                        }
                        catch
                        {
                            Debug.Log("Beatmap Array Ran Out");
                        }
                        timelineInfo.currentBeat = parameter.beat;
                        timelineInfo.currentBar = parameter.bar;
                        timelineInfo.currentTempo = parameter.tempo;
                        //Debug.Log(timelineInfo.currentBar);
                    }
                    break;
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER:
                    {
                        var parameter = (FMOD.Studio.TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_MARKER_PROPERTIES));
                        timelineInfo.lastMarker = parameter.name;
                    }
                    break;
            }
        }
        return FMOD.RESULT.OK;
    }

    //private void OnGUI()
    //{
    //    GUILayout.Box(String.Format("Current Bar = {0}, Last Marker = {1}", timelineInfo.currentBar, (string)timelineInfo.lastMarker)); // Displays FMOD markers in game window
    //}

    void OnDestroy()
    {
        if (musicPlayEvent.isValid())
        {
            musicPlayEvent.setUserData(IntPtr.Zero);
            musicPlayEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            musicPlayEvent.release();
            //Debug.Log("END OVER AHH");
        }

        if (timelineHandle.IsAllocated)
            timelineHandle.Free();
    }

    public static bool IsPlaying(EventInstance musicPlayEvent)
    {
        PLAYBACK_STATE state;
        musicPlayEvent.getPlaybackState(out state);
        return state != PLAYBACK_STATE.STOPPED;
    }

    public bool PhaseChange()
    {
        string marker = (string)timelineInfo.lastMarker;
        if (marker == "PHASE")
        {
            return true;
        }
        return false;
    }

    public bool SongEnd()
    {
        //string marker = (string)timelineInfo.lastMarker;
        if (beatStance == State.Hurting)
        {
            return true;
        }
        return false;
    }
    public State BeatMap()
    {
        //OLD METHOD: Stack with two markers, go by marker name. I.e. 1.1 Is the start of beat 1, 1.2 is the end of beat 1 and so on. 2.1 start of beat 2, 2.2 end of beat 2.
        //Collect first marker, person has to press button before a second marker is added, If second marker is added, clear the stack

        // Used bool instead, window closes when new marker name does not correspond with old marker name. DELETE THESE COMMENTS AFTER
        if ((string)timelineInfo.lastMarker != lastMarkerName && !string.IsNullOrEmpty(timelineInfo.lastMarker))
        {
            //timeWindow.Push((string)timelineInfo.lastMarker);
            lastMarkerName = (string)timelineInfo.lastMarker;

            if (!windowOpen)
            {
                windowOpen = true;
                string marker = (string)timelineInfo.lastMarker;
                // Map markers to stances, defined with 0,1,2 in FMOD. Maybe find better way of mapping so it's adjustable IN ENGINE
                switch (marker)
                {
                    case { } m when m.StartsWith("0"):
                        beatStance = State.ParryLow;
                        break;
                    case { } m when m.StartsWith("1"):
                        beatStance = State.ParryMedium;
                        break;
                    case { } m when m.StartsWith("2"):
                        beatStance = State.ParryHigh;
                        break;
                    default:
                        beatStance = State.Idle;
                        break;
                }
                //Debug.Log("Window OPEN: " + beatStance.ToString());

                switch (marker)
                {
                    case { } m when m.EndsWith("A"):
                        timeInterval = 1.5f;
                        break;
                    case { } m when m.EndsWith("B"):
                        timeInterval = 1f;
                        break;
                    case { } m when m.EndsWith("C"):
                        timeInterval = 0.5f;
                        break;
                    default:
                        timeInterval = 2.0f;
                        break;
                }
            }
            else
            {
                //GameObject.Find("Judge").GetComponent<Judge>().Evaluate();
                windowOpen = false;
                beatStance = State.Idle;
                //Debug.Log("Window Closed");
            }

        }
        Debug.Log("WINDOW COUNT: " + timeWindow.Count); // DELETE WHEN DONE TESTING
        return beatStance;
    }

}
