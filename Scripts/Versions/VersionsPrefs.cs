// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace TinyStats.Versions {
    public static class VersionsPrefs {
        private const string _VERSIONS = "ApplicationVersionsList";
        private const string _MAJOR_ATTRIBUTE = "major";
        private const string _MINOR_ATTRIBUTE = "minor";
        private const string _PATCH_ATTRIBUTE = "patch";
        private const string _TEST_ATTRIBUTE = "test";
        
        public static bool IsHaveSaved() {
            return PlayerPrefs.HasKey(_VERSIONS);
        }
        
        public static bool TryLoadVersions(out List<ApplicationVersion> result) {
            string data = PlayerPrefs.GetString(_VERSIONS);
            
            if (String.IsNullOrEmpty(data)) {
            #if UNITY_EDITOR || TINY_STATS_DEBUG
                Debug.LogError("VersionsPrefs.LoadVersions() - Last saved data null or empty!");
            #endif
                result = null;
                return false;
            }
            
            result = new List<ApplicationVersion>();
            
            XElement root = XElement.Parse(data);
            
            IEnumerable<XElement> versionsElements = root.Elements();
            
            foreach (XElement versionElement in versionsElements) {
                if (TryConvertFromXElement(versionElement, out ApplicationVersion version)) {
                    result.Add(version);
                }
            }
            
            return true;
        }
        
        public static void SaveVersions(List<ApplicationVersion> versionsList) {
            XElement root = new XElement(_VERSIONS);
            
            for (int versionId = 0; versionId < versionsList.Count; versionId++) {
                root.Add(ConvertVersionToXElement(versionsList[versionId]));
            }
            
            PlayerPrefs.SetString(_VERSIONS, root.ToString());
        }
        
        [NotNull]
        private static XElement ConvertVersionToXElement(ApplicationVersion version) {
            XElement result = new XElement($"v{version.ToString()}");
            
            result.Add(new XAttribute(_MAJOR_ATTRIBUTE, version.major));
            result.Add(new XAttribute(_MINOR_ATTRIBUTE, version.minor));
            result.Add(new XAttribute(_PATCH_ATTRIBUTE, version.patch));
            result.Add(new XAttribute(_TEST_ATTRIBUTE, version.test));
            
            return result;
        }
        
        private static bool TryConvertFromXElement(XElement element, out ApplicationVersion version) {
            if (!TryGetIntValueFromAttribute(element, _MAJOR_ATTRIBUTE, out int majorVersion)) {
                return ReturnDefaultVersion(false, out version);
            }
            
            if (!TryGetIntValueFromAttribute(element, _MINOR_ATTRIBUTE, out int minorVersion)) {
                return ReturnDefaultVersion(false, out version);
            }
            
            if (!TryGetIntValueFromAttribute(element, _PATCH_ATTRIBUTE, out int patchVersion)) {
                return ReturnDefaultVersion(false, out version);
            }
            
            if (!TryGetIntValueFromAttribute(element, _TEST_ATTRIBUTE, out int testVersion)) {
                return ReturnDefaultVersion(false, out version);
            }
            
            version = new ApplicationVersion(majorVersion, minorVersion, patchVersion, testVersion);
            
            return true;
        }
        
        private static bool TryGetIntValueFromAttribute(XElement element, string attributeName, out int result) {
            XAttribute attribute = element.Attribute(attributeName);
            
            if (attribute == null) {
                result = default;
                return false;
            }
            
            string value = attribute.Value;
            
            if (string.IsNullOrEmpty(value)) {
                result = default;
                return false;
            }
            
            return int.TryParse(value, out result);
        }
        
        private static bool ReturnDefaultVersion(bool returnValue, out ApplicationVersion version) {
            version = default;
            return returnValue;
        }
    }
}