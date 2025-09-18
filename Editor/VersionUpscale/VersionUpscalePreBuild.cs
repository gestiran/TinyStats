// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using TinyStats.Versions;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace TinyStats.Editor.VersionUpscale {
    public sealed class VersionUpscalePreBuild : IPreprocessBuildWithReport, IPostprocessBuildWithReport {
        public int callbackOrder => 0;

        private readonly bool _isEnableBundleUpscale;
        private readonly bool _isEnableVersioning;
        private readonly bool _isEnableVersionUpscale;
        private readonly VersionUpdateType _updateType;

        public VersionUpscalePreBuild() {
            _isEnableBundleUpscale = VersionUpscalePrefs.LoadIsEnableBundleUpscale();
            _isEnableVersioning = VersionUpscalePrefs.LoadIsEnableVersioning();
            _isEnableVersionUpscale = VersionUpscalePrefs.LoadIsEnableVersionUpscale();
            _updateType = VersionUpscalePrefs.LoadVersionUpdateType();
        }
        
        public void OnPreprocessBuild(BuildReport report) {
            if (_isEnableBundleUpscale) {
                UpscaleBundle();
            }

            if (_isEnableVersioning && _isEnableVersionUpscale) {
                UpscaleVersion(_updateType);   
            }
        }
        
        public void OnPostprocessBuild(BuildReport report) {
            if (report.summary.result is not (BuildResult.Succeeded or BuildResult.Unknown)) {
                if (_isEnableBundleUpscale) {
                    DownscaleBundle();
                }

                if (_isEnableVersioning && _isEnableVersionUpscale) {
                    DownscaleVersion(_updateType);   
                }
            }
        }

        private void UpscaleBundle() {
            int currentBundleVersion = PlayerSettings.Android.bundleVersionCode;
            PlayerSettings.Android.bundleVersionCode = currentBundleVersion + 1;
        }

        private void DownscaleBundle() {
            int currentBundleVersion = PlayerSettings.Android.bundleVersionCode;
            PlayerSettings.Android.bundleVersionCode = currentBundleVersion - 1;
        }
        
        private void UpscaleVersion(VersionUpdateType updateType) {
            ApplicationVersion currentVersion = ApplicationVersionExtension.CreateVersionFromBundle(PlayerSettings.bundleVersion, PlayerSettings.Android.bundleVersionCode);
            ApplicationVersion nextVersion;

            switch (updateType) {
                case VersionUpdateType.Test: PlayerSettings.Android.bundleVersionCode += 1; return;
                case VersionUpdateType.Patch: nextVersion = currentVersion.ToNextPatch(); break;
                case VersionUpdateType.Minor: nextVersion = currentVersion.ToNextMinor(); break;
                case VersionUpdateType.Major: nextVersion = currentVersion.ToNextMajor(); break;
                default: nextVersion = currentVersion; break;
            }


            PlayerSettings.bundleVersion = nextVersion.ToStringRelease();
        }

        private void DownscaleVersion(VersionUpdateType updateType) {
            ApplicationVersion currentVersion = ApplicationVersionExtension.CreateVersionFromBundle(PlayerSettings.bundleVersion, PlayerSettings.Android.bundleVersionCode);
            ApplicationVersion previousVersion;
            
            switch (updateType) {
                case VersionUpdateType.Test: PlayerSettings.Android.bundleVersionCode -= 1; return;
                case VersionUpdateType.Patch: previousVersion = currentVersion.ToPreviousPatch(); break;
                case VersionUpdateType.Minor: previousVersion = currentVersion.ToPreviousMinor(); break;
                case VersionUpdateType.Major: previousVersion = currentVersion.ToPreviousMajor(); break;
                default: previousVersion = currentVersion; break;
            }
            
            PlayerSettings.bundleVersion = previousVersion.ToStringRelease();
        }
    }
}