// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using System.Collections.Generic;
using TinyStats.Versions;
using UnityEditor;
using UnityEngine;

namespace TinyStats.Editor.VersionUpscale {
    public static class VersionUpscaleProjectSettings {
        private static ApplicationVersion _currentVersion;
        private static int _tempMajor;
        private static int _tempMinor;
        private static int _tempPatch;
        private static int _tempTest;
        
        private static bool _isEnableBundleUpscale;
        private static bool _isEnableVersioning;
        private static bool _isEnableVersionUpscale;
        private static VersionUpdateType _updateType;
        
        private const float _VERSION_SEGMENT_WIDTH = 64f;
        private const float _APPLY_VERSION_BUTTON_HEIGHT = 38f;
        
        static VersionUpscaleProjectSettings() {
            _isEnableBundleUpscale = VersionUpscalePrefs.LoadIsEnableBundleUpscale();
            _isEnableVersioning = VersionUpscalePrefs.LoadIsEnableVersioning();
            _isEnableVersionUpscale = VersionUpscalePrefs.LoadIsEnableVersionUpscale();
            _updateType = VersionUpscalePrefs.LoadVersionUpdateType();
            
            _currentVersion = ApplicationVersionExtension.CreateVersionFromBundle(PlayerSettings.bundleVersion, PlayerSettings.Android.bundleVersionCode);
            
            _tempMajor = _currentVersion.major;
            _tempMinor = _currentVersion.minor;
            _tempPatch = _currentVersion.patch;
            _tempTest = _currentVersion.test;
        }
        
        [SettingsProvider]
        public static SettingsProvider VersionUpscaleMenu() {
            SettingsProvider provider = new SettingsProvider("Project/Editor/Version Upscale", SettingsScope.Project);
            
            provider.label = "Version Upscale";
            provider.guiHandler = OnDrawSettings;
            provider.keywords = new HashSet<string>(new[] { "Version", "Prebuild", "upscale", "update" });
            
            return provider;
        }
        
        private static void OnDrawSettings(string obj) {
            DrawToggle("Enable Versioning", ref _isEnableVersioning, VersionUpscalePrefs.SaveIsEnableVersioning);
            GUI.enabled = _isEnableVersioning;
            EditorGUILayout.LabelField("Versioning", EditorStyles.boldLabel);
            ApplicationVersion newVersion = DrawVersionSetter();
            DrawVersionApplySegment(newVersion);
            
            EditorGUILayout.Space();
            GUI.enabled = true;
            EditorGUILayout.LabelField("PreBuild upscale version", EditorStyles.boldLabel);
            DrawToggle("Enable Bundle Upscale", ref _isEnableBundleUpscale, VersionUpscalePrefs.SaveIsEnableBundleUpscale);
            GUI.enabled = _isEnableVersioning;
            DrawToggle("Enable Version Upscale", ref _isEnableVersionUpscale, VersionUpscalePrefs.SaveIsEnableVersionUpscale);
            
            GUI.enabled = _isEnableVersioning && _isEnableVersionUpscale;
            DrawVersionUpdateType("Version Upscale Type", ref _updateType, VersionUpscalePrefs.SaveVersionUpdateType);
        }
        
        private static void DrawToggle(string label, ref bool targetValue, Action<bool> onValueChanged) {
            bool currentValue = targetValue;
            currentValue = EditorGUILayout.Toggle(label, currentValue);
            
            if (currentValue == targetValue) {
                return;
            }
            
            onValueChanged(currentValue);
            targetValue = currentValue;
        }
        
        private static void DrawVersionUpdateType(string label, ref VersionUpdateType targetValue, Action<VersionUpdateType> onValueChanged) {
            VersionUpdateType currentValue = targetValue;
            currentValue = (VersionUpdateType)EditorGUILayout.EnumPopup(label, currentValue);
            
            if (currentValue.Equals(targetValue)) {
                return;
            }
            
            onValueChanged(currentValue);
            targetValue = currentValue;
        }
        
        private static ApplicationVersion DrawVersionSetter() {
            EditorGUILayout.BeginHorizontal();
            
            _tempMajor = VerticalIntField("Major", _tempMajor, _VERSION_SEGMENT_WIDTH);
            _tempMinor = VerticalIntField("Minor", _tempMinor, _VERSION_SEGMENT_WIDTH);
            _tempPatch = VerticalIntField("Patch", _tempPatch, _VERSION_SEGMENT_WIDTH);
            _tempTest = VerticalIntField("Test", _tempTest, _VERSION_SEGMENT_WIDTH);
            
            EditorGUILayout.EndHorizontal();
            return new ApplicationVersion(_tempMajor, _tempMinor, _tempPatch, _tempTest);
        }
        
        private static int VerticalIntField(string label, int value, float width) {
            EditorGUILayout.BeginVertical(GUILayout.Width(width));
            EditorGUILayout.LabelField(label, GUILayout.Width(width));
            value = EditorGUILayout.IntField(value, GUILayout.Width(width));
            EditorGUILayout.EndVertical();
            return value;
        }
        
        private static void DrawVersionApplySegment(ApplicationVersion newVersion) {
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField($"Current version:\t{_currentVersion.ToString()}");
            EditorGUILayout.LabelField($"New version:\t{newVersion.ToString()}");
            EditorGUILayout.EndVertical();
            
            GUI.enabled = !newVersion.Equals(_currentVersion);
            
            if (GUILayout.Button("Apply version", GUILayout.Height(_APPLY_VERSION_BUTTON_HEIGHT))) {
                ApplyVersion(newVersion);
            }
            
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
        }
        
        private static void ApplyVersion(ApplicationVersion version) {
            if (!_isEnableVersioning) {
                return;
            }
            
            _currentVersion = version;
            PlayerSettings.bundleVersion = version.ToString();
            AssetDatabase.SaveAssets();
        }
    }
}