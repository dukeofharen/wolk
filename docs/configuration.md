# Configuration

This page explains the configuration options available when installing Wolk. Since this is a .NET Core application, the [official documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.1) of Microsoft can be followed for some understanding. But, in short, configuration values will be read in the following order:

- Passed as program arguments.
- Passed as environment variables.
- Set in appsettings.{ENVIRONMENT}.json.
- Set in appsettings.json.

Setting keys in .NET Core are set like "Logging:LogLevel:System". This might be hard to set correctly as environment variables. For setting this as environment variable, you can also set it like this: "Logging__LogLevel__System" (double underscore).

**ConnectionStrings:WolkDatabase**

Wolk uses SQLite as database. You configure the connection string like this: `Data Source=/location/to/wolk.db` or `Data Source=C:\location\to\wolk.db`.

**IdentityConfiguration:JwtSecret**

The symmetric key that is used to create the JSON web tokens. It needs to be a completely random string.

**IdentityConfiguration:ExpirationInSeconds**

The number of seconds a JSON web token is valid.

**WolkConfiguration:UploadsPath**

The path where the uploads will be placed. E.g. `/srv/wolk/uploads/` or `C:\data\uploads`.

**WolkConfiguration:EnableUserRegistration**

`true` if you want to be able to register users through the API, `false otherwise (this is the default). ATTENTION! As of now, this endpoint does not have authentication.

**WolkConfiguration:DefaultLoginEmail / WolkConfiguration:DefaultPassword**

When set, a default admin user will be created with this email address and password.

**WolkConfiguration:NoteHistoryLength**

The maximum number of note history items that will be saved. If the number of items exceeds this number, the oldest items will be deleted.