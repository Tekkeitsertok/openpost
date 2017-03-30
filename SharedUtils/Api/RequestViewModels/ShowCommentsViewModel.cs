namespace SharedUtils.Api.RequestViewModels
{
    public class ShowCommentsViewModel : AuthRequest
    {
        public string Page { get; set; }
        public string Language { get; set; }
    }
}
