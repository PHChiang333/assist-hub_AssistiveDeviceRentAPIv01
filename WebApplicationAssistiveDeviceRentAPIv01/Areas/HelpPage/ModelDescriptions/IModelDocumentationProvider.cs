using System;
using System.Reflection;

namespace WebApplicationAssistiveDeviceRentAPIv01.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}