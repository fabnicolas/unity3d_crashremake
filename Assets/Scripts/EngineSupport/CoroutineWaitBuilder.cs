 using UnityEngine;
 
 public class CoRoutineWaitBuilder{
    public WaitForEndOfFrame m_WaitForEndOfFrame;
    public WaitForFixedUpdate m_WaitForFixedUpdate;

    private static class SingletonLoader {
		public static readonly CoRoutineWaitBuilder instance = new CoRoutineWaitBuilder();
	}
	public static CoRoutineWaitBuilder getInstance() {
		return SingletonLoader.instance;
	}

    public CoRoutineWaitBuilder(){
        m_WaitForEndOfFrame = new WaitForEndOfFrame();
        m_WaitForFixedUpdate= new WaitForFixedUpdate();
    }
 }