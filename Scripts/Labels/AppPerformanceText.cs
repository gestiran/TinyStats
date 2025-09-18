// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Collections;
using TMPro;
using UnityEngine;

namespace TinyStats.Labels {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TextMeshProUGUI))]
    internal sealed class AppPerformanceText : MonoBehaviour {
        [SerializeField]
        private TextMeshProUGUI _thisText;

        private const float _REFRESH_DELAY = 0.5f;

        private void OnEnable() => StartCoroutine(UpdateStatsData());
        
        private void OnDisable() => StopAllCoroutines();
        
        private IEnumerator UpdateStatsData() {
            WaitForSecondsRealtime delay = new WaitForSecondsRealtime(_REFRESH_DELAY);

            while (Application.isPlaying) {
                _thisText.text = $"Graphic: {TinyStatsService.graphicsDeviceType}, FPS: {TinyStatsService.fps:00}, MEM: {TinyStatsService.heapUsage:00.0}/{TinyStatsService.heapSize:00.0} MB";
                yield return delay;
            }
        }
        
    #if UNITY_EDITOR
        
        private void Reset() {
            _thisText = GetComponent<TextMeshProUGUI>();
            UnityEditor.EditorUtility.SetDirty(this);
        }
        
    #endif
    }
}