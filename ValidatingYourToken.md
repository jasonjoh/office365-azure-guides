# Validating your Office 365 Access Token #

So you've [registered your app](https://github.com/jasonjoh/office365-azure-guides/blob/master/RegisterAnAppInAzure.md), you're [doing the OAuth flow](http://msdn.microsoft.com/en-us/library/azure/dn645542.aspx), and you have a token. But when you try to use it, you get an invalid token or an access denied error. Let's take a look at how you can validate the token and figure out what's wrong.

## Parse the token ##

The quickest check you can do is to actually decode and validate the token. You'll need to copy the token value that you pass in the `Authorization` header. You can do this from a debugger, add code to your app to write it to a log, or use Fiddler to capture it. However you get it, it's a big long string of seemingly random characters. It's far from random though. It's an encoded JSON Web Token.

To read it, you have a few options. There's a great web-based decoder at http://jwt.calebb.net/. Just paste your token in, and it displays it for you. Or, if you're not comfortable pasting access tokens into a third-party site, you can write some code to parse it yourself! For some quick-and-dirty sample C# code, see parse-token.cs.

However you parse it, it should look something like this. Note that if it errors when you try to parse it, that's a good indicator that the token is invalid. If your token is missing any of the fields you see below, it's likely invalid.

	{
	  "aud": "https://outlook.office365.com/",
	  "iss": "https://sts.windows.net/ac53999f-a70c-47f1-922e-940951b98666/",
	  "iat": 1418851141,
	  "nbf": 1418851141,
	  "exp": 1418855041,
	  "ver": "1.0",
	  "tid": "ac53999f-a70c-47f1-922e-940951b98666",
	  "amr": [
	    "pwd"
	  ],
	  "oid": "d5e00650-e2c4-4466-a190-c24b3c51cac5",
	  "upn": "jason@johnstonian.onmicrosoft.com",
	  "unique_name": "jason@johnstonian.onmicrosoft.com",
	  "sub": "bP6UoAwJUFgXDgAggnQdmQ-LmVEbqDLMneFoaqUTMt8",
	  "puid": "10037FFE8B3D76E8",
	  "family_name": "Johnston",
	  "given_name": "Jason",
	  "appid": "4750e881-5879-4bc7-b77f-3b6ab6772da8",
	  "appidacr": "1",
	  "scp": "Contacts.Read",
	  "acr": "1"
	}

Each one of the fields in this JSON represent a token claim. Azure has some [detailed documentation on all the different claims](http://msdn.microsoft.com/en-us/library/azure/dn195587.aspx), so I won't go into that here. I'll just focus on the ones you should be looking for when troubleshooting the token.

### aud ###

This should be `"https://outlook.office365.com/"` for the Mail, Calendar, or Contacts APIs. For the Files API, it should be a URL to the user's OneDrive. If it's something else, the token won't work. You may need to request a token for the appropriate resource using the refresh token.

### iat, nbf, exp ###

These claims indicate the time that the token was issued (**iat**), and the time span when the token is valid (**nbf** and **exp**). The **nbf** claim is the start time for the token's validity, and the **exp** is the end time. Their values are the number of seconds since 1970-01-010:0:0Z in Coordinated Universal Time (UTC). Make sure that you're using the token within its valid period. If it's expired, you need to request a new one with the refresh token.

### tid ###

This is a GUID that identifies the Office 365 Tenant of the logged in user. The value here should match the GUID in URL in the **iss** claim.

### upn, unique_name, family_name, given_name ###

These claims should correspond to the logged on user.

### appid ###

This should be the client ID of your app. You can verify the client ID in the Azure Management Portal.

### scp ###

This lists all of the permissions that your app has configured in its registration. MSDN has the [full list of available scopes](http://msdn.microsoft.com/en-us/office/office365/howto/application-manifest).

----------
Connect with me on Twitter [@JasonJohMSFT](https://twitter.com/JasonJohMSFT)

Follow the [Exchange Dev Blog](http://blogs.msdn.com/b/exchangedev/)