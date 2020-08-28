using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;
using Syncfusion.XForms.Chat;

namespace SyncChat.Core
{
    public class ViewModel : ReactiveObject
    {
        private readonly SourceList<object> _chatMessages = new SourceList<object>();
        private readonly ObservableAsPropertyHelper<bool> _isSending;

        public ViewModel()
        {
            Messages = new ObservableCollection<object>();

            Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1))
                .Select(_ => new TextMessage { Text = DateTime.Now.ToLongTimeString() })
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(Messages.Add);

            SendMessageCommand = ReactiveCommand.CreateFromTask<object, Unit>(async _ =>
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                Messages.Add(new TextMessage() {Text = DateTime.Now.ToLongTimeString()});
                return Unit.Default;
            });

            SendMessageCommand.IsExecuting
                .ToProperty(this, vm => vm.IsSending, out _isSending);
        }

        public bool IsSending => _isSending.Value;

        public ReactiveCommand<object, Unit> SendMessageCommand { get; }

        public ObservableCollection<object> Messages { get; }
    }
}