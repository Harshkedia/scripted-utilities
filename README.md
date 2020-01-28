# Rhythm of the Day - Backend

[![Generic badge](https://img.shields.io/badge/Deployment-Heroku-Green.svg)](https://rhythm-day-backend.herokuapp.com/)

GraphQL backend server for the Rhythm of the Day tool in the Applied Science Toolkit.

## Development

After cloning the repository locally and installing the dependencies with `npm i`, you must provision the environment variables below for the server to work. Contact Petr Mitev for the API key values needed to start the server.

```json
{
  "FIREBASE_API_KEY": "VALUE_HERE",
  "FIREBASE_AUTH_DOMAIN": "VALUE_HERE",
  "FIREBASE_DATABASE_URL": "VALUE_HERE",
  "FIREBASE_PROJECTID": "VALUE_HERE",
  "FIREBASE_STORAGE_BUCKET": "VALUE_HERE"
}
```

After provisioning the environment variables, the server can be started by running `npm run start` in the root directory of this repository. The development instance will then start on `http://localhost:4000`.

## Usage

Documentation is provided naturally by GraphQL. Visit `http://localhost:4000` once the server is running with a web browser to browse the schema and API documentation. You can also browse the documentation at the [deployed link](https://rhythm-day-backend.herokuapp.com/).

![documentation image](assets/docs.png)
