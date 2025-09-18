// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;

namespace TinyStats.Versions {
    public readonly struct ApplicationVersion : IComparable<ApplicationVersion>, IEquatable<ApplicationVersion> {
        public int major => _major;
        public int minor => _minor;
        public int patch => _patch;
        public int test => _test;
        
        private readonly int _major;
        private readonly int _minor;
        private readonly int _patch;
        private readonly int _test;
        
        public ApplicationVersion(int major, int minor, int patch, int test) {
            _major = major;
            _minor = minor;
            _patch = patch;
            _test = test;
        }
        
        public static bool operator >(ApplicationVersion first, ApplicationVersion second) {
            return first.CompareTo(second) < 0;
        }
        
        public static bool operator <(ApplicationVersion first, ApplicationVersion second) {
            return first.CompareTo(second) > 0;
        }
        
        public override string ToString() => $"{_major}.{_minor}.{_patch}.{_test}";
        
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
            
            if (other._test != _test) {
                return other._test - _test;
            }
            
            return 0;
        }
        
        public bool Equals(ApplicationVersion other) {
            return _major == other._major && _minor == other._minor && _patch == other._patch && _test == other._test;
        }
        
        public override bool Equals(object obj) {
            return obj is ApplicationVersion other && Equals(other);
        }
        
        public override int GetHashCode() {
            return HashCode.Combine(_major, _minor, _patch, _test);
        }
    }
}