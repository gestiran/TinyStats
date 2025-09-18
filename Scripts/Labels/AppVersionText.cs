// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using TinyStats.Versions;
using TMPro;
using UnityEngine;

namespace TinyStats.Labels {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TextMeshProUGUI))]
    internal sealed class AppVersionText : MonoBehaviour {
        [SerializeField]
        private TextMeshProUGUI _thisText;
        
        private void Start() {
        #if UNITY_ANDROID
            SetVersion(TinyStatsService.platform, TinyStatsService.androidSDKVersion, TinyStatsService.version);
        #else
            SetVersion(TinyStatsService.platform, TinyStatsService.version);
        #endif
            Destroy(this);
        }
        
    #if UNITY_ANDROID
        private void SetVersion(RuntimePlatform platform, int apiVersion, ApplicationVersion version) {
            _thisText.text = $"Platform: {platform} (API Level {apiVersion}), Version: {version}";
        }
    #else
        private void SetVersion(RuntimePlatform platform, ApplicationVersion version) {
            _thisText.text = $"Platform: {platform}, Version: {version}";
        }
    #endif
        
    #if UNITY_EDITOR
        
        private void Reset() {
            _thisText = GetComponent<TextMeshProUGUI>();
            UnityEditor.EditorUtility.SetDirty(this);
        }
        
    #endif
    }
}