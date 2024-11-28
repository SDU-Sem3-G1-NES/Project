using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
using Npgsql.Internal;
using TableControllerApi.Models;

namespace TableControllerApi.Services
{
    public class SubscriberUriService
    {
        public List<TableWebhook> Webhooks { get; set; } = new();
        public bool Add(string tableGuid, string uriString)
        {
            if (Uri.TryCreate(uriString, UriKind.Absolute, out Uri? uriResult) && 
            (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
            {
            Webhooks.Add(new TableWebhook(tableGuid, uriResult));
            return true;
            }
            return false;
        }
        public bool Remove(string tableGuid, string uriString)
        {
            if (Uri.TryCreate(uriString, UriKind.Absolute, out Uri? uriResult) && 
            (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
            {
            var webhookToRemove = Webhooks.FirstOrDefault(w => w.TableGuid == tableGuid && w.WebhookUri == uriResult);
            if (webhookToRemove != null)
            {
                Webhooks.Remove(webhookToRemove);
                return true;
            }
            }
            return false;
        }
        public TableWebhook GetByTableId(string tableGuid)
        {
            throw new NotImplementedException();
        }
    }
    public class TableWebhook
    {
        public TableWebhook(string tableGuid, Uri webhookUri)
        {
            TableGuid = tableGuid;
            WebhookUri = webhookUri;
        }
        public string TableGuid { get; set; }
        public Uri WebhookUri { get; set; }
    }
}