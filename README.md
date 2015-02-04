# WebBoggle
An online Boggle game made from these components:

Boggle Server: A game server implemented in C#. Manages scoring and game boards. Allows two players to play a game of 
Boggle from a network connection.

Boggle Connection: A connection class for communicating with Boggle Server from web Server. A BoggleConnection contains 
the game state (score, time remaining, etc). 

Web Server: A custom HTTP server for handling web requests and displaying the client view. Web Server maintains a 
collection of Boggle Connections - one for each user. 

Web Client: An webpage UI for the game. Talks to Web Server through ajax, Web Server talks to Boggle Server using Boggle 
Connection. 


To run local version: <br>
1 run Boggle Server: BoggleServer 200 dictionary.txt <br>
2 run Web Server: cd BoggleWebServer/BoggleWebServer/bin/Debug; BogleWebServer <br>
3 view client @ http://localhost:3012
