using System;

namespace Frankfort.VBug
{



    //--------------- VERTICAL SLICE --------------------
    [Serializable]
    public enum DataFormatType
    {
        /// <summary>
        /// Twice as fast as binaryLZF, but 2.5 times the size !
        /// </summary>
        binaryRaw,
        
        /// <summary>
        /// Great combo between filesizes and speed
        /// </summary>
        binaryLZF,

        /// <summary>
        /// XML Serializer is a factor 100(!) times slower then the binairy formatter!
        /// </summary>
        xmlRaw,

        /// <summary>
        /// XML Serializer is a factor 100(!) times slower then the binairy formatter!
        /// </summary>
        xmlLZF
    }
    //--------------- VERTICAL SLICE --------------------
			




    //--------------- SCREEN CAPTURE --------------------
    [Serializable]
    public enum ScreenCropMethod
    {
        none,
        POT,
        squarePOT
    }


    [Serializable]
    public enum ScreenCaptureMethod
    {
        /*
        /// <summary>
        /// This type uses the Camera.targetTexture in order to redirect the rendered pixels.
        /// 
        /// Pro's:
        ///     - Fastest method available.
        ///     - No re-render required
        ///     - uses Graphics.Blit() if the capture-resolution is lower than the Screen-res.
        ///     
        /// Cons:
        ///     - PRO-licence only
        ///     - GUI excluded
        ///     - Could cause visual artifacts like bending (due to 16-bits precision)
        ///     - Not compatible with custom MainCamera targetTexture usage.
        /// </summary>
        redirectTargetBuffer,


        /// <summary>
        /// This type uses the active rendertexture.
        /// Pro's:
        ///   - Fast (on Average 30/45% slower then 'redirectTargetBuffer')
        ///   - No re-render required
        ///   - uses Graphics.Blit() if the capture-resolution is lower than the Screen-res.
        /// 
        /// Con's
        ///   - PRO-licence only
        ///   - GUI excluded
        ///   - On some mobile devices or Unity-free, RenderTexture.active is null during OnPostRender, therefore it will switchback to 'jitRender'.
        ///   - Might produce artifacts when using postprocessing shaders or rendertexture-tricks.
        /// </summary>
        activeRenderTexture,
         */

        /// <summary>
        /// Re-renders the entire image again directly to the right format. Does not require Graphics.Blit and allows you to re-render everything at lower quality.
			
		///     Pro's
		/// 	  - Its very Fast at low render-resolutions
		/// 	  - It allows multiple active camera's to be rendered at the same time
		/// 	  - Re-renders the screen on a second camera. excluding postprocessing shaders. Might solve capture-artifacts
		/// 	  
		/// 	Con's
		/// 	  - PRO-licence only. It works without pro, but gives bottom-left-corner artifacts.
        /// 	  - It does not include Unity's OnGUI renders.
        /// </summary>
        jitRender,


        /// <summary>
		/// 	Reads all the pixels directly to a texture once the End-of-frame is reached.
			
		/// 	Pro's
		/// 	  - Includes OnGUI drawings!
		/// 	  - No PRO-licence required
			  
		/// 	Con's
		/// 	  - Slow!
        /// 	  - Does not allow multiple camera's
        /// </summary>
        endOfFrame
    }


    /// <summary>
    /// Sorted by its rough capture-performance and filesize, with the exception of PNG and JPG.
    /// 
    /// Raw formats:
    ///     - Best use: Mobile - short sessions length
    ///     - Best performance, though the output can be large.
    /// 
    /// LZF formats:
    ///     - Best use: Mobile - medium sessions length
    ///     - 2 to 6 times smaller then raw, though it does increase performance overhead a bit (lzf is a very very fast compression-method though)
    ///     - Compression takes place on a seperate Thread per frame, utilizing multi-core architectures.
    ///     
    /// PNG:
    ///     - Currently not supported (due to massive performance overhead)! Please send us an email if you want this to be a part of next update!
    ///     - Best use: PC or other powerfull devices - lengthy sessions.
    ///     - supports transparency (just like the RGBA32 & RGBA16 options) and a great filesize, though performance is poor.
    ///     - Compression takes place on the MainThread.
    /// 
    /// JPG:C:\DATA\Media\Unity Projects\vBug\Assets\vBug\vBug Debugger\vBugPublicEnums.cs
    ///     - Currently not supported (due massive to performance overhead)! Please send us an email if you want this to be a part of next update!
    ///     - Best use: PC or other powerfull devices - lengthy sessions.
    ///     - has the best filesize-quality ratio, but is most compute-intensive! Compression takes place on a seperate Thread.
    /// </summary>
    [Serializable]
    public enum ScreenCaptureQuality
    {
        rawGray8,
        rawRGBA16,
        rawRGB24,
        rawRGBA32,
        lzfGray8,
        lzfRGBA16,
        lzfRGB24,
        lzfRGBA32
        /*
    PNG,
    JPG100,
    JPG80,
    JPG60,
    JPG40
         */
    }

    //--------------- SCREEN CAPTURE --------------------


}