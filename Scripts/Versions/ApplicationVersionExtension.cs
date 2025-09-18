// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using UnityEngine;

namespace TinyStats.Versions {
    public static class ApplicationVersionExtension {
        public static ApplicationVersion ToNextMajor(this ApplicationVersion version) {
            return new ApplicationVersion(version.major + 1, version.minor, version.patch, version.test);
        }
        
        public static ApplicationVersion ToPreviousMajor(this ApplicationVersion version) {
            return new ApplicationVersion(version.major - 1, version.minor, version.patch, version.test);
        }
        
        public static ApplicationVersion ToNextMinor(this ApplicationVersion version) {
            return new ApplicationVersion(version.major, version.minor + 1, version.patch, version.test);
        }
        
        public static ApplicationVersion ToPreviousMinor(this ApplicationVersion version) {
            return new ApplicationVersion(version.major, version.minor - 1, version.patch, version.test);
        }
        
        public static ApplicationVersion ToNextPatch(this ApplicationVersion version) {
            return new ApplicationVersion(version.major, version.minor, version.patch + 1, version.test);
        }
        
        public static ApplicationVersion ToPreviousPatch(this ApplicationVersion version) {
            return new ApplicationVersion(version.major, version.minor, version.patch - 1, version.test);
        }
        
        public static ApplicationVersion CreateVersionFromBundle(string versionText, int bundleVersion) {
            string[] parts = versionText.Split('.');
            
            if (parts.Length != 3) {
            #if UNITY_EDITOR || APPLICATION_STATS_DEBUG
                Debug.LogError("StatsService.ParsVersionToData() - Incorrect format of the project version!");
            #endif
                return new ApplicationVersion();
            }
            
            if (!int.TryParse(parts[0], out int major)) {
            #if UNITY_EDITOR || APPLICATION_STATS_DEBUG
                Debug.LogError("StatsService.ParsVersionToData() - Can't parse major version to text!");
            #endif
                return new ApplicationVersion();
            }
            
            if (!int.TryParse(parts[1], out int minor)) {
            #if UNITY_EDITOR || APPLICATION_STATS_DEBUG
                Debug.LogError("StatsService.ParsVersionToData() - Can't parse minor version to text!");
            #endif
                return new ApplicationVersion();
            }
            
            if (!int.TryParse(parts[2], out int patch)) {
            #if UNITY_EDITOR || APPLICATION_STATS_DEBUG
                Debug.LogError("StatsService.ParsVersionToData() - Can't parse patch version to text!");
            #endif
                return new ApplicationVersion();
            }
            
            return new ApplicationVersion(major, minor, patch, bundleVersion);
        }
    }
}