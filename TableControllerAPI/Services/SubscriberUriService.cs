using TableControllerApi.Models;

namespace TableControllerApi.Services
{
    public class SubscriberUriService
    {
        public List<Uri> Webhooks { get; set; } = new();
        public bool AddUri(string uriString)
        {
            if (Uri.TryCreate(uriString, UriKind.Absolute, out Uri? uriResult) && 
            (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
            {
            Webhooks.Add(uriResult);
            return true;
            }
            return false;
        }
        public bool RemoveUri(string uriString)
        {
            if (Uri.TryCreate(uriString, UriKind.Absolute, out Uri? uriResult) && 
            (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps) && Webhooks.Contains(uriResult))
            {
            Webhooks.Remove(uriResult);
            return true;
            }
            return false;
        }
    }
}