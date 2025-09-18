// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using TinyUtilities.Editor.Utilities;
using UnityEditor;

namespace TinyStats.Editor.VersionUpscale {
    public static class VersionUpscalePrefs {
        private const string _IS_ENABLE_BUNDLE_UPSCALE_0 = "EditorIsEnableBundleUpscale_{0}";
        private const string _IS_ENABLE_VERSIONING_0 = "EditorIsEnableVersioning_{0}";
        private const string _IS_ENABLE_VERSION_UPSCALE_0 = "EditorIsEnableVersionUpscale_{0}";
        private const string _VERSION_UPDATE_TYPE_0 = "EditorVersionUpdateType_{0}";
        
        public static void SaveIsEnableBundleUpscale(bool value) => EditorPrefs.SetBool(string.Format(_IS_ENABLE_BUNDLE_UPSCALE_0, ProjectUtility.project), value);
        
        public static void SaveIsEnableVersioning(bool value) => EditorPrefs.SetBool(string.Format(_IS_ENABLE_VERSIONING_0, ProjectUtility.project), value);
        
        public static void SaveIsEnableVersionUpscale(bool value) => EditorPrefs.SetBool(string.Format(_IS_ENABLE_VERSION_UPSCALE_0, ProjectUtility.project), value);
        
        public static void SaveVersionUpdateType(VersionUpdateType value) => EditorPrefs.SetInt(string.Format(_VERSION_UPDATE_TYPE_0, ProjectUtility.project), (int)value);
        
        public static bool LoadIsEnableBundleUpscale() => EditorPrefs.GetBool(string.Format(_IS_ENABLE_BUNDLE_UPSCALE_0, ProjectUtility.project), false);
        
        public static bool LoadIsEnableVersioning() => EditorPrefs.GetBool(string.Format(_IS_ENABLE_VERSIONING_0, ProjectUtility.project), false);
        
        public static bool LoadIsEnableVersionUpscale() => EditorPrefs.GetBool(string.Format(_IS_ENABLE_VERSION_UPSCALE_0, ProjectUtility.project), false);
        
        public static VersionUpdateType LoadVersionUpdateType() => (VersionUpdateType)EditorPrefs.GetInt(string.Format(_VERSION_UPDATE_TYPE_0, ProjectUtility.project), 0);
    }
}