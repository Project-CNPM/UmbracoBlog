using CleanBlog.Core.Services;
using Umbraco.Core.Composing;
using Umbraco.Core;

namespace CleanBlog.Core.Composing
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class RegisterServicesComposer : IUserComposer
    {
        
        public void Compose(Composition composition)
        {
            composition.Register<ISmtpService, SmtpService>(Lifetime.Singleton);
            composition.Register<IEmailService, EmailService>(Lifetime.Request);
            composition.Register<IArticleService, ArticleService>(Lifetime.Request);
            composition.Register<ISearchService, SearchService>(Lifetime.Request);
            composition.Register(typeof(IDataTypeValueService), typeof(DataTypeValueService),Lifetime.Request);

        }
    }
}
