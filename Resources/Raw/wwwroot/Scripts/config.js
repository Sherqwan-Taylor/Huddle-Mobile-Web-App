// config.js - App template configuration
export const APPWRITE = {
    endpoint: "https://cloud.appwrite.io/v1",
    projectId: "YOUR_PROJECT_ID",
    databaseId: "YOUR_DATABASE_ID",
    collectionId: "YOUR_COLLECTION_ID"
};

export const DAILY = {
    apiKey: "YOUR_DAILY_API_KEY",
    domain: "YOUR_SUBDOMAIN.daily.co"
};

// ⚠️  PRODUCTION NOTE:
// The Daily API key should NOT live in client-side code in production.
// Move room creation to an Appwrite Function so the key stays server-side.
