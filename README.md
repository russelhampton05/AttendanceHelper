# AttendanceHelper

##What

Attendance helper is a simple program that is meant to help my wife get attendance for her class.


##How
Attendance helper offers simlpe UI, user credentials encryption, and a POST/GET. The attendance window dynamically populates the student list in real time, gathering data from the school's records using the login credentials provided. Students click on their name to indicate that they are present, and then on a timer the teacher is emailed.

##Future plans
If I come back to this project, I'd like to get automated attendance submitting working. Currently the site uses JS and ajax posts to submit the information, and this is difficult to achieve with .NET's httpclient.


##Use of this code
I am the writer of this code unless otherwise indicated in the codebase. I cited pieces that I borrowed from outside sources, notably the encryption and the XAML for the attendance window. All sources cited in code. Feel free to use this code in anyway. Note that the program is currently set to interact with a specific school's website system! Abstracting out the specific post/get addresses and navigation wouldn't be difficult with the current codebase. It's for this reason that I didn't release a build.



