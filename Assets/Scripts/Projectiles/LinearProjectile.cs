using System;
using UnityEngine;

namespace TowerDefense.Projectiles
{
    [RequireComponent(typeof(Rigidbody))]
    public class LinearProjectile : BaseProjectile
    {
        public float acceleration;
        public float startSpeed;
        
        protected override void Awake()
        {
            base.Awake();
            Rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (!IsFired)
            {
                return;
            }

            if (Math.Abs(acceleration) >= float.Epsilon)
            {
                Rigidbody.velocity += transform.forward * acceleration * Time.deltaTime;
            }
        }

        public override void FireAtPoint(Vector3 startPoint, Vector3 targetPoint)
        {
            transform.position = startPoint;
            Fire(BallisticsMath.CalculateLinearFireVector(startPoint, targetPoint, startSpeed));
        }

        public override void FireInDirection(Vector3 startPoint, Vector3 fireVector)
        {
            transform.position = startPoint;

            // If we have no initial speed, we provide a small one to give the launch vector a baseline magnitude.
            if (Math.Abs(startSpeed) < float.Epsilon)
            {
                startSpeed = 0.001f;
            }

            Fire(fireVector.normalized * startSpeed);
        }

        public override void FireAtVelocity(Vector3 startPoint, Vector3 fireVelocity)
        {
            transform.position = startPoint;
            startSpeed = fireVelocity.magnitude;
            Fire(fireVelocity);
        }
    }
}
