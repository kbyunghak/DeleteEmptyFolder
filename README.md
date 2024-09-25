1. Flow Chart
![image](https://github.com/user-attachments/assets/ae65be52-eb41-4e81-b1a2-e7e359267142)

2.	Description
a.	 What is it?
Delete Empty Folder is an application written in C# with the .NET framework. It generates a Windows Form to display a graphical interface the user can interact with. Given a file system path, the application accesses the path’s directories and its subdirectories to check if any folder or zip file needs to be deleted. Folders are deleted if they are empty. Zip files are deleted if they are empty. Also, the application checks the entries of each zip file. Delete Empty Folder deletes an entry in a zip file if the entry is empty or the entry contains only a header but no data.
b.	How to use it?
To use the Delete Empty Folder follow the steps below:
1.	Run the application
2.	Enter the path of the target folder
a.	If it is an invalid path, the application will output a warning in the text area. Check the input if this happens.
3.	Click delete
a.	The application will run and output messages in the text area as it deletes folders and files
b.	A message will be printed in the text area when the application is done
4.	Close the program after it is done or enter a new target folder path
Note: The application loads with a default target folder path. If you want to change the default path, edit the App.config file at line 7.
