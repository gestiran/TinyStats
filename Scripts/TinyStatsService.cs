// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using System.Collections.Generic;
using TinyStats.Versions;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;

namespace TinyStats {
    public static class TinyStatsService {
        public static int fps => Mathf.Min((int)(1f / (Time.unscaledDeltaTime == 0f ? 1f : Time.unscaledDeltaTime)), Application.targetFrameRate);
        public static float heapUsage => Profiler.GetMonoUsedSizeLong() * 0.000001f;
        public static float heapSize => Profiler.GetMonoHeapSizeLong() * 0.000001f;
        public static NetworkReachability network;
        
        public static readonly ApplicationVersion version;
        public static readonly ApplicationVersion[] versions;
    #if UNITY_ANDROID
        public static readonly int androidSDKVersion;
    #endif
        public static readonly RuntimePlatform platform;
        public static readonly GraphicsDeviceType graphicsDeviceType;
        public static readonly int cpuCoresCount;
        public static readonly int gpuMemorySize;
        public static readonly int ramSize;
        
        static TinyStatsService() {
            version = ApplicationVersionExtension.CreateVersionFromBundle(Application.version, GetBundleVersion());
            versions = GetVersions(version).ToArray();
            
        #if UNITY_ANDROID
            androidSDKVersion = GetAndroidSDKVersion();
        #endif

        #if UNITY_EDITOR
        #if UNITY_ANDROID
            platform = RuntimePlatform.Android;
        #elif UNITY_STANDALONE_WIN
            _platform = RuntimePlatform.WindowsEditor;
        #endif
        #else
            _platform = Application.platform;
        #endif
            
            graphicsDeviceType = SystemInfo.graphicsDeviceType;
            cpuCoresCount = SystemInfo.processorCount;
            gpuMemorySize = SystemInfo.graphicsMemorySize;
            ramSize = SystemInfo.systemMemorySize;
        }
        
        private static List<ApplicationVersion> GetVersions(ApplicationVersion current) {
            List<ApplicationVersion> result;
            
            if (VersionsPrefs.IsHaveSaved()) {
                if (!VersionsPrefs.TryLoadVersions(out result)) {
                    result = CreateCurrent(current);
                } else if (!result.Contains(current)) {
                    result.Add(current);
                    result.Sort();
                    VersionsPrefs.SaveVersions(result);
                }
            } else {
                result = CreateCurrent(current);
                VersionsPrefs.SaveVersions(result);
            }

            return result;
        }

        private static List<ApplicationVersion> CreateCurrent(ApplicationVersion current) {
            List<ApplicationVersion> result = new List<ApplicationVersion>(1);
            result.Add(current);
            return result;
        }
        
    #if UNITY_ANDROID
        private static int GetBundleVersion() {
        #if UNITY_EDITOR
            return UnityEditor.PlayerSettings.Android.bundleVersionCode;
        #endif
            
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject packageManager = currentActivity.Call<AndroidJavaObject>("getPackageManager");
            AndroidJavaObject packageInfo = packageManager.Call<AndroidJavaObject>("getPackageInfo", Application.identifier, 0);

            return packageInfo.Get<int>("versionCode");
        }

        private static int GetAndroidSDKVersion() {
        #if UNITY_EDITOR
            return (int)UnityEditor.PlayerSettings.Android.minSdkVersion;
        #endif
            
            string androidVersionText = SystemInfo.operatingSystem;
            int sdkPos = androidVersionText.IndexOf("API-", StringComparison.Ordinal) + 4;
                
            if (androidVersionText.Length < sdkPos + 2) {
                return 0;
            }
                
            if (!int.TryParse(androidVersionText.Substring(sdkPos, 2), out int result)) {
                return 0;
            }

            return result;
        }
    #endif
    }
}