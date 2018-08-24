## CEC Images Extractor

To run thism you will need to pass in the full path to the folder where you want the images extracted to and secondly the full path to a text file that contains the CEC users ids of the images you want to download.

For example.
The usernames are in a file called usernames.txt at c:\Temp
Output should be sent to c:\Temp

To run the extract run the following from the command line whilst in the folder where this .exe file is
```
LdapQuery.exe C:\Temp C:\Temp\usernames.txt
```
If any images are not found or there is an error downloading an image, a error file fill be created and saved to the same location as the images, i.e. C:\Temp in this example.