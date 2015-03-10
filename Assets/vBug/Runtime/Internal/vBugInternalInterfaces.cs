using System;
using System.Collections.Generic;
using UnityEngine;

namespace Frankfort.VBug.Internal
{
    public interface IAbortableWorkObject
    {
        bool isAborted { get; set; }
    }

    public interface IHumanInputProvider
    {
        void Reset();
        void InitCurrentFrame(); //Its called during Update
        HumanInputProviderData GetResultEOF(); //Its called during WaitForEndOfFrame
        void Dispose();
    }

    public interface IMeshDataProvider
    {
        void GetResultEOF(ref int streamPriority, GameObject go, MeshCaptureMethod captureMethod, bool forceKeyframe, List<SMesh> storeTo);
        void Dispose();
    }

    public interface IParticleDataProvider
    {
        void GetResultEOF(ref ParticleDataSnapshot storeTo); //Its called during WaitForEndOfFrame, streamPriority is used to sort the vertical slices when writing to the disk or wifi
        void Dispose();
    }
}
