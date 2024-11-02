![alt text](https://github.com/denis-marchenkov/assets-dump/blob/0f9f9c4d77065dbfb9f13281341d515f7a8a6e13/works_on_my_machine.png)

# Table of Contents
- [Description](#description)
- [Data Structure](#data-structure)
- [Installation](#installation)
  - [Manual Installation](#manual-installation)
  - [Docker](#docker)

# Description

Example of a CRUD API for managing hospital patients.

<br/>

Solution consists of 6 projects:

<br/>

**Hospital.Presentation** - entry point for api;

**Hospital.Persistence** - database context;

**Hospital.Domain** - domain models;

**Hospital.Application** - commands and infrastructure;

**Hospital.Console** - helper console application for seeding and clearing database;

**Hospital.Tests** - unit tests.

<br/>

# Data structure

Patient record:
```json
{
    "name": {
        "id": "d8ff176f-bd0a-4b8e-b329-871952e32e1f",
        "use": "official",
        "family": "Family Name",
        "given": [
            "Name",
            "Surname"
        ]
    },
    "gender": "male",
    "birthDate": "2024-01-13T18:25:43",
    "active": true
}
```

![Database](https://github.com/denis-marchenkov/assets-dump/blob/master/hospital_database.png)

# Installation

You can run it manually in Visual Studio or using docker-compose.

Keep in mind there is a dependency on MsSql server.

There's a file ```Hospital.Console\patients_date_guid.txt``` containing hundred predefined patient IDs and birth dates. This data will be used during the seed process to allow easier api testing.

## Manual Installation

1) Download current repository by running: ```git clone https://github.com/denis-marchenkov/hospital-api.git``` over an empty folder;
2) Build ```Hospital.sln``` solution;
3) Adjust connection string in ```Hospital.Presentation\appsettings.json```, don't mind the DbType option - keep it sql;
4) Initialize DB by running EF migration in package manager console: ```Update-Database -Migration InitialMigration```;
5) Start application (startup projects by defaut are ```Hospital.Presentation``` and ```Hospital.Console```);
6) Seed database as described below.

By default swagger entry points are:
```
https://localhost:7031
http://localhost:5250
```

In order to seed database navigate to console window of ```Hospital.Console``` (run this project if it's not running already).

Console application takes in the following options:
```
-s [amount]    - seed N records (if used without [amount] - seeds 100 records)
-u [url]       - specify api url for creating patient. If specified, seed will use api endpoint to create patients, otherwise it falls back to entity framework.
-c             - clear database
-x             - exit
```

Example of seeding database:

```-s 100 -u https://localhost:5250/patients```

Or simply:

```-s```

## Docker
1) Download current repository by running: ```git clone https://github.com/denis-marchenkov/hospital-api.git``` over an empty folder;
2) In command line switch to ```Hospital``` solution root folder;
3) Run ```docker-compose up --build```
4) Seed database as described below.

<br />

Container structure:

![Database](https://github.com/denis-marchenkov/assets-dump/blob/master/hospital_container_structure.png)

<br />

In order to seed database attach to console container. You will be greeted with empty prompt, so press enter to trigger console output:

<br/>

![ConsoleAttach](https://github.com/denis-marchenkov/assets-dump/blob/master/hospital_console_attach.png)

<br/>

Seed commands are the same as described in manual installation, however keep in mind that the urls have changed since we're in docker environment now.
The api url will be ```http://webapi/patients``` since the default name of the service in ```docker-compose.yml``` is ```webapi```. So in order to seed database via url use:

```-s 100 -u http://webapi/patients```

Or simply:

```-s```

<br/>

Swagger url also changed and mapped to 5000 port (TLS amended):

```http://localhost:5000```

<br/>

- [Back To Top](table-of-contents)
