using System;
using MVVMBase.DataBinding;

namespace MVVMBase.ViewBase
{
    public interface IView<T> where T : ViewModelBase
    {
        T BindingContext { get; set; }
        void Reveal(bool immediate=false,Action action=null);
        void Hide(bool immediate=false,Action action=null);
    }
}
