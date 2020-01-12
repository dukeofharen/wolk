# API

You can create, read, update and delete records in Wolk by using the REST API. The user interfaces also uses this API. This page explains the possibilities of the API and how to get started.

## General

This page does not explain every endpoint in detail. To get started with the API, do one of the following.

- There is a [Postman](https://www.getpostman.com/) collection that contains all endpoints for testing purposes, that can be found [here](https://github.com/dukeofharen/wolk/blob/master/scripts/testing/wolk_requests.json). Make sure you executed "Users => Authenticate" first so the correct JWT is loaded in memory.
- There is a [swagger.json](https://github.com/dukeofharen/wolk/releases/latest) that can be downloaded from GitHub Releases. This Swagger file can be imported in several tools.
- When running Wolk, go to `http://[WOLK]/swagger` to view the internal Swagger page where you can do requests.

## Authentication

Requests are authenticated using JSON Web Tokens (JWT). To obtain a JWT, a `POST` request to the `/api/user/authenticate` endpoint should be made using the following JSON body:

```
{
	"email": "wolk@example.com",
	"password": "YouRpAssword1!"
}
```

After a successful attempt, you get the following response body:

```
{
    "id": 3,
    "email": "wolk@example.com",
    "token": "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIzIiwiU2VjdXJpdHlTdGFtcCI6IjhhMThmY2ZiLTJjZWMtNGZjMS05ZmJlLWRkNjdhMjJiZmE1NCIsIm5iZiI6MTU3ODgzNjYxNCwiZXhwIjoxNTc5MzYyMjE0LCJpYXQiOjE1Nzg4MzY2MTR9.CMjAzm1heibjrRCBKcY3MPNkhdTWGqUjvl_E5MURSLCBObY696-v3qPnT9g66fHu0LczqcYEWfPp9dD_UYZSmg"
}
```

The `token` field contains the actual JWT. This needs to be sent with every subsequent request in the `Authorization` header in this form `Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVC...`

## Notebooks

Notebooks are containers for notes. The notebook resource contains endpoints for reading, creating, updating and deleting notebooks.

## Notes

The note is the main entity of Wolk. The note resource contains endpoints for reading, creating, updating and deleting notes. It represents a simple note, but by specifying another `noteType` when creating the note, the note will be rendered differently on the frontend. The following note types are (currently) supported by Wolk:

- `PlainText` (1): the note will be rendered as plain text. In the UI, all new lines will be replaced with `<br />`.
- `Markdown` (2): the note will be interpreted as Markdown and the parsed HTML will be rendered in the UI.
- `TodoTxt` (3): the note will be interpreted as [todo.txt](http://todotxt.org/), a simple, text-based format for specifying your todo items. More about this is explained in the [user manual](user-manual.md).
- `StickyNotes` (4): the note will be interpreted as sticky notes. Multiple sticky notes are placed, as plain text, in one note and the result will be rendered on the user interface. More about this is explained in the [user manual](user-manual.md).

## Attachments

One note can have multiple attachments. The attachment metadata will be saved in the database and the actual attachment will be saved on disk. The attachment resource contains endpoints for reading, creating and deleting attachments.

## Access tokens

An access token is a token which can be used to grant (temporary) access to an entity. As of now, only access tokens for attachments can be created. The access token can have an expiration date, so it is invalid after a certain time.

## Users

The users endpoint is used for administering users and authentcation. There is an endpoint for creating a new user, but this is turned off by default (see [configuration](configuration.md)), for now.

## Backups

The backups endpoints can be used to download a complete zipped backup of Wolk. It also contains and endpoint for uploading a backup. ATTENTION! All contents (including users) will be overwritten, so be careful when restoring a backup!