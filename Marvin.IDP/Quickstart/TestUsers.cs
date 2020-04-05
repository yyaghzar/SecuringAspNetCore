// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace Marvin.IDP
{
    public class TestUsers
    {
        public static List<TestUser> Users = new List<TestUser>
        {
            new TestUser{
                SubjectId = "2C2F9C88-A447-4ECC-AD96-BEC5C40F4BCD", 
                Username = "Frank", 
                Password = "password", 
                Claims = 
                {
                   
                    new Claim(JwtClaimTypes.GivenName, "Frank"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                }
            },
            new TestUser{
                SubjectId = "CFC1D8B6-A6EE-42E0-AC3A-2BA32795CA45", 
                Username = "Claire", 
                Password = "password", 
                Claims = 
                {
                    new Claim(JwtClaimTypes.GivenName, "Claire"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),

                }
            }
        };
    }
}