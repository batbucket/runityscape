using Scripts.View.ObjectPool;
using System.Collections.Generic;

namespace Scripts.View.Portraits {

    /// <summary>
    /// Interface for representing the various holder classes in a view.
    /// </summary>
    /// <typeparam name="C">Content needed to populate this view.</typeparam>
    public interface IHolderView<C>
        where C : struct {

        /// <summary>
        /// Populates the holder's appearance based on the contents.
        /// </summary>
        /// <param name="contents">Group of contents to use to populate the holder.</param>
        void AddContents(IList<C> contents);
    }
}