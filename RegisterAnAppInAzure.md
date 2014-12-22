# Manually Register an Office 365 API app in Azure AD #

Once you're familiar with the process, registering an app in Azure AD to enable access to the Office 365 REST APIs ([Mail](http://msdn.microsoft.com/office/office365/APi/mail-rest-operations), [Calendar](http://msdn.microsoft.com/en-us/office/office365/api/calendar-rest-operations), [Contacts](http://msdn.microsoft.com/en-us/office/office365/api/contacts-rest-operations), and [Files](http://msdn.microsoft.com/en-us/office/office365/api/files-rest-operations)) is a fairly straightforward process. However, if you've never done it before, it can be a little intimidating. Let's step through the process together. But first, let's briefly discuss **why** you have to do this in the first place.

## Why do I have to register? ##

The Office 365 REST APIs require authentication with an OAuth2 access token acquired from Azure AD. If you're not familiar with the OAuth2 flows, you can see all the gory details [here](http://tools.ietf.org/html/rfc6749). There's also a good overview of the [Authorization Code Grant Flow](http://msdn.microsoft.com/en-us/library/azure/dn645542.aspx) on MSDN. To summarize, you need a client ID, and for web apps, you also need a client secret. The client ID is a GUID issued by Azure that uniquely identifies your application. The client secret is also generated by Azure, and can be thought of as a password for the app.

So the answer to our question is: in order to get a client ID and secret from Azure, you have to register the app and provide some basic information about it, including what resources your app wants to access, and what permissions it needs.

## Where do I start? ##

First, you need access to an administrator account for an Office 365 Subscription. This can be an existing subscription you have for your work or business, or it can be an [Office 365 Developer Tenant](https://portal.office.com/Signup/MainSignup15.aspx?OfferId=6881A1CB-F4EB-4db3-9F18-388898DAF510&DL=DEVELOPERPACK&ali=1). You can even use the free developer tenant that [comes with your MSDN subscription](http://msdn.microsoft.com/en-us/subscriptions/jj919154.aspx) (if you have one). Just bear in mind that whatever subscription you use to register your app will show as the publisher for that app when users are prompted to give access.

Next you need access to the [Azure Management Portal](https://manage.windowsazure.com). If you have an Azure subscription, you already have access and you're probably already familiar with the portal. If you don't, don't worry! You don't need an Azure subscription to register an app. Every Office 365 subscription comes with an Azure Active Directory, which is all you need. You can enable management portal access to your Azure AD fairly easily.

Just log on to Office 365 in your browser (I like to just log on to Outlook Web App) with an administrative account. You should see an "Admin" menu just to the left of your profile picture:

![](https://github.com/jasonjoh/office365-azure-guides/images/AdminMenuInOwa.png)

The "Azure AD" option in this menu will take you to a one-time registration page to activate portal access. When I did this for my MSDN developer tenant, I was presented with a big scary green button that said "Purchase". However, when I clicked on it I was never asked for a credit card or any sort of billing information. It just went through the process of enabling portal access. 

Once you have portal access, you're ready to register your app. 

## Registering your app ##

Log on to the Azure Management Portal. Look for your directory. If you have "All Items" selected on the left-hand side, you might see something like this. The value in the "Name" column may be different, but the important thing is that you're looking for the entry that has "Directory" in the "Type" column.

![](https://github.com/jasonjoh/office365-azure-guides/images/AzureDirectory.png)

Click on that entry and your view changes. Click on "Applications" in the top bar.

![](https://github.com/jasonjoh/office365-azure-guides/images/AzureDirectorySelected.png)

If this is your first app, you'll likely only see two entries here, one for Office 365 Exchange Online, and one for Office 365 SharePoint Online. Ignore those, and look for the "Add" button at the bottom.

![](https://github.com/jasonjoh/office365-azure-guides/images/AppList.png)

Click the "Add" button to start the wizard. When asked "What do you want to do?", choose "Add an application my organization is developing". This takes you to this screen.

![](https://github.com/jasonjoh/office365-azure-guides/images/AddAnApp.png)

Type a name for your app. Then you need to specify which type of app you're developing. "Web Application and/or Web API" is for exactly what it says. The "Native Client Application" choice is what you want for anything that isn't a web app or web API. This includes phone apps, tablet apps, etc. The experience is a little different for each choice, so we'll do both.

### Web app/Web API ###

If you choose a web app/API, the next screen will look like this.

![](https://github.com/jasonjoh/office365-azure-guides/images/WebAppNextStep.png)

Here's what you should put in these fields.

- **Sign-on URL**: I recommend putting the URL to the home page for your app. You can change this anytime, so you can use a test server for now, or localhost if you're developing on your local machine.
- **App ID URI***: This is a unique identifier for your app, in the form of a URI. It isn't a real web address. I recommend using your Office 365 domain + a unique name for your app. For example, I used "https://johnstonian.onmicrosoft.com/ManualRegistration". 

Click the check button to complete the wizard.

### Native client application ###

If you choose a native application, the next screen will look like this.

![](https://github.com/jasonjoh/office365-azure-guides/images/NativeAppNextStep.png)

Here all you need is the **Redirect URI**. This is similar to the **App ID URI** for a web app. It isn't a real web address, it is more of a unique identifier for your app. I recommend the same construction for this field: your Office 365 domain + a unique name for your app.

Click the check button to complete the wizard.

## Configuring your app ##

At this point your app is registered, but you need to do some additional configuration to enable access to the Office 365 APIs. If you've just completed the registration process, you should now be at the Quick Start page for the app. Depending on if you chose a web app or a native client app, the Quick Start looks a little different, but for either choice you should see a "Configure" option near the top.

![](https://github.com/jasonjoh/office365-azure-guides/images/AppQuickStart.png)

Click there to configure the app. The configure pages are different for web apps and native apps. However, they are similar enough that I'll just list each field and call out if it is web app or client app only. The important fields for Office 365 access are:

- **Application is multi-tenant** (web app only): This defaults to "No". Leave it to "No" if you only want users in your Office 365 organization to be able to sign on to this app. If you intend to make it available outside your organization, set it to "Yes". You can change this setting later, so if you want to leave it to "No" while you are developing the app, that's perfectly fine.
- **Client ID**: This is a crucial part of the OAuth2 flow. You'll want to copy this value so your app can use it to request authorization.
- **Keys** (web app only): This is the other crucial part of the OAuth2 flow for web apps. This is where you generate what is commonly referred to as your client secret. Use the drop-down to select a one-year or two-year key. Notice that the value doesn't show up right away. You have to click the "Save" button to display the key. You can do this after you're done configuring the app. **However, it is important to note that this key will only ever display the one time, after you click "Save". If you don't copy the value then, you cannot retrieve it. You will have to generate a new one.** 

![](https://github.com/jasonjoh/office365-azure-guides/images/NewClientSecret.png)

- **Reply URL** (web app only): This is set to the value you specified for **Sign-on URL** when creating the app registration. You can modify it or add new reply URLs here. This will be the root of your OAuth2 redirect URLs. If your app specifies a redirect URL (in the redirect_uri parameter of the [authorization code request](http://msdn.microsoft.com/en-us/library/azure/dn645542.aspx)) that is not based on one of the values set here, Azure will return an error.
- **Redirect URIs** (native app only): This is similar to the **Reply URL** for web apps. You will use this value when requesting authorization tokens.
- **Permissions to other applications**: This is where you specify which resources your app needs to access. Let's take a close look at this section.

### Setting permissions ###

For a new app, you should see something like this.

![](https://github.com/jasonjoh/office365-azure-guides/images/AddPermissions.png)

You have an entry for Windows Azure Active Directory, with one delegated permission. That permission allows the app to sign in as the user and read the user's profile. You can add additional permissions to Azure AD here if you want to use the Graph API, but we're here to talk about Office 365. Click the "Add Application" button to add Office 365 services. That brings up a list of applications to choose from.

![](https://github.com/jasonjoh/office365-azure-guides/images/ChooseApps.png)

The "Office 365 Exchange Online" choice will allow your app to access the Mail, Calendar, and/or Contacts APIs. The "Office 365 SharePoint Online" choice will allow your app to access the Files API. Click one (or both!) and click the check button to complete your selection and return to the configuration screen. You should now have a new entry for each service you selected.

![](https://github.com/jasonjoh/office365-azure-guides/images/ExchangeAdded.png)

Click the drop-down for the permission type you want to give. Briefly, the difference between the two:

- **Application permissions** (Not yet implemented for Office 365): The permission is granted to the application itself for all users in the organization. Once an administrator gives consent, individual users do not have to sign in for the app to be able to access their mailbox or OneDrive.
- **Delegated permissions**: The permission is granted for the application to act as the user in their mailbox or OneDrive only. Each user must sign in to give consent to their own mailbox or OneDrive.

Select the appropriate permissions for your application. Bear in mind that if you select a "read and write" permission (say to the user's mail), you do not have to also select the "read" permission. Doing so won't break anything, but it's just not necessary!

Once you have the permissions set the way you want, click the "Save" button to finish. You can always go and adjust permissions later if needed. Any users that consented before you change will be re-prompted for consent with the adjusted permissions the next time they sign in. If you created a new key to use as a client secret, be sure to copy it now.

## Finishing up ##

That's it! You're done! You should have a client ID and a client secret (if you're developing a web app). Time to start developing!


----------
Connect with me on Twitter [@JasonJohMSFT](https://twitter.com/JasonJohMSFT)
Follow the [Exchange Dev Blog](http://blogs.msdn.com/b/exchangedev/)