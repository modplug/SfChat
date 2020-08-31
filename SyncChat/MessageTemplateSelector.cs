using System;
using Syncfusion.XForms.Chat;
using Xamarin.Forms;

namespace SyncChat
{
    public class MessageTemplateSelector : ChatMessageTemplateSelector
    {
        private readonly DataTemplate _simpleMessageTemplate = new DataTemplate(() => new SimpleMessageView());
        private readonly DataTemplate _advancedMessageTemplate = new DataTemplate(() => new AdvancedMessageView());
        private readonly Random _randomGenerator = new Random();
            public MessageTemplateSelector(SfChat chat) : base(chat)
        {

        }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            // Return a random data template just to make sure it works with different templates.
            var value = _randomGenerator.Next(0, 1);
            return value == 0
                ? _simpleMessageTemplate
                : _advancedMessageTemplate;
        }
    }
}