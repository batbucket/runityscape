using System;
using Scripts.View.ObjectPool;
using UnityEngine;

namespace Scripts.View.Sound {
    /// <summary>
    /// Holds a clip for playing
    /// </summary>
    /// <seealso cref="Scripts.View.ObjectPool.PooledBehaviour" />
    public class ClipView : PooledBehaviour {
        [SerializeField]
        private AudioSource source;

        /// <summary>
        /// Sets the volume.
        /// </summary>
        /// <value>
        /// The volume.
        /// </value>
        public float Volume {
            set {
                source.volume = value;
            }
        }

        /// <summary>
        /// Sets the clip.
        /// </summary>
        /// <value>
        /// The clip.
        /// </value>
        public AudioClip Clip {
            set {
                source.clip = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is playing.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this clip is playing; otherwise, <c>false</c>.
        /// </value>
        public bool IsPlaying {
            get {
                return source.isPlaying;
            }
        }

        /// <summary>
        /// Sets a value indicating whether this clip is looping.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this clip is looping; otherwise, <c>false</c>.
        /// </value>
        public bool IsLoop {
            set {
                source.loop = value;
            }
        }

        /// <summary>
        /// Plays this clip.
        /// </summary>
        public void Play() {
            source.Play();
        }

        /// <summary>
        /// Reset the state of this MonoBehavior.
        /// </summary>
        public override void Reset() {
            source.Stop();
            source.loop = false;
        }
    }
}