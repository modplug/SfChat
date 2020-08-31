using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;

namespace SyncChat.Core
{
    public class ViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<bool> _isSending;

        public ViewModel()
        {
            Messages = new ObservableCollection<object>();

             Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(3))
                 .Select(_ => DateTime.Now.ToLongTimeString())
                 .ObserveOn(RxApp.MainThreadScheduler)
                 .Subscribe(Messages.Add);

            SendMessageCommand = ReactiveCommand.CreateFromTask<string, Unit>(async _ =>
            {
                // Simulate network call
                await Task.Delay(TimeSpan.FromSeconds(2));
                Messages.Add(_);
                return Unit.Default;
            });

            SendMessageCommand.IsExecuting
                .ToProperty(this, vm => vm.IsSending, out _isSending);
        }

        public bool IsSending => _isSending.Value;

        public ReactiveCommand<string, Unit> SendMessageCommand { get; }

        public ObservableCollection<object> Messages { get; }
    }
}