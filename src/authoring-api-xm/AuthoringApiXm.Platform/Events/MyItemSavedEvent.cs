using Sitecore.Abstractions;
using Sitecore.Data.Items;
using Sitecore.Events;
using System;

namespace AuthoringApiXm.Platform.Events
{
    public class MyItemSavedEvent
    {
        private readonly BaseLog _log;

        public MyItemSavedEvent(BaseLog log)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public void OnItemSaved(object sender, EventArgs args)
        {
            var item = Event.ExtractParameter(args, 0) as Item;
            var templateId = item.TemplateID.ToGuid();

            if (templateId != Guid.Empty)
            {
                _log.Info("LonghornTaco is COOL!", this);
            }
        }
    }
}


