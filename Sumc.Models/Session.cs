namespace Sumc.Models
{
    public class Session
    {
        public int Id { get; set; }

        public string GlobalAuthToken { get; set; }

        public string SessionToken { get; set; }

        public string FormToken { get; set; }

        public bool IsActive { get; set; }
    }
}
