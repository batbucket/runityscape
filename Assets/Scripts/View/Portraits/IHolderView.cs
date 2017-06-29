using Scripts.View.ObjectPool;
using System.Collections.Generic;

namespace Scripts.View.Portraits {
    public interface IHolderView<C>
        where C : struct {
        void AddContents(IEnumerable<C> contents);
    }
}