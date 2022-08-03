
using Unityengine;

public class Init : MonoBehaviour {
    
    // Do Optimization
    private void Awake () {
        #if UNITY_EDITOR
           Debug.logger.logEnabled = true;
        #else
           Debug.logger.logEnabled = false;
        #endif
    }
}