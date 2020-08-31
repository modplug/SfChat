using System;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using SyncChat.Core;
using Syncfusion.XForms.Chat;
using Xamarin.Forms;

namespace SyncChat
{
    public partial class MainPage : ContentPage
    {
        private readonly ObservableCollectionExtended<object> _messages = new ObservableCollectionExtended<object>();

        public MainPage()
        {
            InitializeComponent();
            var bindingContext = new ViewModel();
            BindingContext = bindingContext;

            // Since we don't want any direct reference to Syncfusion in our core library
            // listen to updates from the view model and transform the items to be SfChat compatible.
            bindingContext.Messages
                .ToObservableChangeSet()
                .Transform(o => new TextMessage {Text = (string) o})
                .Sort(SortExpressionComparer<TextMessage>.Ascending(t => t.DateTime))
                .CastToObject()
                .Bind(_messages)
                .Subscribe();

            ChatView.MessageTemplate = new MessageTemplateSelector(ChatView);
            ChatView.Messages = Messages;

            // Setting this to true just to showcase that it doesn't work on Android.
            ChatView.ShowKeyboardAlways = true;
        }

        public ObservableCollectionExtended<object> Messages => _messages;

        private void ChatView_OnSendMessage(object sender, SendMessageEventArgs e)
        {
            // The documentation states that e.Handled = false should not be added to the collection
            // I only got this working by setting e.Handled = true, which makes me think that the documentation is wrong.
            e.Handled = true;
            var text = ChatView.Editor.Text;

            // Resetting the editor text doesn't work.
            ChatView.Editor.Text = string.Empty;

            if (BindingContext is ViewModel vm)
            {
                vm.SendMessageCommand.Execute(text)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(_ =>
                    {
                        // Resetting the editor text does work here
                        // I'm guessing it's because the UI gets a refresh when a new item is added to the collection.
                        ChatView.Editor.Text = string.Empty;
                    });
            }
        }
    }
}
