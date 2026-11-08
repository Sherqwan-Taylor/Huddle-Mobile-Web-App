This project requires the use of Appwrite for it's backend server and Daily.co for it's video calling feature.



This build of the project will require the following:



APPWRITE:

* endpoint
* projectId
* databaseId
* collectionId

All of which can be acquired from your Appwrite project



DAILY:

* apiKey
* domain

Both of which can be acquired from your Daily.co dashboard



PRODUCTION NOTE:

The Daily API key should NOT live in client-side code in production.

For future productions, move room creation to an Appwrite Function so the key stays server-side.



These keys will go inside the config file that is located in:

Resources\\Raw\\wwwroot\\Scripts

This is a small hobby project that I have been working on using AI assistance while currently in college to showcase on my portfolio, this is a simple mobile web video calling app that uses Appwrite as a backend and Daily.co for it's video calling. It's my first time making a mobile app, so it's a bit rough, but I think I did OK with it, though I will try to add more to it as I go along and learn more.

You can download and try it live here:
https://tdr-app-download.appwrite.network
