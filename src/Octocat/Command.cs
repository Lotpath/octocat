namespace Octocat
{
    public class Command
    {
        public string Verb { get; set; }
        public string Noun { get; set; }
        public string Organization { get; set; }
        public string Repository { get; set; }
        public string Target { get; set; }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(Verb)
                   && string.IsNullOrEmpty(Noun)
                   && string.IsNullOrEmpty(Organization)
                   && string.IsNullOrEmpty(Repository)
                   && string.IsNullOrEmpty(Target);
        }
    }
}