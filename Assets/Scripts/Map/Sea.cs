using System;
using System.Collections.Generic;
using Shapes;
using UnityEngine;

namespace Map
{
    public class Sea : MonoBehaviour
    {
        [SerializeField] private int pointCount;
        [SerializeField] private float waveAmplitude = 1;
        [SerializeField] private float waveCountPerUnit = 1;
        [SerializeField] private float waveSpeed = 1;
        [SerializeField] private float startOffset = 0.5f;
        [SerializeField] private Vector3 lineStartingPoint;
        [SerializeField] private Vector3 lineEndingPoint;
        
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private List<Vector3> _originalPositions;

        private void Awake()
        {
            _originalPositions = new List<Vector3>();
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.positionCount = pointCount+1;
            _lineRenderer.SetPosition(0,lineStartingPoint);
            _originalPositions.Add(lineStartingPoint);
            for (int i = 1; i <= pointCount; i++)
            {
                var t = (float)i / pointCount;
                var v3 = Vector3.Lerp(lineStartingPoint, lineEndingPoint, t);
                _lineRenderer.SetPosition(i,v3);
                _originalPositions.Add(v3);
            }
        }

        private void Update()
        {
            for (int i = 0; i < _lineRenderer.positionCount; i++)
            {
                var t = (float)i / _lineRenderer.positionCount;
                var amp = Mathf.Lerp(waveAmplitude, 0, t);
                var pos = _lineRenderer.GetPosition(i);
                pos.y = _originalPositions[i].y + (Mathf.Sin((startOffset+(Time.time*waveSpeed)-i) * waveCountPerUnit) * amp);
                _lineRenderer.SetPosition(i,pos);
            }
        }
    }
}