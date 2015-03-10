using System;
namespace Frankfort.VBug.Internal
{
    public delegate void ResultReadyCallback<T>(int frameNumber, T result, int streamPriority);
}