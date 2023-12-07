Summary and Goals 

The goal of the app is to view various forms of data (text, image as of now) from a source. Currently the source is a Raspberry Pi (RPI) doing object detection using TF llite models. The app can receive data in the form of pictures, text and videos from the RPI in real time. 

The current state of the app is to focus on receiving data from one source. In the future I’d like to expand this to multiple sources. For example different object detection models, sound recognition etc.. 

For the future state I would like to see the RPI act more as a server and I want to be receiving data from vehicle(s). I want to have a system of vehicles and a local server (RPI) with the capability of connecting to a cloud server. The app would then connect to the local server or the cloud server. As of now I think this approach or one similar would be best. The app could then access past data, not be constrained with storage (as much) if connecting to a cloud server is an option.


Development side ToDos 

App side (Net Maui (c#)) 

DONE Work in the Model-View-ViewModel architecture. DONE
•	Look into best practices, folder organization, passing in services or view models to views?
•	Is there a way to add a install the nuget packages and system settings? Something similar to pip in python? 
•	DONE Change configure connection process DONE

1)	DONE Connect to mqtt DONE
2)	DONE Test connection to mqtt with text and image DONE
3)	Load/create config (broker address and topics). Default located on app. Others to be loaded via mqtt 
	Create logic to modify config/logs 

Server side (python & RPI) 

•	Separate the camera interface (Open CV), MQTT from the model 
o	Have a camera interface class that can display simple streams to test camera and connectivity 
o	DONE MQTT client to comply with testing the connection to the app DONE
o	DONE Will have two separate python files along the lines of test_stream and test_mqtt DONE Note mqtt client is separated in the app so we can test connectivity separately DONE
•	Config creation
o	Seems easier to send the config to the app instead of having them be created by the user or stored locally on the app. 
o	Also have the ability to select different models. 
•	Models: 
o	Train in the vineyard specific model 
o	Also have our model class a bit more abstract so that we can incorporate different models with different inputs /outputs 
o	Differrrent model? Looking at yolov8 using coral here https://github.com/ultralytics/ultralytics/issues/4089 

Known issues 
	App  Net Maui 
•	Loading logs is leading to a crash. Will need to separate the log receiving logiv and display, I think for now the same var is handling both 
•	The toggle stream is working but inconsistently. Resource issue? 
•	The display photos page is displaying the test photo 
•	Stream and photos display bounding boxes not specified in selected labels (in config) 
