# Site Map Generator Tool

Tadataka was developed as a final year project for my Computer Science BSc at the University of Leeds.

Site maps can be used to aid in the navigation of a website, exploration of a website hierarchy, or increasing the quantity and quality of traffic through a website. The motivation for this project is to allow users to obtain site maps and other relevant information in a way that is intuitive, fast, free to use, and provides a better service than the existing alternative solutions.

Another key motivation for the project is the development of my own skills and knowledge. The breadth of scope for this project will allow me to improve my programming skills and expand my knowledge in fields such as graph algorithms, web-crawling, and site map protocols.

## Build Status
|Action|Status
|-|-
|Build|[![Build](https://github.com/tomoddy/SiteMapGeneratorTool/actions/workflows/build.yml/badge.svg)](https://github.com/tomoddy/SiteMapGeneratorTool/actions/workflows/build.yml)
|Unit Tests|[![Unit Tests](https://github.com/tomoddy/SiteMapGeneratorTool/actions/workflows/unittests.yml/badge.svg)](https://github.com/tomoddy/SiteMapGeneratorTool/actions/workflows/unittests.yml)
|Automation Tests|[![Selenium Tests](https://github.com/tomoddy/SiteMapGeneratorTool/actions/workflows/seleniumtests.yml/badge.svg)](https://github.com/tomoddy/SiteMapGeneratorTool/actions/workflows/seleniumtests.yml)

## Build Instructions

The sections below detail various methods for downloading and running the application code.

### Prerequisites

The following services need to be configured before the application can be run. Once the services are configured, the missing values from the ```firebase-git.json``` and ```appsettings-git.json``` files need to be added and the files need to be renamed ```firebase.json``` and ```appsettings.json``` respectively.

#### AWS
An Amazon Web Services account is required with access to S3 and SQS. The access key, secret key, and account ID for the AWS account are required. An S3 bucket needs to be created; the bucket name is required. An SQS FIFO queue needs to be created; the service URL and queue name are required.

#### Firebase
A Google account is required with access to Firebase. The type, project ID, private key ID, client email, client ID, authentication URI, token URI, authentication provider certification URL, and client certification URL from the server account credentials are required. The Firebase keypath, database name, and system collection are required.

#### Email Account
An email account with SMTP access needs to be created. The username, display name, password, host address, and port number are all required.

#### VAPID
A set of VAPID keys need to be created for the notification system, from which the subject, public key, and private key are required.

### Visual Studio

The application code can be downloaded and opened in Visual Studio. Once the application is open in VS, running the application is trivial.

### Command Line

The .NET CLI can be used to build and run the application. To build the application, navigate to ```/SiteMapGeneratorTool``` and run the ```dotnet build``` command.

To run the application, navigate to ```/SiteMapGeneratorTool/SiteMapGeneratorTool``` and run the ```dotnet run``` command.

To run the unit tests, navigate to ```/SiteMapGeneratorTool/SiteMapGeneratorToolTests``` and run ```dotnet run``` command.

To run the automation tests, navigate to ```/SiteMapGeneratorTool/SiteMapGeneratorSelenium``` and run ```dotnet run``` command.

### Hosted Version

The application is hosted on Azure. This version of the application is fully featured and free to use. The hosted application is available at https://tadataka.azurewebsites.net/.
