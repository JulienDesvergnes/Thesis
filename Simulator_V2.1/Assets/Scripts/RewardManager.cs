using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{

    public float rewardFromModelGlobal = 0.0f;
    public float rewardFromModelLocal = 0.0f;
    public float rewardFromSubject = 0.0f;
    public float rewardFromObs = 0.0f;

    public float CumulativeReward = 0.0f;
 
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public float getTotalReward() {
        return rewardFromModelGlobal + rewardFromModelLocal + rewardFromSubject + rewardFromObs;
    }

    public void addRewardFromModelGlobal(float r) {
        rewardFromModelGlobal += r;
        CumulativeReward += r;
    }

    public void addRewardFromModelLocal(float r) {
        rewardFromModelLocal += r;
        CumulativeReward += r;
    }

    public void addRewardFromSubject(float r) {
        rewardFromSubject += r;
        CumulativeReward += r;
    }

    public void addRewardFromObs(float r) {
        rewardFromObs += r;
        CumulativeReward += r;
    }

    public void reset() {
        resetOnlyReward();

        CumulativeReward = 0.0f;
    }

    public void resetOnlyReward() {
        rewardFromModelGlobal = 0.0f;
        rewardFromModelLocal = 0.0f;
        rewardFromSubject = 0.0f;
        rewardFromObs = 0.0f;
    }
}
