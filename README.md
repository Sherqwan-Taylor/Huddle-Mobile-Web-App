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

