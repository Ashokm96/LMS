# LMS Security Review

Review scope: the ASP.NET Core API, configuration, local deployment files, and Angular client. This document records the current findings so future changes can be reviewed against explicit security expectations.

## Findings

### 1. Exposed JWT signing secret — Critical — remediated in working tree

The original `appsettings.json` contained a hardcoded JWT signing key. Anyone who obtained the public repository could forge bearer tokens. The key is removed from current files and replaced by the required `JWT__SECRETKEY` environment variable. The old key must be revoked and all existing sessions should be invalidated. Historical commits must also be rewritten before publishing this remediation.

### 2. Database credentials in application configuration — High — remediated in working tree

The original configuration included a concrete MongoDB connection string. Although it pointed to localhost, checked-in connection settings encourage unsafe promotion to shared environments. The connection string is now required through `DBCONFIGURATION__CONNECTIONSTRING`, with only a non-secret example in `.env.example`.

### 3. Overly permissive CORS policy — High — open

The API currently allows any origin through `SetIsOriginAllowed((hosts) => true)` while also allowing credentials. Production deployments should replace this with an explicit allow-list of trusted UI origins and should not allow credentials for arbitrary origins.

### 4. Unrestricted host binding and transport assumptions — Medium — open

The local Compose setup uses HTTP for an internal development network. Production must terminate TLS at a trusted reverse proxy or load balancer, set forwarded-header handling correctly, and use an explicit host allow-list rather than relying on `AllowedHosts: "*"`.

### 5. Dependency and input validation drift — Medium — open

The project targets .NET 6 and older package versions. CI now checks for vulnerable dependencies, but a production release should upgrade to a supported .NET version after compatibility testing. Controllers should also apply explicit validation attributes and return consistent problem-details responses for malformed input.

## Verification checklist

- [ ] Revoke the exposed JWT key and invalidate tokens.
- [ ] Rewrite Git history and force-push the cleaned LMS repository.
- [ ] Confirm the old key is absent from all reachable commits and branches.
- [ ] Configure an explicit production CORS allow-list.
- [ ] Upgrade the runtime and dependencies under a tested migration.
- [ ] Add authentication and authorization integration tests.
