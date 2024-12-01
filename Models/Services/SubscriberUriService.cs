using DataAccess;

namespace Models.Services
{
    public class SubscriberUriService
    {
        private readonly SubscriberRepository subscriberRepository = new SubscriberRepository();
        public bool Add(string tableGuid, string uriString)
        {
            if (Uri.TryCreate(uriString, UriKind.Absolute, out Uri? uriResult) && 
            (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)
            && subscriberRepository.GetSubscriber(tableGuid) == null)
            {
                subscriberRepository.InsertSubscriber(tableGuid, uriString);
                return true;
            }
            return false;
        }
        public bool Remove(string tableGuid)
        {
            if (subscriberRepository.GetSubscriber(tableGuid) != null)
            {
                subscriberRepository.DeleteSubscriber(tableGuid);
                return true;
            }
            return false;
        }
        public string GetByTableId(string tableGuid)
        {
            return subscriberRepository.GetSubscriber(tableGuid) ?? "";
        }
    }
}