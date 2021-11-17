# ﻿Diablo II : Resurrected - Multi Instance Manager README

## Credits:

> Huge credits to the original work of [@Sunblood](https://github.com/Sunblood/) for creating the work upon this project is based, a very nice tool written in AutoIT
> This project is in many ways a re-engineerd, rewritten and C# ported version of Sunblood's original work.
> https://github.com/Sunblood/D2RML/

## Donations:
*	Bitcoin: 1P7Sbra6cNEDYzRQANer8SdmqyeLLQSDKL
*	Ethereum: 0x4A16f62C9FC337606040f69a2044Da81C6ef7bcD
*	Cardano: addr1qyers7tzkx8z4pmat0a3xg8g5dhdhguehmsnqns7dg84uzpj8puk9vvw92rh6klmzvsw3gmwmw3en0hpxp8pu6s0tcyqxkhsa6
*	FG: D2JSP Forum Gold Donations Welcometo [Farmith](https://forums.d2jsp.org/user.php?i=1261112)

## Discord:

If you want to chat about the app, have feature requests you wish to brainstorm or just feel like saying hi feel free to join the discord channel.
https://discord.gg/fCRxGXQWcP

## Preparation:
~~If you have never used the accompanying handle64.exe you need to manually run that executable once, and accept the terms of use for it before beginning.~~
No unneccessary preparations needed, maybe a soft drink and a sandwich?

## Tutorial:

For each account you wish to use, perform the following steps:
	
	1. Click "Add Account" button 

	2. Your battle net launcher will boot, log in with your credentials.

	3. Press "Play Game" button in the launcher

	4. Wait (and click frenetically as we all do) untill you are connected to your online account and can see your characters

	5. Close the game (The battle.net launcher will already have closed automatically)

Once you have added all your accounts (They will be visible in the 'Accounts:' list view) you can check one or several of these accounts that you wish to launch,
launch them with the "Launch" button.

They will now login, one after the other by using hidden one-time keys which you generated while doing the "Add Account"-part.

### Note: The one-time keys do not last forever, occasionally you will get "Could not connect to Server", in which case you need to mark the affected account(s) and press "Refresh" which sets up new one-time keys by a similar procedure to the initial setup.


Enjoy!




FAQ:
	
	* What does the "Dump RegKey" button do?
		A) it dumps (duh) the current token the gameclient has stored into dump.bin, this can be used as a backup-way of 
			accessing the launch procedure, should the automatic way fail. The way you do it in this special case is:
			1. Login to the game normally through battle.net launcher (without using this tool)
			2. Start the game with the proper button
			3. Wait untill you are logged into character-select in the game client
			4. Press "Dump RegKey"
			5. Rename dump.bin to a name you prefer for this particular account and start D2RMIM, you can now press Launch with the proper name.
			
	* Does this application save my username/password in any way?
		- A) If you choose to use the "Automation" feature, your username and password will be saved into your windows credential store, should you wish to use it, 
			but usage of that feature is completely optional.

	* How does this app work?
		- A) It uses one-time hashes/keys stored by the D2R client to authenticate just like it normally does

	* It doesnt work! (More of a statement than a question tbh)
		- A) Your one-time keys may be outdated, if you launch battle.net normally and login to your account, you will need to refresh the tokens for the account 
				once you want to multi-box again. use the "Refresh" button after selecting affected account(s).
		  A) Run the program as administrator, maybe that helps
		  A) What a pity..





## Version log:

* 1.6.2	**Minor release 1.6.2***

	-	Bugfixes:
			* Fixed pre-launch commands (now working in experimental mode)
			* Fixed post-launch commands (now working in experimental mode)

	-	New features:
			* Attempt for spawned processes to become de-elevated (in use for pre and post launch commands)

	-	General improvements:
			* Moved .bincnf to .cnf (profile configuration files) in an attempt to solve a strange bug where some people perhaps see the .bincnf in the profile listing on the mainUI aswell
			* Changed logclears to be just separations for debug.log
			* Added a new logclear (empty) which is executed every time D2RMIM is started instead, for debug.log
			* Added full date + time additions to debug.log

* 1.6.1
	-	**Bugfix release**
			* Fix for wrong client executable being still referenced in the project.
			* Fix for wrong client installation path still referenced.
			* Fix for wrong command-line arguments still referenced.
			* Stability-fix for profile deletion.
			* Added removal of credentials upon removal of profile
			* Start of renaming of "Account" things to "Profile" in the cases where it really is "Profile" things and not "Account" things.
			* Added true-password mode for password-field in account-setup

