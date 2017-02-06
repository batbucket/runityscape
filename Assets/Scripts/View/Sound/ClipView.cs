using System;
using Scripts.View.ObjectPool;
using UnityEngine;

namespace Scripts.View.Sound {
    public class ClipView : PooledBehaviour {
        [SerializeField]
        private AudioSource source;

        public AudioClip Clip {
            set {
                source.clip = value;
            }
        }

        public bool IsPlaying {
            get {
                return source.isPlaying;
            }
        }

        public bool IsLoop {
            set {
                source.loop = value;
            }
        }

        public void Play() {
            source.Play();
        }

        public override void Reset() {
            source.Stop();
            source.loop = false;
        }
    }
}