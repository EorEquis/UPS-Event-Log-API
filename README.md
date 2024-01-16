# UPS Event Log API

---

## Purpose

To read the WIndows Application log, and look for significant CyberPower UPS events :
* Utility power failure
* Utility power has been restored
* Local communication with the device has been lost
* Communication with the device has resumed
and produce a JSON payload via a REST API that provides to flags, a powerFlag and a commFlag, which can be used to identify if UPS status is currently good or bad.  (0 or 1)

This information will then feed the (Observatory COntrol Server)[https://github.com/EorEquis/TriStar-Observatory-Control-Server] as a "safety source".

## Current State

Eh.  It works.  It's ugly, but it works.

## To Do

* Maybe scrounge up another brand of UPS and see if we can do something similar.
* Make the whole thing much more configurable and dynamic, for example passing parameters for days back, and so on.
* Maybe make it a service?  Who knows.  NSSM will probably do for now.
	
## None of this works, my observatory exploded because your code sucks.

* Sucks to be you.  You downloaded someone else's code, free of charge, and trusted your observatory to it.  You get what you get.
* If you're not comfortable with placing such trust in a whole pile of shitty C# code, making your own tweaks, and DIY risks in general, this probably isn't the project for you.
* In short, I bear no responsibility for any outcome of your use of this code.  Use at your own risk!  	
