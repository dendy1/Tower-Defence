using UnityEngine;

namespace TowerDefense.Towers
{
    [RequireComponent(typeof(LineRenderer))]
    public class TowerRadiusVisualizer : MonoBehaviour
    {
        [Header("Radius Parameters")]
        [Range(3, 256)]
        [SerializeField] private int numSegments = 128;
        [SerializeField] private Color color;
        [SerializeField] private float width;
        
        private LineRenderer _lineRenderer;
        
        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.startColor = color;
            _lineRenderer.endColor = color;
            _lineRenderer.startWidth = width;
            _lineRenderer.endWidth = width;
            _lineRenderer.positionCount = numSegments + 1;
            _lineRenderer.useWorldSpace = false;
        }

        public void DoRenderer(float radius)
        {
            if (!_lineRenderer)
            {
                return;
            }

            _lineRenderer.startColor = color;
            _lineRenderer.endColor = color;
            _lineRenderer.startWidth = width;
            _lineRenderer.endWidth = width;
            _lineRenderer.positionCount = numSegments + 1;
            _lineRenderer.useWorldSpace = false;
            
            var deltaTheta = (float)(2.0 * Mathf.PI) / numSegments;
            var theta = 0f;
            
            for (var i = 0; i < numSegments + 1; i++)
            {
                var x = radius * Mathf.Cos(theta);
                var z = radius * Mathf.Sin(theta);
                var pos = new Vector3(x, 0, z);
                _lineRenderer.SetPosition(i, pos);
                theta += deltaTheta;
            }
        }
    }
}