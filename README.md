# Al Studente

Al Studente is an ASP.NET MVC web application designed for private music lessons teachers who need a place to organize and track their student roster. Users can add new student profiles to their student list which is ordered by their scheduled weekly lesson times. On the details page of each student, teachers can add notes to help track the student's progress and other relevant information. Future versions will implement student user features including teacher - student lesson tracking and interaction.

## Running Al Studente

Al Studente is not a deployed application. To run the program:
  - Download the project files from GitHub and open AlStudente.sln in Visual Studio. 
  - Run the SQL scripts to create a local database and seed data. 
  - Create a project in Google Firebase that uses Email/Password authentication. 
  - add the Firebase api key and connection string to your local appsettings.json file. 
  - Run the program from Visual Studio as FirebaseMVC, which will launch your default
    browser utilizing local ports 5000 and 5001.


## Al Studente ERD

![Al Studente ERD](/AlStudenteERD.png)