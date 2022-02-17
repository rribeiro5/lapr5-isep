using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.Posts
{
    public class CommentId : EntityId
    {
        public CommentId(object value) : base(value)
        {
        }

        protected override object createFromString(string text)
        {
            throw new System.NotImplementedException();
        }

        public override string AsString()
        {
            throw new System.NotImplementedException();
        }
    }
}