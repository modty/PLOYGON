using Loxodon.Framework.Contexts;
using Loxodon.Framework.Services;

namespace Domain.Contexts
{
    public class CharacterContext:Context
    {
        private string username;
        public string Username { get { return this.username; } }
        public CharacterContext(string username) : this(username, null)
        {
            this.username = username;
            Context.AddContext(username,this);
        }

        public CharacterContext(string username, IServiceContainer container) : base(container, GetApplicationContext())
        {
            this.username = username;
        }
    }
}