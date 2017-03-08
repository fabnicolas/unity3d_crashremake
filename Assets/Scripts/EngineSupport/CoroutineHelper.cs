using UnityEngine;
using System;
using System.Collections;

/*
    This class optimizes and semplifies Coroutine management.
 */
public class CoroutineHelper
{
    public WaitForEndOfFrame m_WaitForEndOfFrame;   // Cached object for faster yields.
    public WaitForFixedUpdate m_WaitForFixedUpdate; // As above.

    // Singleton pattern.
    private static class SingletonLoader
    {
        public static readonly CoroutineHelper instance = new CoroutineHelper();
    }
    public static CoroutineHelper getInstance()
    {
        return SingletonLoader.instance;
    }

    /*
        Constructor of CoroutineHelper.
     */
    public CoroutineHelper()
    {
        m_WaitForEndOfFrame = new WaitForEndOfFrame();      // ONLY first time, CACHE the object to use for next times!
        m_WaitForFixedUpdate = new WaitForFixedUpdate();    // As above.
    }

    /*
        This function provides frame-indipendent computation in easy-accessible way thanks to delegate functions (callback).
        Weight value determines the animation progress.
        Callback sent MUST have a float parameter for retrieving the weight.

        Usage example:
        T foo = INITIAL_VALUE;      // T usually float, int
        StartCoroutine(
            CoroutineHelper.executeInTime(ANIMATION_DURATION, (float weight_changing_through_time)=>{
                foo = -(T)Mathf.Lerp(0,100,weight_changing_through_time);
            })
        );
     */
    public static IEnumerator executeInTime(float animation_duration, Action<float> callback_onFrameComputationDone)
    {
        float weight = 1;   // This weight decreases through time; if reaches 0, animation finished.
        while (weight > 0)
        {
            weight -= Time.deltaTime / animation_duration;  // Decrease animation weight based on deltaTime/animationTime.
            callback_onFrameComputationDone(1-weight); // Callback the weight value.
            // MAYBE TODO: return directly Mathf.Lerp(start_value, end_value, weight) in another func.
            yield return null;
        }
    }
}