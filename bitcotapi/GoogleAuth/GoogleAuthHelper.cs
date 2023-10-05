namespace GoogleAuth
{public static class GoogleAuthHelper
{
    public static async Task<string> GetAccessTokenAsync(string code)
    {
        // Create a Google OAuth client.
        var client = new GoogleOAuthClient("YOUR_CLIENT_ID", "YOUR_CLIENT_SECRET");

        // Get the user's access token.
        var token = await client.GetAccessTokenAsync(code);

        // Return the access token.
        return token;
    }

    public static async Task<GoogleJsonWebSignature.Payload> VerifyIdTokenAsync(string token)
    {
        // Create a Google JSON Web Signature verifier.
        var verifier = new GoogleJsonWebSignatureVerifier();

        // Verify the user's ID token.
        var payload = await verifier.VerifySignedTokenAsync(token);

        // Return the ID token payload.
        return payload;
    }

    public static async Task<GoogleUserInfo> GetUserInfoAsync(string token)
    {
        // Create a Google OAuth client.
        var client = new GoogleOAuthClient("YOUR_CLIENT_ID", "YOUR_CLIENT_SECRET");

        // Get the user's information.
        var userInfo = await client.GetUserInfoAsync(token);

        // Return the user's information.
        return userInfo;
    }
}
}