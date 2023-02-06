using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IzvidaciWebApp.Providers.Http.Options;
public class AkcijaProviderOptions
{
    public string BaseUrl { get; init; }
}
/*
 * in appsettings the json section should be as:
{ 
    ...
    "AkcijaProviderOptions":
    {
        "BaseUrl":"http://localhost:1234/Akcija"
    }
    ...
}
 */