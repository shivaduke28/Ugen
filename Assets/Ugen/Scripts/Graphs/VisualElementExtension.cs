using R3;
using UnityEngine.UIElements;

namespace Ugen.Graphs
{
    public static class VisualElementExtension
    {
        public static Observable<Unit> OnClickAsObservable(this Button button)
        {
            return Observable.FromEvent(h => button.clicked += h, h => button.clicked -= h);
        }

        public static Observable<T> AsObservable<T>(this CallbackEventHandler handler) where T : EventBase<T>, new()
        {
            return Observable.FromEvent<EventCallback<T>, T>(
                h => e => h(e),
                h => handler.RegisterCallback(h),
                h => handler.UnregisterCallback(h)
            );
        }

        public static Observable<ChangeEvent<T>> OnValueChangeAsObservable<T>(this INotifyValueChanged<T> source)
        {
            return Observable.FromEvent<EventCallback<ChangeEvent<T>>, ChangeEvent<T>>(
                h => new EventCallback<ChangeEvent<T>>(h),
                h => source.RegisterValueChangedCallback(h),
                h => source.UnregisterValueChangedCallback(h));
        }
    }
}
