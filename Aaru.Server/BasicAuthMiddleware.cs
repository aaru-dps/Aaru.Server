// -------------------------------------------------------------------------------------------------
// Copyright (c) Johan Boström. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Aaru.Server
{
    public class BasicAuthMiddleware
    {
        readonly RequestDelegate next;
        readonly string          realm;

        public BasicAuthMiddleware(RequestDelegate next, string realm)
        {
            this.next  = next;
            this.realm = realm;
        }

        public async Task Invoke(HttpContext context)
        {
            string authHeader = context.Request.Headers["Authorization"];

            if(authHeader != null &&
               authHeader.StartsWith("Basic "))
            {
                // Get the encoded username and password
                string encodedUsernamePassword =
                    authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1]?.Trim();

                // Decode from Base64 to string
                string decodedUsernamePassword =
                    Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));

                // Split username and password
                string username = decodedUsernamePassword.Split(':', 2)[0];
                string password = decodedUsernamePassword.Split(':', 2)[1];

                // Check if login is correct
                if(IsAuthorized(username, password))
                {
                    await next.Invoke(context);

                    return;
                }
            }

            // Return authentication type (causes browser to show login dialog)
            context.Response.Headers["WWW-Authenticate"] = "Basic";

            // Add realm if it is not null
            if(!string.IsNullOrWhiteSpace(realm))
            {
                context.Response.Headers["WWW-Authenticate"] += $" realm=\"{realm}\"";
            }

            // Return unauthorized
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }

        // Make your own implementation of this
        // Check that username and password are correct
        public bool IsAuthorized(string username, string password)
        {
            IConfigurationBuilder builder       = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            IConfigurationRoot    configuration = builder.Build();
            string                validUser     = configuration.GetValue<string>("MetricsAuthentication:Username");
            string                validPassword = configuration.GetValue<string>("MetricsAuthentication:Password");

            return !string.IsNullOrWhiteSpace(validUser)                                   &&
                   !string.IsNullOrWhiteSpace(validPassword)                               &&
                   username.Equals(validUser, StringComparison.InvariantCultureIgnoreCase) &&
                   password.Equals(validPassword);
        }
    }
}