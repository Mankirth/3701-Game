using UnityEngine;
using System;
using System.Runtime.InteropServices;
using FMODUnity;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    public static MusicManager me;

    private Stack<string> timeWindow = new Stack<string>();
    private bool windowOpen;
    private string lastMarkerName;

    public State beatStance;

    [SerializeField]
    private EventReference music;

    [StructLayout(LayoutKind.Sequential)]
    public class TimelineInfo
    {
        public int currentBeat = 0;
        public int currentBar = 0;
        public float currentTempo = 0;
        public int currentPosition = 0;
        public float songLength = 0;
        public FMOD.StringWrapper lastMarker = new FMOD.StringWrapper(); // Gets name of marker passed on FMOD timeline, useful for tracking beat windows
    }

    public TimelineInfo timelineInfo = null;

    private GCHandle timelineHandle;

    private FMOD.Studio.EVENT_CALLBACK beatCallback; // Returns beat event from FMOD track
    private FMOD.Studio.EventDescription descriptionCallback;

 public FMOD.Studio.EventInstance musicPlayEvent;

    private void Awake()
    {
        me = this;

        musicPlayEvent = RuntimeManager.CreateInstance(music);
        musicPlayEvent.start();
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
    }

    private void Update()
    {
        musicPlayEvent.getTimelinePosition(out timelineInfo.currentPosition);

        BeatMap();

 

    }



    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    static FMOD.RESULT BeatEventCallback( FMOD.Studio.EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
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
                        var parameter = (FMOD.Studio.TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_BEAT_PROPERTIES));
                        timelineInfo.currentBeat = parameter.beat;
                        timelineInfo.currentBar = parameter.bar;
                        timelineInfo.currentTempo = parameter.tempo;
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

    private void OnGUI()
    {
        GUILayout.Box(String.Format("Current Bar = {0}, Last Marker = {1}", timelineInfo.currentBar, (string)timelineInfo.lastMarker)); // Displays FMOD markers in game window
    }

    // Frees memory when music event is finished
    void OnDestroy()
    {
        if (musicPlayEvent.isValid())
        {
            musicPlayEvent.setUserData(IntPtr.Zero);
            musicPlayEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            musicPlayEvent.release();
        }

        if (timelineHandle.IsAllocated)
            timelineHandle.Free();
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

                // Map markers to stances, defined with 0,1,2 in FMOD. Maybe find better way of mapping so it's adjustable IN ENGINE
                switch ((string)timelineInfo.lastMarker)
                {
                    case "0":
                        beatStance = State.ParryLow;
                        break;
                    case "1":
                        beatStance = State.ParryMedium;
                        break;
                    case "2":
                        beatStance = State.ParryHigh;
                        break;
                    default:
                        beatStance = State.Idle;
                        break;
                }
                Debug.Log("Window OPEN: " + beatStance.ToString());


            }
            else
            {
                windowOpen = false;
                beatStance = State.Idle;
                Debug.Log("Window Closed");
            }

        }
        Debug.Log("WINDOW COUNT: " + timeWindow.Count); // DELETE WHEN DONE TESTING
        return beatStance;
    }

}
