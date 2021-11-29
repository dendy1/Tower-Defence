using UnityEditor;
using UnityEngine;

namespace TowerDefense.Towers
{
    [CustomEditor(typeof(Targetter)), CanEditMultipleObjects]
    public class TargetterEditor : Editor
    {
        /// <summary>
        /// Configuration for which collider to use
        /// </summary>
        public enum TowerCollider
        {
            /// <summary>
            /// For sphere collider
            /// </summary>
            Sphere,

            /// <summary>
            /// For capsule collider
            /// </summary>
            Capsule
        }

        /// <summary>
        /// The targetter to edit
        /// </summary>
        private Targetter _targetter;

        /// <summary>
        /// The collision configuration to use
        /// </summary>
        private TowerCollider _colliderConfiguration;

        /// <summary>
        /// The radius of the collider
        /// </summary>
        private float _colliderRadius;

        // Capsule specific info

        /// <summary>
        /// The height of a capsule collider
        /// </summary>
        private float _extraVerticalRange;

        /// <summary>
        /// The attached collider
        /// </summary>
        private Collider _attachedCollider;

        /// <summary>
        /// The serialized property representing <see cref="_attachedCollider"/>
        /// </summary>
        private SerializedProperty _serializedAttachedCollider;

        /// <summary>
        /// draws the default inspector 
        /// and then draws configuration for colliders
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            // To make the inspector a little bit neater
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Targetter Collider Configuration", EditorStyles.boldLabel);

            _colliderConfiguration =
                (TowerCollider)EditorGUILayout.EnumPopup("Targetter Collider", _colliderConfiguration);
            AttachCollider();
            _colliderRadius = EditorGUILayout.FloatField("Radius", _colliderRadius);
            if (_colliderConfiguration == TowerCollider.Capsule)
            {
                _extraVerticalRange = EditorGUILayout.FloatField("Vertical Range", _extraVerticalRange);
            }

            SetValues();
            EditorUtility.SetDirty(_targetter);
            EditorUtility.SetDirty(_attachedCollider);
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// For attaching and hiding the correct collider
        /// </summary>
        void AttachCollider()
        {
            switch (_colliderConfiguration)
            {
                case TowerCollider.Sphere:
                    if (_attachedCollider is SphereCollider)
                    {
                        GetValues();
                        return;
                    }

                    if (_attachedCollider != null)
                    {
                        DestroyImmediate(_attachedCollider, true);
                    }

                    _attachedCollider = _targetter.gameObject.AddComponent<SphereCollider>();
                    _serializedAttachedCollider.objectReferenceValue = _attachedCollider;
                    break;
                case TowerCollider.Capsule:
                    if (_attachedCollider is CapsuleCollider)
                    {
                        GetValues();
                        return;
                    }

                    if (_attachedCollider != null)
                    {
                        DestroyImmediate(_attachedCollider, true);
                    }

                    _attachedCollider = _targetter.gameObject.AddComponent<CapsuleCollider>();
                    _serializedAttachedCollider.objectReferenceValue = _attachedCollider;
                    break;
            }

            SetValues();
            _attachedCollider.hideFlags = HideFlags.HideInInspector;
        }

        /// <summary>
        /// Assigns the values to the collider
        /// </summary>
        void SetValues()
        {
            switch (_colliderConfiguration)
            {
                case TowerCollider.Sphere:
                    var sphere = (SphereCollider)_attachedCollider;
                    sphere.radius = _colliderRadius;
                    break;
                case TowerCollider.Capsule:
                    var capsule = (CapsuleCollider)_attachedCollider;
                    capsule.radius = _colliderRadius;
                    capsule.height = _extraVerticalRange + _colliderRadius * 2;
                    break;
            }
        }

        /// <summary>
        /// Obtains the information from the collider
        /// </summary>
        void GetValues()
        {
            switch (_colliderConfiguration)
            {
                case TowerCollider.Sphere:
                    var sphere = (SphereCollider)_attachedCollider;
                    _colliderRadius = sphere.radius;
                    break;
                case TowerCollider.Capsule:
                    var capsule = (CapsuleCollider)_attachedCollider;
                    _colliderRadius = capsule.radius;
                    _extraVerticalRange = capsule.height - _colliderRadius * 2;
                    break;
            }
        }

        /// <summary>
        /// Caches the collider and hides it
        /// and configures all the necessary information from it
        /// </summary>
        void OnEnable()
        {
            _targetter = (Targetter)target;
            _serializedAttachedCollider = serializedObject.FindProperty("attachedCollider");
            _attachedCollider = (Collider)_serializedAttachedCollider.objectReferenceValue;

            if (_attachedCollider == null)
            {
                _attachedCollider = _targetter.GetComponent<Collider>();
                if (_attachedCollider == null)
                {
                    switch (_colliderConfiguration)
                    {
                        case TowerCollider.Sphere:
                            _attachedCollider = _targetter.gameObject.AddComponent<SphereCollider>();
                            break;
                        case TowerCollider.Capsule:
                            _attachedCollider = _targetter.gameObject.AddComponent<CapsuleCollider>();
                            break;
                    }

                    _serializedAttachedCollider.objectReferenceValue = _attachedCollider;
                }
            }

            if (_attachedCollider is SphereCollider)
            {
                _colliderConfiguration = TowerCollider.Sphere;
            }
            else if (_attachedCollider is CapsuleCollider)
            {
                _colliderConfiguration = TowerCollider.Capsule;
            }

            // to ensure the collider is referenced by the serialized object
            if (_serializedAttachedCollider.objectReferenceValue == null)
            {
                _serializedAttachedCollider.objectReferenceValue = _attachedCollider;
            }

            GetValues();
            _attachedCollider.isTrigger = true;
            _attachedCollider.hideFlags = HideFlags.HideInInspector;
        }
    }
}
