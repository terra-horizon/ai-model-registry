# Security

Key aspects of the Security checklist and practices that TERRA services must pass have been defined in the processes and documents governing the platform development and quality assurance. In this section we present a selected subset of these that are directly, publicly available and affect the usage and configuration of the service.

## Authentication

All endpoints exposed by this service require authentication using Bearer tokens in the form of JWTs (JSON Web Tokens). Clients must include a valid token in the Authorization header of each HTTP request, using the following format:

```
Authorization: Bearer <token>
```

The service only accepts JWTs that are issued by a trusted identity provider, the [TERRA AAI service](https://github.com/terra-horizon/terra-aai). This issuer is responsible for authenticating users and issuing tokens with claims that the service can validate. One of the critical claims in the token is the *aud* (audience) claim. The value of this claim must include the identifier of this service, ensuring that the token was intended to be used with it.

When a token is received, the service performs a series of validation steps before granting access to any endpoint. These steps typically include verifying the token’s signature using the public keys published by the trusted issuer, checking the issuer *iss* claim to confirm it matches the expected TERRA AAI issuer, validating the audience *aud* claim to ensure the token was meant for this service, and checking the token expiration *exp* to confirm the token is still valid. Only if all these checks pass will the request be authenticated and passed for further processing.

The location of the configuration governing the specific behavior is described in the relevant [Configuration](configuration.md) section.

## Authorization

When an authenticated call reaches the service, the caller may be authorized to perform an action or not. This will have to be authorized based on the grants that are present as roles in the access token presented as well as the context in which they want to perform the operation.

Within the service, all data access operations as well as individual actions pass authorization checks. The permissions that are checked along with the policies attached to each one is managed in a configuration file that the respective [Configuration](configuration.md) section describes.

Possible authorization policies include:

* **Context-less assignment**: such as an administrator that can perform action X on entity Y, regardless of the kind of affiliation they have with the entity
* **Owner**: Specific kind of affiliation between the calling user and the entity over which the action is to be performed, indicating ownership of the entity. An example is an accounting event generator client that is managed by a specific user
* **Client**: a policy similar to the Context-less permission assignment. Instead of checking specificaly the "role" claim type, this policy will check the caller client id to have a specific value to match the policy
* **Authenticated**: Authenticated user, regardless of any other characteristic vcan be granted or not granted the specific permission
* **Anonymous**: Anonymous users be be granted or not granted the specific permission

With respect to Context-less grant assignment, there are two access levels that cen be statically assigned and interpreted for authorization purposes:

* **Admin**: Users with admin grant can manage all aspects of the application and see accounting events from all aggregation sources
* **User**: Users with user grant can see aggregation events from the aggregation sources they are associated with inside the accounting service

These roles are explicitly managed and assigned within the [TERRA AAI service](https://github.com/terra-horizon/terra-aai) service.

## Secrets

Secrets are a special kind of configuration that requires special handling. This is described in the respective [Configuration](configuration.md) section.