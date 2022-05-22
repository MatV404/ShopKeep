To create the database and seed it, open the CMD in this folder and enter
dotnet ef database update ShopKeepDB-Migration -s Migration -p ShopKeepDB

You should also move appsettings.json from Migration to ShopKeep/bin/x86/Debug/AppX 
for the connection to be established (it's where configuration reads from :-( ).