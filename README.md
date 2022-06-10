# GameWatch
GameWatch is a little project for me to monitor how much time i spend on playing games, as to limit how much time i spend at a computer. It is much like what most phones have these days, where you can limit how much screen time you spend. What it does is basically just watching the processes thats running on your computer and if a given process is running, it counts down a timer. When that timer reaches zero, it gives a notification for you to stop playing. You can either make allowed times for different games, or you can use the overlay process of steam (`gameoverlayui.exe`), as an example, to get how much time you have spend in all games in general.

<p align="center">
  <img src="https://user-images.githubusercontent.com/22596587/173028659-a1f265e7-bf0f-4b4b-bc80-01536f96503d.png" height=260>
  <img src="https://user-images.githubusercontent.com/22596587/173028670-a769fa95-f9ce-45d0-af62-f1930bd58cf9.png" height=260>
  <img src="https://user-images.githubusercontent.com/22596587/173028680-8e1da160-1b54-4dd6-9969-307d69cd6370.png" height=260>
</p>

While it was intended for use on games, it can essentially be used on any application on your computer.

## How to Use
The application is a little window that is bound to your task tray in windows. To open it, double click on the icon in the tray. To hide the window again, simply click anywhere else on your computer. You can also exit the application by right clicking on the tray icon and click exit.

First you need to setup some watchers, this is done from the Settings menu that pops up if you click on the cog icon in the corner of the main window. Here you can add new watchers, by giving the group a name, followed by a list of different processes you want to monitor (comma seperated string). Lastly you can type in how much time you want yourself to allow in that application. Clicking accept in the bottom of the settings window will take you back to the main view and the watcher will now be running in the background.
