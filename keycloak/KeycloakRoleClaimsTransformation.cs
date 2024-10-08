using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Text.Json;

namespace keycloak;

public class KeycloakRoleClaimsTransformation : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var identity = principal.Identity as ClaimsIdentity;
        var realmAccessClaim = identity?.FindFirst("realm_access");
        if (realmAccessClaim != null)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true  // Ignore case when deserializing JSON
            };

            // Deserialize the realm_access JSON to extract the roles
            var realmAccess = JsonSerializer.Deserialize<RealmAccess>(realmAccessClaim.Value, options);

            if (realmAccess?.Roles != null)
            {
                foreach (var role in realmAccess.Roles)
                {
                    // Add each role as a Claim of type ClaimTypes.Role
                    identity.AddClaim(new Claim(ClaimTypes.Role, role));
                }
            }
        }

        return Task.FromResult(principal);
    }

    public class RealmAccess
    {
        public List<string> Roles { get; set; }
    }
}
