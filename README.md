# Keycloak RBAC with .net web api

## Description
This project integrates Keycloak, an open-source identity and access management solution, to provide secure authentication and authorization for your applications.

Find more on keycloak in https://medium.com/@raiamar021/integrating-keycloak-with-net-a-simple-example-of-authentication-and-authorization-05cb6187eb35 

## Useful references
- https://youtu.be/fvxQ8bW0vO8?t=636
- https://youtu.be/Blrn5JyAl6E?t=622
- https://www.rfc-editor.org/rfc/rfc6749#:~:text=The%20OAuth%202.0%20Authorization%20Framework.%20Abstract.%20The%20OAuth%202.0%20authorization
- https://www.youtube.com/watch?v=q3FiuTZlroE&list=PL1Nml43UBm6dOj4UuH-7a9e3wO6eL2SCi

## Configuration
> "Keycloak": {
> 
>"MetadataAddress": "http://localhost:8080/realms/client-name/.well-known/openid-configuration",
> 
>"ValidIssuer": "http://localhost:8080/realms/client-name",
> 
>"Audience": "what's in aud scope",
> 
>"IntrospectionUrl": "http://localhost:8080/realms/client-name/protocol/openid-connect/token/introspect",
> 
>"ClientId": "client-name",
> 
>"ClientSecret": "secret-key"
> 
>}

