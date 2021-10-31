Diablo II : Resurrected - Multi Instance Manager README

Tutorial:

For each account you wish to use, perform the following steps:
	
	1. Click "Add Account" button 

	2. Your battle net launcher will boot, log in with your credentials.

	3. Press "Play Game" button in the launcher

	4. Wait (and click frenetically as we all do) untill you are connected to your online account and can see your characters

	5. Close the game (The battle.net launcher will already have closed automatically)

Once you have added all your accounts (They will be visible in the 'Accounts:' list view) you can check one or several of these accounts that you wish to launch,
launch them with the "Launch" button.

They will now login, one after the other by using hidden one-time keys which you generated while doing the "Add Account"-part.

Enjoy!




FAQ:
	
	Does this application save my username/password in any way?
		- A) Not unless someone else added something to it that wasn't there originally

	How does this app work?
		- A) It uses one-time hashes/keys stored by the D2R client to authenticate just like it normally does

	It doesnt work! (More of a statement than a question tbh)
		- A) Your one-time keys may be outdated, if you launch battle.net normally and login to your account, you will need to refresh the tokens for the account 
				once you want to multi-box again. use the "Refresh" button after selecting affected account(s).
		  A) Run the program as administrator, maybe that helps
		  A) What a pity..





Version log:

1.0:

First "official" release of D2RMIM, pretty much works at this point

Note: When first launching D2RMIM and going through setup, don't be alarmed if the game client + bnet launcher exit out once setup, just press "Launch" if you just with to use 1 single account.

The closing of bnet + client is to allow multiple accounts to be setup easily without manually closing stuff down, when using "Launch" stuff doesn't get closed down arbitrarily.

Note: There is a "Dump RegKey" button still on the gui in this release, this is mostly for debug-purposes tho so don't worry about it, all it does is dump the web-keys for D2R into "dump.bin", nothing more nothing less.