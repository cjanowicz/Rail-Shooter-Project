using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class LevelManager : MonoBehaviour {

    public Flowchart myFlowChart;
    public EnemyManager myEnemyManager;
    /// <Speed Floats>
    /// These public floats are accessed by other scripts to determine how fast the objects are coming at the camera. It can be used to affect how the player and enemy and screenshake looks later.
    /// </summary>
    public float zSpeed = 10;
    public float xSpeed = 0;

    /// <Event Array>
    /// Spawn drones,
    /// </summary>
    public enum StoryEventType {SpawnDrones3, SpawnBombers2, SpawnBoss, StoryBlock1, StoryBlock2, TerrainChangeDesert, TerrainChangeForest, DoNothing };
    public enum ProgressCondition { AllEnemiesDestroyed, StoryBlockFinished, WaitForTime, DoNextImmediately, BossDestroyed};

    public bool storyLoops = false;


    /// <Event Array>
    /// An event list of some sort is necessary. 
    /// An array of enums! Enum: SpawnDrones1, SpawnBombers2, StoryBlock1, TerrainChangeForest, TerrainChangeDesert, TerrainChangeCity
    /// maybe a 2D array for the condition to proceed to the next event. IE: first enum is the event type, the second enum is the conditions to proceed to the next iteration
    /// OK! An iterator, and a "returnWaveDestroyed" and a "returnBlockFinished" or "returnAfterTime"
    /// Just an array of level elements and events accessed linearly, triggering one after the other
    /// 
    /// </summary>

    /// <summary>
    /// This will appear as a list in the spector with drop down functionality.
    /// </summary>
    [System.Serializable]
    public class StoryEvent
    {
        public StoryEventType storyEvent;
        public ProgressCondition progressCondition;
    }
    public bool enemiesDestroyedFlag = false;
    public bool bossDestroyedFlag = false;
    public bool flowchartFinishedFlag = false;


    [SerializeField]
    private const int storyArraySize = 10;

    public StoryEvent[] myStoryEventArray = new StoryEvent[storyArraySize];


    [SerializeField]
    private int storyIterator = 0;

    public bool demoStoryLoops = false;

    // Use this for initialization
    void Start () {

        //Call first event, then let it re-call itself
        StartLevelEvent();
    }
    
	
	// Update is called once per frame
	void Update () {
		
	}

    void StartLevelEvent()
    {
        if(storyIterator >= myStoryEventArray.Length) {
            if(demoStoryLoops == true) {
                storyIterator = 0;
            } else {
                Debug.Log("Level End Sequence goes here. About to hit a null ref unless you prevent the level event array from trying to access a null spot.");
                //Start level complete sequence
            }
        }
        switch (myStoryEventArray[storyIterator].storyEvent)
        {
            case StoryEventType.SpawnDrones3:
                myEnemyManager.CreateEnemies();
                //Spawn drone wave
                break;
            case StoryEventType.SpawnBombers2:
                //Spawn bomber wave
                break;
            case StoryEventType.SpawnBoss:
                //spawn boss
                myEnemyManager.SpawnSkullBoss();
                break;
            case StoryEventType.StoryBlock1:
                myFlowChart.ExecuteBlock("FirstStoryBlock");
                break;
            case StoryEventType.StoryBlock2:
                myFlowChart.ExecuteBlock("SecondStoryBlock");
                break;
            case StoryEventType.TerrainChangeDesert:
                //change terrain
                break;
            case StoryEventType.TerrainChangeForest:
                //change terrain
                break;
        }
        checkProgressEvent();
    }
    
    void checkProgressEvent()
    {
        switch (myStoryEventArray[storyIterator].progressCondition)
        {
            case ProgressCondition.DoNextImmediately:
                storyIterator++;
                StartLevelEvent();
                break;

            case ProgressCondition.AllEnemiesDestroyed:
                ///SO! The enemy handler I have already can tell when a wave is destroyed
                ///Need to make it so that when waves that I specify (AND ONLY THOSE) are destroyed
                ///It shouts back "this particular enemy group was destroyed!"
                enemiesDestroyedFlag = true;
                break;

            case ProgressCondition.StoryBlockFinished:
                flowchartFinishedFlag = true;
                break;

            case ProgressCondition.WaitForTime:
                StartCoroutine(WaitThreeSeconds());
                break;

            case ProgressCondition.BossDestroyed:
                ///SO! The enemy handler I have already can tell when a wave is destroyed
                ///Need to make it so that when waves that I specify (AND ONLY THOSE) are destroyed
                ///It shouts back "this particular enemy group was destroyed!"
                bossDestroyedFlag = true;
                break;
        }
    }

    IEnumerator WaitThreeSeconds()
    {
        yield return new WaitForSeconds(3);
        storyIterator++;
        StartLevelEvent();
    }

    public void DroneWaveDestroyed()
    {
        if(enemiesDestroyedFlag == true)
        {
            storyIterator++;
            Invoke("StartLevelEvent",1);
            enemiesDestroyedFlag = false;
        }
    }

    public void BossDestroyed()
    {
        if (bossDestroyedFlag == true)
        {
            storyIterator++;
            Invoke("StartLevelEvent", 1);
            bossDestroyedFlag = false;
        }
    }

    public void FlowchartBlockFinished()
    {
        if (flowchartFinishedFlag == true)
        {
            storyIterator++;
            Invoke("StartLevelEvent", 1);
            flowchartFinishedFlag = false;
        }
    }
}
