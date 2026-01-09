using UnityEngine;

namespace Mirror {
    // [RequireComponent(typeof(Rigidbody))] <- OnValidate ensures this is on .target
    [AddComponentMenu("Network/Network Rigidbody (Unreliable)")]
    public class NetworkRigidbodyUnreliable : NetworkTransformUnreliable {
        Rigidbody rb;

        protected override void OnValidate() {
            // Skip if Editor is in Play mode
            if (Application.isPlaying)
                return;

            base.OnValidate();

            // we can't overwrite .target to be a Rigidbody.
            // but we can ensure that .target has a Rigidbody, and use it.
            if (target.GetComponent<Rigidbody>() == null) {
                Debug.LogWarning($"{name}'s NetworkRigidbody.target {target.name} is missing a Rigidbody", this);
            }
        }

        // cach Rigidbody and original isKinematic setting
        protected override void Awake() {
            // we can't overwrite .target to be a Rigidbody.
            // but we can use its Rigidbody component.
            rb = target.GetComponent<Rigidbody>();
            if (rb == null) {
                Debug.LogError($"{name}'s NetworkRigidbody.target {target.name} is missing a Rigidbody", this);
                return;
            }
            base.Awake();
        }

        protected override void OnTeleport(Vector3 destination) {
            base.OnTeleport(destination);

            rb.position = transform.position;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        protected override void OnTeleport(Vector3 destination, Quaternion rotation) {
            base.OnTeleport(destination, rotation);

            rb.position = transform.position;
            rb.rotation = transform.rotation;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        private void FixedUpdate() {
            if (isOwned) {
#if UNITY_2023_1_OR_NEWER
                networkedVelocity = rb.linearVelocity;
#else
                networkedVelocity = rb.velocity;
#endif
                networkedAngularVelocity = rb.angularVelocity;
            }
            else {
#if UNITY_2023_1_OR_NEWER
                rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, networkedVelocity, (float)NetworkTime.offset);
#else
                rb.velocity = Vector3.Lerp(rb.velocity, networkedVelocity, (float)NetworkTime.offset);
#endif
                rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, networkedAngularVelocity, (float)NetworkTime.offset);
            }
        }

        [Networked] private Vector3 networkedVelocity = Vector3.zero;
        [Networked] private Vector3 networkedAngularVelocity = Vector3.zero;
    }
}
