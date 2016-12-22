# MultiplayerGameBackbone
This Application uses a database to manage players that are active.

*********************************************************************
Program: Multiplayer Game Backbone 
Author: Bobby Thorne
Contact Info: bobby.a.thorne@gmail.com
*********************************************************************
Program Intro:

This is a start of an application that uses a database to keep track 
of users info that are currently logged in. As admin you can see
others locations and will update in real time but currently the 
method of how to handle adding others to the canvas is not 
implemented. 

There is no way to make other users admins unless if you manually 
update the sql file but the one currently on file is:
username: bobthorne 
password: newuser

#Must run batch file to create database.
*********************************************************************
Features: 

This application has a Login front that uses a 
hashpassword Sha256 and user passwords can be updated
when logged in. There is a table that that has question 
and answer incase user forgot their password but it is currently
not in use.

Once logged in, player is able to navigate character with the arrow
keys which in the future will allow a keybinding setting. While 
logged in the user is allow to see who are on the server and also
their current item list.

As an admin you are able to see players on server and also player info
such as id number and health and location. The admin also has a console 
line that is visible when pushing the insert button on keyboard.
This will allow admin to call certain commands such as additem when 
implemented. The console line will need to take in arguement and 
seperate the text to see what process it does.

**********************************************************************
Applications: 

What I see for this program could be a simple multiplayer coop game
such as a mario style missions or a dungeon scroller or even a
chat room.


**********************************************************************
Potential Issues:

Currently how the logic handles jumping it only allow a certain height
which if you add blocks to jump on there will be an issue of jumping 
on a box that is over the max height.

**********************************************************************
Bugs: 

In some instance the beginning logo will show up while in game but 
can easily be removed by pausing have not able to recreate to figure
out logic needed to prevent this from happening.

