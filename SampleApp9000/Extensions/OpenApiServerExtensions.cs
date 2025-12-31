using System.Collections.Generic;
using Microsoft.OpenApi;

namespace SampleApp9000.Extensions
{
    public static class OpenApiServerExtensions
    {
        public static OpenApiServer WithVariable(this OpenApiServer server, string key, OpenApiServerVariable value)
        {
            server.Variables ??= new Dictionary<string, OpenApiServerVariable>();
            server.Variables.Add(key, value);
            return server;
        }
    }
}
