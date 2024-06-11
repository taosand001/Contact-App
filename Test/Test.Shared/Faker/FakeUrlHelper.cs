using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Diagnostics.CodeAnalysis;

namespace Test.Shared.Faker
{
    public class FakeUrlHelper : IUrlHelper
    {

        private readonly string _url;

        public FakeUrlHelper(string url)
        {
            _url = url;
        }
        public ActionContext ActionContext => throw new NotImplementedException();

        public string Action(UrlActionContext actionContext)
        {
            return _url;
        }

        [return: NotNullIfNotNull("contentPath")]
        public string? Content(string? contentPath)
        {
            throw new NotImplementedException();
        }

        public bool IsLocalUrl([NotNullWhen(true), StringSyntax("Uri")] string? url)
        {
            throw new NotImplementedException();
        }

        public string? Link(string? routeName, object? values)
        {
            throw new NotImplementedException();
        }

        public string? RouteUrl(UrlRouteContext routeContext)
        {
            throw new NotImplementedException();
        }
    }
}
