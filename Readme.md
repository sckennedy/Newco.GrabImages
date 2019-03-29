## NewCo.GrabImages

This is a dotnet core application and to run it, you will need the dotnet core runtime or SDK installed on the machine where it is running.  

[Click here for download](https://www.microsoft.com/net/download/dotnet-core/2.1 "dotnet core download")

To run this you will need to pass in the full path to the folder where you want the images extracted to and secondly the full path to a text file that contains the CEC users ids of the images you want to download.

For example.
The usernames are in a file called usernames.txt at c:\Temp
Output should be sent to c:\Temp

To run the extract run the following from the command line whilst in the folder where this .dll file is

`
dotnet GrabImages.dll C:\Temp C:\Temp\usernames.txt
`

If any images are not found or there is an error downloading an image, a error file fill be created and saved to the same location as the images, i.e. C:\Temp in this example.