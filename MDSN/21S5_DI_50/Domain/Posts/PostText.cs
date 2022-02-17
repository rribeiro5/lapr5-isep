using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.Posts
{
    public class PostText :IValueObject
    {
        public string text { get; private set;}

        public PostText(string text)
        {
            this.text = text; //implementar regras negocio
        }
    }
}