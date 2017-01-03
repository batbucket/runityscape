using System;

namespace Scripts.Model.Pages {

    /// <summary>
    /// Helper class for passing
    /// conditional Actions to a Page's constructor.
    /// </summary>
    public struct PageActions {
        public Action onEnter;
        public Action onExit;
        public Action onFirstEnter;
        public Action onFirstExit;
        public Action onTick;
    }
}