* 1.6
	-	**Major Revamp**
	
	-	Bugfixes:
	-	* Fixed problem where setup of new accounts would take longer than needed, by removing handle-kill from procedure.
	-	* Fixed a hotkey problem that hotkeys were in numlock mode and were sending "drink potion" and other bad things to other clients.
	-	* Fixed a problem where setting window-title would sometimes fail because the handle wasn't yet created properly.

	-	New features:
	-	* Added new UI/functionality to configure accounts on a per-account basis (Configure-button)
	-	* Added new functionality to enable/disable grouping of client windows in the taskbar on a per-client basis.
	-	* Added new functionality to manually choose installationpath/executable name of battle.net/D2R on a per-client basis.
	-	* Added new functionality to process/configure pre-launch commands to be executed before the client on a per-client basis.
	-		* Currently disabled
	-	* Added new functionality to process/configure post-launch commands to be executed after the client on a per-client basis.
	-		* Currently disabled
	-	* Added new functionality to process/configure 'command-line arguments' on a per-client basis.
	-	* Added new functionality to select client position on screen on a per-client basis.
	-	* Added new functionality to select/configure client Realm on a per-client basis.
	-	* Added new functionality to process/configure D2R Client 'Settings.config' on a per-client basis.
	
	-	Improvements:
	-	* Added completely new functionality for handling a per-client based hotkey system that allows modifier keys: ctrl, shift, alt.
	-	* Added more Log functionality to catch unforseen errors.
	-	* Faster "skip intro videos" method added
	-	* Skip-intro videos now on a per-client basis configuration + uses other methods of skipping them to not lock up mouse.
	-	* Faster and more secure way of keeping track of handles.

	-	ToDo:
	-	* Window positioner needs a "width" and "height" system to allow inactive windows to not take up so much space/resources, 
			this task is in a "long term"-watch	state as it needs many things to fall into place first.

* 1.5.1
	-	**Bugfix & Refactoring release**
	-	Bugfixes:
	-	* Fixed problem with "Add Account" button not listening to current settings
	-	* Fixed problem with "Launch" button only being able to launch one client 
	-	* Fixed problem with "Refresh" button only being able to launch once
	-	* Fixed problem with memory leak in handle manipulation
	-	* Fixed problem with a few blocking calls
	-	Refactoring:
	-	* Created a slew of new helpers and shit to clean up this mess of a project and make things readable and debug-able
	-	* Split up window features, process features, automation features and general logic
	-	New functionality:
	-	* Added auto-skipping of "videos" at the launch of the game client in Setup, Launch & Refresh-modes
	-	* Added better closing mechanisms for launchers, client launchers, and clients.
	-	* Added functionality to keep track of which EXE-name we are working with to kill the right thing with mutex/process handling
	-	* Added task-functionality to launch button to follow same suite as Add and Refresh i.e block buttons untill done and not blocking UI thread

* 1.5
	-	Auto-login is working (experimental mode, let me know how it works for you)
	-		Auto-Login uses the built-in windows credential system to save credentials locally on your computer. if you don't want your credentials saved on your computer, just don't use the feature.
	-	Major stability improvements, UI no longer freezes while refreshing/adding accounts
	-	Usability upgrades, buttons become grayed out if D2RMIM is in the middle of tasks, wait for the buttons to become available or restart app if need be.

* 1.4.2
	-	Feature release, added global hotkeys (which can be disabled in the .config file) per default: numlock enabled -> numpad 0 = first client, numpad 1 = 2nd client, numpad 2 = third client
	-	Check the changes in MultiInstanceManager.config for adding less/more hotkeys, setting them to enabled="false" disables them and enabled="true" enables them.

* 1.4.1
	-	Minor bugfix release, removed a bug from the "Add Account" button

* 1.4
	-	Added functionality to allow command-line arguments to the clients.

* 1.3
	-	Removed dependency for Handle64.exe

* 1.2 
	- 	Buggfix sorting "Setting up several accounts in one flow", saving the tokens in a more reliable fashion.

* 1.1
	-	 General Bugfixes and attempts at bugfixes

* 1.0

	- 	First "official" release of D2RMIM, sort-of works at this point, sometimes.

Note: When first launching D2RMIM and going through setup, don't be alarmed if the game client + bnet launcher exit out once setup, just press "Launch" if you just with to use 1 single account.

The closing of bnet + client is to allow multiple accounts to be setup easily without manually closing stuff down, when using "Launch" stuff doesn't get closed down arbitrarily.

Note: There is a "Dump RegKey" button still on the gui in this release, this is mostly for debug-purposes tho so don't worry about it, all it does is dump the web-keys for D2R into "dump.bin", nothing more nothing less.

## TODO:

*	Add more configuration options
*	Find a faster way to "refresh" accounts
