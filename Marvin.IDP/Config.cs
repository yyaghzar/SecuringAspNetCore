// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Marvin.IDP
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {   // return Subject ID Claim
                new IdentityResources.OpenId(),
                // Profile related Claims
                new IdentityResources.Profile(),
                // Address related claims
               new IdentityResources.Address(), 

               new IdentityResource(){ 
                Name = "roles",
                Description = "Your role(s)",
                UserClaims = new List<string>{"role"}
               }
            };

        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[] 
            { };
        
        public static IEnumerable<Client> Clients =>
            new Client[] 
            { 
            
            new Client{ 
                ClientName = "Image Gallery",
                ClientId = "ImageGalleryClient",
                AllowedGrantTypes = GrantTypes.Code,
                //AlwaysIncludeUserClaimsInIdToken = true,
                RequirePkce = true,
                RedirectUris = { "https://localhost:44389/signin-oidc"},
                PostLogoutRedirectUris = { "https://localhost:44389/signout-callback-oidc"},
                
                AllowedScopes = 
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Address,
                    "roles"
                },
                ClientSecrets = { new Secret("secret".Sha256())}


            }
            
            };
        
    }
}