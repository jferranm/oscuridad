using System;
namespace Frankfort.VBug.Internal
{


    //--------------- Workload Execution --------------------
    [Serializable]
    public enum WorkloadExecutorType
    {
        waitForEndOfFrame,
        fixedUpdate,
        update,
        lateUpdate,
        gui,
        thread
    } 
    //--------------- Workload Execution --------------------
			

    
	//--------------- Data Processing --------------------
    [Serializable]
    public enum ProcessHandlingType
    {
        none = 0,
        saveToDisk = 1,
        streamToEditor = 2,
        streamToServer = 4
    }
	//--------------- Data Processing --------------------
			




    //--------------- GAME OBJECT REFLECTION --------------------

    [Serializable]
    public enum TypeClassification
    {
        unknown,
        primitive,
        collection,
        unityObj,
        obj,
    }

    public enum UnityObjectType
    {
        unityObject,
        texture,
        material
    }

    //--------------- GAME OBJECT REFLECTION --------------------
			
}
