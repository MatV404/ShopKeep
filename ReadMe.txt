To create the database and seed it, open the CMD in this folder (the root of the project) and enter
dotnet ef database update ShopKeepDB-Migration -s Migration -p ShopKeepDB

You should also move appsettings.json from ShopKeep/Migration to Shopkeep/ShopKeep/bin/x86/Debug/AppX 
for the connection to be established (it's where configuration reads from, it seems), otherwise the application
won't let you log in and will report a database error occuring.

The Migration project exists only to allow for the migration of the database.
The connection string I left in here is set for the default postgres user with password, well, password.

You should then be able to log into the app as:
Username: Admin
Password: Admin

(Note: The seeded data is rather minimal, so you will have to create your own items and shops, but it should be easy
with the UI. Additionally, you will need to register your own user account to test purchases in shops.)
================================================================================================================================================
What you can do in the app:
Login:
	You can log into the app as an admin or an user

Register:
	Register a new user account

Admin:
	View all shops and edit their stock (remove, reduce the amount, change the price, add new stock, generate stock, print stock into a text file)
	Create a shop, can generate random stock. Shop Type determines Item Types that are generated as stock (generation is optional).
	Manage Items, see all items, delete items, modify chosen item (click on it in the list view and open a new screen with the item's information).
	Create Item, create a new item, its types determine which shops can have it generated as their stock.
	Change Password, change your password.
	Manage Users, manage regular users, delete them, ban them, remove items from them, change their coin balance.
	Log Out, go back to Login

User:
	View all shops, purchase items in shops, sell items to shops.
	View Inventory, view all your items and balance.
	Change Password, change your password.
	Log Out go back to Login.
================================================================================================================================================
	