using Scripts.View.ObjectPool;
using Scripts.View.Portraits;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.View.Portraits {

    public static class PortraitUtil {

        /// <summary>
        /// Used to selectively update held items from a holder class
        /// so that items that remain in the object between ticks
        /// keep the same gameobject so that special effects can work.
        /// </summary>
        /// <typeparam name="C">Type containing information needed to update the view</typeparam>
        /// <typeparam name="V">Type representing a view to be updated</typeparam>
        /// <param name="previous">Dictionary containing the previous tick's items</param>
        /// <param name="contents">Contents containing this tick's items to be setup</param>
        /// <param name="prefab">Prefab object to be created of type V</param>
        /// <param name="thisGameObject">Gameobject of the holder</param>
        /// <param name="idSelector">Function that selects a unique identifier in Content</param>
        /// <param name="setupView">Function that sets up the view based on information from content</param>
        public static void GetDifferences<C, V>(
            ref IDictionary<int, V> previous,
            IList<C> contents,
            V prefab,
            GameObject thisGameObject,
            Func<C, int> idSelector,
            Action<V, C> setupView
            )
            where V : PooledBehaviour
            where C : struct {
            IDictionary<int, V> current = new Dictionary<int, V>();
            foreach (C content in contents) {
                V view = default(V);

                // New contents also has previous character -> pass on to this iteration of the dictionary
                int id = idSelector(content);
                if (previous.ContainsKey(idSelector(content))) {
                    view = previous[id];
                    previous.Remove(id); // Remove from previous dict
                    Util.Unparent(view.gameObject); // Remove association so we can update order

                    // New contents has entirely new character -> make a new portraitview
                } else {
                    view = ObjectPoolManager.Instance.Get(prefab);
                }
                setupView(view, content);
                current.Add(new KeyValuePair<int, V>(id, view));
                Util.Parent(view.gameObject, thisGameObject); // Parent outside so order is maintained (in case if character -> new char / character occurs between ticks)
            }

            // Previous dict should only have unrefreshed characters, return these.
            foreach (V view in previous.Values) {
                ObjectPoolManager.Instance.Return(view);
            }
            previous = current;
        }
    }
}