// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;

namespace TinyStats.Versions {
    public readonly struct ApplicationVersion : IComparable<ApplicationVersion>, IEquatable<ApplicationVersion> {
        public int major => _major;
        public int minor => _minor;
        public int patch => _patch;
    #if UNITY_ANDROID
        public int test => _test;
    #endif
        
        private readonly int _major;
        private readonly int _minor;
        private readonly int _patch;
        
    #if UNITY_ANDROID
        private readonly int _test;
    #endif
        
    #if UNITY_ANDROID
        public ApplicationVersion(int major, int minor, int patch, int test) {
            _major = major;
            _minor = minor;
            _patch = patch;
            _test = test;
        }
    #else
        public ApplicationVersion(int major, int minor, int patch) {
            _major = major;
            _minor = minor;
            _patch = patch;
        }
    #endif
        
        public static bool operator >(ApplicationVersion first, ApplicationVersion second) {
            return first.CompareTo(second) < 0;
        }
        
        public static bool operator <(ApplicationVersion first, ApplicationVersion second) {
            return first.CompareTo(second) > 0;
        }
        
        public override string ToString() {
        #if UNITY_ANDROID
            return $"{_major}.{_minor}.{_patch}.{_test}";
        #else
            return $"{_major}.{_minor}.{_patch}";
        #endif
        }
        
        public string ToStringRelease() => $"{_major}.{_minor}.{_patch}";
        
        public int CompareTo(ApplicationVersion other) {
            if (other._major != _major) {
                return other._major - _major;
            }
            
            if (other._minor != _minor) {
                return other._minor - _minor;
            }
            
            if (other._patch != _patch) {
                return other._patch - _patch;
            }
            
        #if UNITY_ANDROID
            if (other._test != _test) {
                return other._test - _test;
            }
        #endif
            
            return 0;
        }
        
        public bool Equals(ApplicationVersion other) {
            bool global = _major == other._major && _minor == other._minor && _patch == other._patch;
        #if UNITY_ANDROID
            return global && _test == other._test;
        #else
            return global;
        #endif
        }
        
        public override bool Equals(object obj) {
            return obj is ApplicationVersion other && Equals(other);
        }
        
        public override int GetHashCode() {
        #if UNITY_ANDROID
            return HashCode.Combine(_major, _minor, _patch, _test);
        #else
            return HashCode.Combine(_major, _minor, _patch);
        #endif
        }
    }
}