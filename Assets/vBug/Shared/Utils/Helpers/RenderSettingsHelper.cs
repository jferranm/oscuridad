using System;
using UnityEngine;

namespace Frankfort.VBug.Internal
{
    public static class RenderSettingsHelper
    {
        
        private static AnisotropicFiltering anisotropicFiltering;
        private static int antiAliasing;
        private static float lodBias;
        private static int maximumLODLevel;
        private static int pixelLightCount;
        private static int shadowCascades;
        private static float shadowDistance;
        private static bool softVegetation;
        private static int vSyncCount;


        public static void StoreRenderQuality()
        {
            anisotropicFiltering = QualitySettings.anisotropicFiltering;
            antiAliasing = QualitySettings.antiAliasing;
            lodBias = QualitySettings.lodBias;
            maximumLODLevel = QualitySettings.maximumLODLevel;
            pixelLightCount = QualitySettings.pixelLightCount;
            shadowCascades = QualitySettings.shadowCascades;
            shadowDistance = QualitySettings.shadowDistance;
            softVegetation = QualitySettings.softVegetation;
        }
        public static void SetLowRenderQuality()
        {
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
            QualitySettings.antiAliasing = 0;
            QualitySettings.lodBias = 0f;
            QualitySettings.maximumLODLevel = 0;
            QualitySettings.pixelLightCount = 1;
            QualitySettings.shadowCascades = 0;
            QualitySettings.shadowDistance = 0;
            QualitySettings.softVegetation = false;
        }
        public static void RestoreRenderQuality()
        {
            QualitySettings.anisotropicFiltering = anisotropicFiltering;
            QualitySettings.antiAliasing = antiAliasing;
            QualitySettings.lodBias = lodBias;
            QualitySettings.maximumLODLevel = maximumLODLevel;
            QualitySettings.pixelLightCount = pixelLightCount;
            QualitySettings.shadowCascades = shadowCascades;
            QualitySettings.shadowDistance = shadowDistance;
            QualitySettings.softVegetation = softVegetation;
        }
    }
}
