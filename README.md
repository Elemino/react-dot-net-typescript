# Example Financial App 🔥


### Dependencies

Ensure the following are installed on the machine that will be used for running the application:

- Node (https://nodejs.org/en/)
- Docker (https://www.docker.com/get-started)

### Running the application

To run the code challenge application:

### `npm install`

This will:

- Install Node dependencies from package.json
- Install .NET Core dependencies from api/api.csproj and api.test/api.test.csproj (using dotnet restore)

Then run:

### `npm start`

This will start the app and it will be viewable at http://localhost:5000

***


### Introduction

An example of a frontend that I've built for a financial web application that creates a simple dashboard for basic financial information as part of a code challenge.  It is one of the most fun projects I've ever worked on and I've finally figured out how to run it without errors. The dashboard tracks basic financial information and is organized into four departments:  Products, Marketing, Sales, and Services.  For each calendar month since January 2016, a monthly amount is determined for each department's budget and a monthly amount for that the department actually spent.  Monthly mounts are already planned for each department through to December 2019.  The monthly amounts for actual expenditures are added at month-end.

The dashboard consists of four main charts that aggregate this information:

* Calendar Year to Date Budget by Department (pie chart)
* Calendar Year to Date Expense by Department (pie chart)
* Month by Month Budget over last 12 months (bar chart)
* Month by Month Expenses over last 12 month (bar chart)

What's included:

* A design and implemention of a front end and REST API with an underlying database that's used to generate the dashboard information.

The database schema includes the following information:
* Calendar month
* Calendar year
* Department
* Amount budgeted
* Amount spent

NB:  For the amount spent during a calendar month, a database entry is updated at the end of the month.  Amount is NOT updated during the month.

The application performs these functions:

* Authenticate using a local username/password scheme; 
* View a series of default dashboard graphs; and
* Select other dashboard graphs from a selection list


**Implementation Notes**<br/>
The 4 main charts are:

* Calendar Year to Date Budget by Department (pie chart)
* Calendar Year to Date Expense by Department (pie chart)
* Month by Month Budget over last 12 months (bar chart)
* Month by Month Expenses over last 12 month (bar chart)



Available options for date periods are:

* Calendar Year to Date
* Quarter to Date
* Last Calendar Year
* Last Quarter
* Last Month



To do:

- [ ] Deploy to a cloud service provider