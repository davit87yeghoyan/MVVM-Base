using System.Collections.Generic;

namespace MVVMBase.DataBinding
{
    public static class ViewModelBaseExtensions
    {
        public static IEnumerable<T> Ancestors<T>(this ViewModelBase origin) where T : ViewModelBase
        {
            if (origin==null)
            {
                yield break;
            }
            var parentViewModel = origin.ParentViewModel;
            while (parentViewModel!=null)
            {
                if (parentViewModel is T castedViewModel)
                {
                    yield return castedViewModel;
                }
                parentViewModel = parentViewModel.ParentViewModel;
            }

        }
    }
}
